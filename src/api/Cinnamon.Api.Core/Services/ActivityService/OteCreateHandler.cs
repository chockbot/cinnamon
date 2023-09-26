using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Interactors;
using Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.ActivityService;

public class OteCreateHandler : IOteCreateHandler
{
    private readonly IActivityData activityData;
    private readonly IGetProfileHandler getProfileHandler;
    private readonly IGenerateActivityHandler generateActivityHandler;
    private readonly ICustomerData customerData;

    public OteCreateHandler(IActivityData activityData, IGetProfileHandler getProfileHandler,
        IGenerateActivityHandler generateActivityHandler, ICustomerData customerData)
    {
        this.activityData = activityData;
        this.getProfileHandler = getProfileHandler;
        this.generateActivityHandler = generateActivityHandler;
        this.customerData = customerData;
    }

    public AppResult<OteCreateResult> Execute(OteCreateArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<OteCreateResult>.CreateFailed(ex, "An error occured in OteCreateHandler");
        }
    }

    public async Task<AppResult<OteCreateResult>> ExecuteAsync(OteCreateArgs args)
    {
        try
        {
            var isEmptyPricelist = args.Pricings.Count() == 0;
            if(isEmptyPricelist)
            {
                return AppResult<OteCreateResult>.CreateFailed(new ApplicationException("Invalid request."), "Invalid request.");
            }

            var currentUser = await this.getProfileHandler.ExecuteAsync(new AccountService.Interactors.GetProfileArgs {});
            if(!currentUser.Succeeded || currentUser.Result is null)
            {
                return AppResult<OteCreateResult>.CreateFailed(new ApplicationException(currentUser.Message), currentUser.Message);
            }

            var activity = args.Activity;

            var generateHandlerRes = await generateActivityHandler.ExecuteAsync(new GenerateActivityHandlerArgs {
                ActivityName = activity.EventName
            });
            if(!generateHandlerRes.Succeeded || generateHandlerRes.Result is null)
            {
                return AppResult<OteCreateResult>.CreateFailed(new ApplicationException(generateHandlerRes.Message), generateHandlerRes.Message);
            }

            var sortedPrice = args.Pricings.OrderBy(p => p.Price).ToList();
            var stringPrice = sortedPrice.Count > 1 ? string.Format("PHP {0} - {1}", sortedPrice.First().Price, sortedPrice.Last().Price) :
                string.Format("PHP {0}", sortedPrice.First().Price);

            var createOteRes = await activityData.CreateOteActivity(new Framework.ApiCommand.ApiData.Activity.Request.CreateOteActivityArgs {
                Activity = new Framework.ApiCommand.ApiData.Activity.Request.CreateOteActivityArgs.OteActivity {
                    BarangayCode = activity.BarangayCode,
                    BarangayName = activity.BarangayName,
                    CityName = activity.CityName,
                    CityNumber = activity.CityNumber,
                    CustomerId = currentUser.Result.Id,
                    Description = activity.Description,
                    EventName = activity.EventName,
                    ExperienceCreationTypeId = activity.ExperienceCreationTypeId,
                    ExperienceTypeId = activity.ExperienceTypeId,
                    Handler = generateHandlerRes.Result.GeneratedHandler,
                    HouseNo = activity.HouseNo,
                    IsPublished = activity.IsPublished,
                    PinnedLocation = activity.PinnedLocation,
                    PostalCode = activity.PostalCode,
                    Recurrence = activity.Recurrence,
                    RegionCode = activity.RegionCode,
                    RegionName = activity.RegionName,
                    ScheduleFrom = activity.ScheduleFrom,
                    ScheduleTo = activity.ScheduleTo,
                    StringPrice = stringPrice,
                    IsComingSoon = args.Activity.IsComingSoon
                },
                Pricings = args.Pricings.Select(p => {
                    return new Framework.ApiCommand.ApiData.Activity.Request.CreateOteActivityArgs.OtePricing {
                        Description = p.Description,
                        IsAbsorbFees = p.IsAbsorbFees,
                        MaxSlots = p.MaxSlots,
                        Price = p.Price
                    };
                }).ToList()
            });
            if(!createOteRes.Succeeded || createOteRes.Result is null || !createOteRes.Result.IsSuccess)
            {
                return AppResult<OteCreateResult>.CreateFailed(new ApplicationException(createOteRes.Message), createOteRes.Message);
            }

            // update customer status to maker
            if(!currentUser.Result.IsMaker)
            {
                var updateCustomerRes = await customerData.UpdateCustomer(new Framework.ApiCommand.ApiData.Customer.Request.UpdateCustomerArgs {
                    CustomerId = currentUser.Result.Id,
                    IsMaker = true
                });
                if(!updateCustomerRes.Succeeded || updateCustomerRes.Result is null || !updateCustomerRes.Result.IsSuccess)
                {
                    return AppResult<OteCreateResult>.CreateFailed(new ApplicationException(updateCustomerRes.Message), updateCustomerRes.Message);
                }
            }

            return AppResult<OteCreateResult>.CreateSucceeded(new OteCreateResult {Id = createOteRes.Result.Result.Id}, "One time event successfully created.");
        }
        catch (Exception ex)
        {
            return AppResult<OteCreateResult>.CreateFailed(ex, "An error occured in OteCreateHandler");
        }
    }
}