using System.Security.Claims;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Interactors;
using Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.ActivityService;

public class GetOwnedActivityHandler : IGetOwnedActivityHandler
{
    private readonly IGetActivityHandler getActivityHandler;
    private readonly IHttpContextAccessor httpContext;

    public GetOwnedActivityHandler(IGetActivityHandler getActivityHandler, IHttpContextAccessor httpContext)
    {
        this.getActivityHandler = getActivityHandler;
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

            var result = await getActivityHandler.ExecuteAsync(new GetActivityArgs {
                ActivityId = args.ActivityId,
                CustomerId = id,
                IncludeActivityAddress = args.IncludeActivityAddress,
                IncludeActivityDescription = args.IncludeActivityDescription,
                IncludeActivityImages = args.IncludeActivityImages,
                IncludeActivitySearchTags = args.IncludeActivitySearchTags,
                IncludeAtivitySchedules = args.IncludeAtivitySchedules,
                IsActive = args.IsActive,
                IncludeCustomer = args.IncludeCustomer
            });

            if(!result.Succeeded || result.Result == null)
            {
                return AppResult<GetOwnedActivityResult>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }

            var activity = result.Result;

            var activityEntity = new GetOwnedActivityResult {
                ActivityLevel = activity.ActivityLevel,
                AdditionalRequirements = activity.AdditionalRequirements,
                Address1 = activity.Address1,
                Address2 = activity.Address2,
                CanAdultsJoin = activity.CanAdultsJoin,
                City = activity.City,
                CityName = activity.CityName,
                Subdivision = activity.Subdivision,
                Region = activity.Region,
                RegionName = activity.RegionName,   
                Barangay = activity.Barangay,
                BarangayName = activity.BarangayName,
                PostalCode = activity.PostalCode,
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
                Handler = activity.Handler,
                IsSetSession = activity.IsSetSession,
                SessionName = activity.SessionName,
                PinnedLocation = activity.PinnedLocation,
                Status = activity.Status,
                ActivitySchedules = activity.ActivitySchedules != null ? activity.ActivitySchedules.Select(s => {
                    return new GetOwnedActivityResult.ActivitySchedule {
                        Id = s.Id,
                        DateTime = s.DateTime,
                        Name = s.Name,
                        PerUnit1 = s.PerUnit1,
                        PerUnit2 = s.PerUnit2,
                        Price = s.Price,
                        PriceUnit1 = s.PriceUnit1,
                        PriceUnit2 = s.PriceUnit2,
                        UnitPrice = s.UnitPrice,
                        Order = s.Order,
                        IsActiveSchedule = s.IsActiveSchedule
                    };
                }) : Enumerable.Empty<GetOwnedActivityResult.ActivitySchedule>(),
                Images = activity.Images != null ? activity.Images.OrderBy(i => i.Order).Select(i => {
                    return new GetOwnedActivityResult.ActivityImage {
                        Id = i.Id,
                        ImageSrc = i.ImageSrc,
                        Name = i.Name,
                        Order = i.Order
                    };
                }) : Enumerable.Empty<GetOwnedActivityResult.ActivityImage>(),
                Owner = activity.Owner != null ? new GetOwnedActivityResult.CustomerOwner {
                    Handler = activity.Owner.Handler,
                    Id = activity.Owner.Id
                } : null
            };

            return AppResult<GetOwnedActivityResult>.CreateSucceeded(activityEntity, "Successfully get owned activity");
        }
        catch (Exception ex)
        {
            return AppResult<GetOwnedActivityResult>.CreateFailed(ex, "An error occured in GetOwnedActivityHandler");
        }
    }
}