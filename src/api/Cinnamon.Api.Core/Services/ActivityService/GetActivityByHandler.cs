using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Interactors;
using Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.ActivityService;

public class GetActivityByHandler : IGetActivityByHandler
{
    private readonly IActivityData activityData;

    public GetActivityByHandler(IActivityData activityData)
    {
        this.activityData = activityData;
    }

    public AppResult<GetActivityByHandlerResult> Execute(GetActivityByHandlerArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;   
        }
        catch (Exception ex)
        {
            return AppResult<GetActivityByHandlerResult>.CreateFailed(ex, "An error occured in GetActivityByHandler");
        }
    }

    public async Task<AppResult<GetActivityByHandlerResult>> ExecuteAsync(GetActivityByHandlerArgs args)
    {
        try
        {
            var result = await activityData.GetActivityByHandler(args.Handler, 
                new Framework.ApiCommand.ApiData.Activity.Request.GetActivityArgs {
                    IncludeAddress = args.IncludeActivityAddress,
                    IncludeDescription = args.IncludeActivityDescription,
                    IncludeImages = args.IncludeActivityImages,
                    IncludeSchedules = args.IncludeAtivitySchedules,
                    IncludeSearchTags = args.IncludeActivitySearchTags,
                    IsActive = args.IsActive,
                    CustomerId = args.CustomerId,
                    IncludeCustomer = args.IncludeCustomer
                }
            );

            if(!result.Succeeded || result.Result == null)
            {
                return AppResult<GetActivityByHandlerResult>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }

            if(result.Succeeded && !result.Result.IsSuccess)
            {
                return AppResult<GetActivityByHandlerResult>.CreateFailed(
                    new ApplicationException(result.Result.ErrorInfo?.Message), "An error occured in GetActivityByHandler");
            }
            var activity = result.Result.Result;

            var activityEntity = new GetActivityByHandlerResult {
                ActivityLevel = activity.ActivityLevel,
                AdditionalRequirements = activity.AdditionalRequirements,
                Address1 = activity.Address1,
                Address2 = activity.Address2,
                CanAdultsJoin = activity.CanAdultsJoin,
                City = activity.City,
                CityName = activity.CityName,
                Subdivision = activity.Subdivision,
                Region= activity.Region,
                RegionName= activity.RegionName,
                PostalCode = activity.PostalCode,   
                Barangay= activity.Barangay,
                BarangayName = activity.BarangayName,
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
                CreatedBy = activity.CreatedBy,
                MarDetails = activity.MapDetails,
                IsSetSession = activity.IsSetSession,
                SessionName = activity.SessionName,
                ActivitySchedules = activity.Schedules != null ? activity.Schedules.Select(s => {
                    return new GetActivityByHandlerResult.ActivitySchedule {
                        Id = s.Id,
                        DateTime = s.DateTime,
                        Name = s.Name,
                        PerUnit1 = s.PerUnit1,
                        PerUnit2 = s.PerUnit2,
                        Price = s.Price,
                        PriceUnit1 = s.PriceUnit1,
                        PriceUnit2 = s.PriceUnit2,
                        UnitPrice = s.UnitPrice,
                        Order = s.Order
                    };
                }) : Enumerable.Empty<GetActivityByHandlerResult.ActivitySchedule>(),
                Images = activity.Images != null ? activity.Images.OrderBy(i => i.Order).Select(i => {
                    return new GetActivityByHandlerResult.ActivityImage {
                        Id = i.Id,
                        ImageSrc = i.ImageLocation,
                        Name = i.ImageName,
                        Order = i.Order
                    };
                }) : Enumerable.Empty<GetActivityByHandlerResult.ActivityImage>(),
                Owner = activity.Owner != null ? new GetActivityByHandlerResult.CustomerOwner {
                    Handler = activity.Owner.Handler,
                    Id = activity.Owner.Id
                } : null,
            };

            return AppResult<GetActivityByHandlerResult>.CreateSucceeded(activityEntity, "Successfully get activity");
        }
        catch (Exception ex)
        {
            return AppResult<GetActivityByHandlerResult>.CreateFailed(ex, "An error occured in GetActivityByHandler");
        }
    }
}