using System.Security.Claims;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Interactors;
using Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.ActivityService;

public class GetOwnedActivityByHandler : IGetOwnedActivityByHandler
{
    private readonly IGetActivityByHandler getActivityByHandler;
    private readonly IHttpContextAccessor httpContext;

    public GetOwnedActivityByHandler(IGetActivityByHandler getActivityByHandler, IHttpContextAccessor httpContext)
    {
        this.getActivityByHandler = getActivityByHandler;
        this.httpContext = httpContext;
    }

    public AppResult<GetOwnedActivityByHandlerResult> Execute(GetOwnedActivityByHandlerArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<GetOwnedActivityByHandlerResult>.CreateFailed(ex, "An error occured in GetOwnedActivityByHandler");
        }
    }

    public async Task<AppResult<GetOwnedActivityByHandlerResult>> ExecuteAsync(GetOwnedActivityByHandlerArgs args)
    {
        try
        {
            // get customer id saved in claims
            var customerId = httpContext.HttpContext?.User.FindFirstValue("UserId");
            if(customerId == null)
            {
                return AppResult<GetOwnedActivityByHandlerResult>.CreateFailed(
                    new ApplicationException("Unable to determine current account login"), "Unable to determine current account login");
            }
            int id = Convert.ToInt32(customerId);

            var result = await getActivityByHandler.ExecuteAsync(new GetActivityByHandlerArgs {
                Handler = args.Handler,
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
                return AppResult<GetOwnedActivityByHandlerResult>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }

            var activity = result.Result;

            var activityEntity = new GetOwnedActivityByHandlerResult {
                ActivityLevel = activity.ActivityLevel,
                AdditionalRequirements = activity.AdditionalRequirements,
                Address1 = activity.Address1,
                Address2 = activity.Address2,
                CanAdultsJoin = activity.CanAdultsJoin,
                City = activity.City,
                Subdivision = activity.Subdivision,
                Region = activity.Region,   
                Barangay = activity.Barangay,
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
                IsSetSession = activity.IsSetSession,
                SessionName = activity.SessionName, 
                ActivitySchedules = activity.ActivitySchedules != null ? activity.ActivitySchedules.Select(s => {
                    return new GetOwnedActivityByHandlerResult.ActivitySchedule {
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
                }) : Enumerable.Empty<GetOwnedActivityByHandlerResult.ActivitySchedule>(),
                Images = activity.Images != null ? activity.Images.OrderBy(i => i.Order).Select(i => {
                    return new GetOwnedActivityByHandlerResult.ActivityImage {
                        Id = i.Id,
                        ImageSrc = i.ImageSrc,
                        Name = i.Name,
                        Order = i.Order
                    };
                }) : Enumerable.Empty<GetOwnedActivityByHandlerResult.ActivityImage>(),
                Owner = activity.Owner != null ? new GetOwnedActivityByHandlerResult.CustomerOwner {
                    Handler = activity.Owner.Handler,
                    Id = activity.Owner.Id
                } : null
            };

            return AppResult<GetOwnedActivityByHandlerResult>.CreateSucceeded(activityEntity, "Successfully get owned activity");
        }
        catch (Exception ex)
        {
            return AppResult<GetOwnedActivityByHandlerResult>.CreateFailed(ex, "An error occured in GetOwnedActivityByHandler");
        }
    }
}