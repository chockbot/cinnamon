using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Interactors;
using Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.ActivityService;

public class OteUpdateHandler : IOteUpdateHandler
{
    private readonly IActivityData activityData;
    private readonly IGetProfileHandler getProfileHandler;
    private readonly IGenerateActivityHandler generateActivityHandler;

    public OteUpdateHandler(IActivityData activityData, IGetProfileHandler getProfileHandler, 
        IGenerateActivityHandler generateActivityHandler)
    {
        this.activityData = activityData;
        this.getProfileHandler = getProfileHandler;
        this.generateActivityHandler = generateActivityHandler;
    }

    public AppResult<OteUpdateResult> Execute(OteUpdateArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<OteUpdateResult>.CreateFailed(ex, "An error occured in OteUpdateHandler");
        }
    }

    public async Task<AppResult<OteUpdateResult>> ExecuteAsync(OteUpdateArgs args)
    {
        try
        {
            var isEmptyPricelist = args.Pricings.Count() == 0;
            if(isEmptyPricelist)
            {
                return AppResult<OteUpdateResult>.CreateFailed(new ApplicationException("Pricing must not empty. Invalid request."), "Pricing must not empty. Invalid request.");
            }

            var currentUser = await this.getProfileHandler.ExecuteAsync(new AccountService.Interactors.GetProfileArgs {});
            if(!currentUser.Succeeded || currentUser.Result is null)
            {
                return AppResult<OteUpdateResult>.CreateFailed(new ApplicationException(currentUser.Message), currentUser.Message);
            }

            var activityToUpdateRes = await activityData.GetActivityById(args.Activity.Id, new Framework.ApiCommand.ApiData.Activity.Request.GetActivityArgs {
                CustomerId = currentUser.Result.Id
            });
            if(!activityToUpdateRes.Succeeded || activityToUpdateRes.Result is null || !activityToUpdateRes.Result.IsSuccess)
            {
                return AppResult<OteUpdateResult>.CreateFailed(new ApplicationException("Can't find activity. Invalid request."), "Can't find activity. Invalid request.");
            }
            var activityToUpdate = activityToUpdateRes.Result.Result;

            // update handler only if have changes in activity title
            var handler = activityToUpdate.Handler;
            if(args.Activity.EventName.ToLower() != activityToUpdate.Title.ToLower())
            {
                var generateHandlerRes = await generateActivityHandler.ExecuteAsync(new GenerateActivityHandlerArgs {
                    ActivityName = args.Activity.EventName
                });
                if(!generateHandlerRes.Succeeded || generateHandlerRes.Result == null)
                {
                    return AppResult<OteUpdateResult>.CreateFailed(new ApplicationException(generateHandlerRes.Message), generateHandlerRes.Message);
                }
                handler = generateHandlerRes.Result.GeneratedHandler;
            }

            var sortedPrice = args.Pricings.OrderBy(p => p.Price).ToList();
            var stringPrice = sortedPrice.Count > 1 ? string.Format("PHP {0} - {1}", sortedPrice.First().Price, sortedPrice.Last().Price) :
                string.Format("PHP {0}", sortedPrice.First().Price);

            var entity = new Cinnamon.Framework.ApiCommand.ApiData.Activity.Request.UpdateOteActivityArgs {
                Activity = new Framework.ApiCommand.ApiData.Activity.Request.UpdateOteActivityArgs.UpdateOteActivity {
                    BarangayCode     = args.Activity.BarangayCode ?? string.Empty,
                    BarangayName     = args.Activity.BarangayName ?? string.Empty,
                    CategoryId       = args.Activity.CategoryId,
                    CityName         = args.Activity.CityName ?? string.Empty,
                    CityNumber       = args.Activity.CityNumber ?? string.Empty,
                    Description      = args.Activity.Description,
                    EventName        = args.Activity.EventName,
                    ExperienceTypeId = args.Activity.ExperienceTypeId,
                    Handler          = handler,
                    HouseNo          = args.Activity.HouseNo,
                    Id               = args.Activity.Id,
                    IsPublished      = args.Activity.IsPublished,
                    PinnedLocation   = args.Activity.PinnedLocation ?? string.Empty,
                    PostalCode       = args.Activity.PostalCode ?? string.Empty,
                    Recurrence       = args.Activity.Recurrence,
                    RegionCode       = args.Activity.RegionCode ?? string.Empty,
                    RegionName       = args.Activity.RegionName ?? string.Empty,
                    ScheduleFrom     = args.Activity.ScheduleFrom,
                    ScheduleTo       = args.Activity.ScheduleTo,
                    StringPrice      = stringPrice,
                    IsComingSoon     = args.Activity.IsComingSoon
                },
                Pricings = args.Pricings.Select(p => {
                    return new Framework.ApiCommand.ApiData.Activity.Request.UpdateOteActivityArgs.UpdateOtePricing {
                        Description = p.Description,
                        Id = p.Id,
                        IsAbsorbFees = p.IsAbsorbFees,
                        MaxSlots = p.MaxSlots,
                        Price = p.Price,
                        Name = p.Name
                    };
                }).ToList()
            };

            var updateOte = await activityData.UpdateOteActivity(entity);
            if(!updateOte.Succeeded || updateOte.Result is null || !updateOte.Result.IsSuccess)
            {
                return AppResult<OteUpdateResult>.CreateFailed(new ApplicationException(updateOte.Result?.ErrorInfo?.Message), updateOte.Message);
            }

            return AppResult<OteUpdateResult>.CreateSucceeded(new OteUpdateResult {Id = updateOte.Result.Result.Id}, "One time event successfully updated.");
        }
        catch (Exception ex)
        {
            return AppResult<OteUpdateResult>.CreateFailed(ex, "An error occured in OteUpdateHandler");
        }
    }
}