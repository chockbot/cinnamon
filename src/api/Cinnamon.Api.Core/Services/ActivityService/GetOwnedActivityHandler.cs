using System.Security.Claims;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Interactors;
using Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.ActivityService;

public class GetOwnedActivityHandler : IGetOwnedActivityHandler
{
    private readonly IActivityData activityData;
    private readonly IHttpContextAccessor httpContext;

    public GetOwnedActivityHandler(IActivityData activityData, IHttpContextAccessor httpContext)
    {
        this.activityData = activityData;
        this.httpContext = httpContext;
    }

    public AppResult<GetOwnedActivityResult> Execute(GetOwnedActivityArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<GetOwnedActivityResult>.CreateFailed(ex, "An error occured in GetOwnedActivityHandler");
        }
    }

    public async Task<AppResult<GetOwnedActivityResult>> ExecuteAsync(GetOwnedActivityArgs args)
    {
        try
        {
            // get customer id saved in claims
            var customerId = httpContext.HttpContext?.User.FindFirstValue("UserId");
            if(customerId == null)
            {
                return AppResult<GetOwnedActivityResult>.CreateFailed(
                    new ApplicationException("Unable to determine current account login"), "Unable to determine current account login");
            }
            int id = Convert.ToInt32(customerId);

            var result = await activityData.GetActivityById(args.ActivityId, new Framework.ApiCommand.ApiData.Activity.Request.GetActivityArgs {
                CustomerId = id,
                IncludeAddress = args.IncludeActivityAddress,
                IncludeDescription = args.IncludeActivityDescription,
                IncludeImages = args.IncludeActivityImages,
                IncludeSchedules = args.IncludeAtivitySchedules,
                IncludeSearchTags = args.IncludeActivitySearchTags,
                IsActive = args.IsActive
            });

            if(!result.Succeeded || result.Result == null)
            {
                return AppResult<GetOwnedActivityResult>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }

            if(result.Succeeded && !result.Result.IsSuccess)
            {
                return AppResult<GetOwnedActivityResult>.CreateFailed(
                    new ApplicationException(result.Result.ErrorInfo?.Message), "An error occured in GetOwnedActivityHandler");
            }
            var activity = result.Result.Result;

            var activityEntity = new GetOwnedActivityResult {
                ActivityLevel = activity.ActivityLevel,
                AdditionalRequirements = activity.AdditionalRequirements,
                Address1 = activity.Address1,
                Address2 = activity.Address2,
                CanAdultsJoin = activity.CanAdultsJoin,
                City = activity.City,
                CustomerBringWithThem = activity.CustomerBringWithThem,
                Description = activity.Description,
                District = activity.District,
                ExperienceCategoryId = activity.ExperienceCategoryId,
                ExperienceTypeId = activity.ExperienceTypeId,
                Id = activity.Id,
                IsPublished = activity.IsPublished,
                MinimumAge = activity.MinimumAge,
                Price = activity.Price,
                Remarks = activity.Remarks,
                SearchTags = activity.SearchTags,
                SkillLevel = activity.SkillLevel,
                SpecificsYouWillProvide = activity.SpecificsYouWillProvide,
                SubCategoryId = activity.SubCategoryId,
                Title = activity.Title,
                ActivitySchedules = activity.Schedules != null ? activity.Schedules.Select(s => {
                    return new GetOwnedActivityResult.ActivitySchedule {
                        Id = s.Id,
                        DateTime = s.DateTime,
                        Name = s.Name,
                        PerUnit1 = s.PerUnit1,
                        PerUnit2 = s.PerUnit2,
                        Price = s.Price,
                        PriceUnit1 = s.PriceUnit1,
                        PriceUnit2 = s.PriceUnit2,
                        UnitPrice = s.UnitPrice
                    };
                }) : Enumerable.Empty<GetOwnedActivityResult.ActivitySchedule>(),
                Images = activity.Images != null ? activity.Images.Select(i => {
                    return new GetOwnedActivityResult.ActivityImage {
                        Id = i.Id,
                        ImageSrc = i.ImageLocation,
                        Name = i.ImageName
                    };
                }) : Enumerable.Empty<GetOwnedActivityResult.ActivityImage>()
            };

            return AppResult<GetOwnedActivityResult>.CreateSucceeded(activityEntity, "Successfully get owned activity");
        }
        catch (Exception ex)
        {
            return AppResult<GetOwnedActivityResult>.CreateFailed(ex, "An error occured in GetOwnedActivityHandler");
        }
    }
}