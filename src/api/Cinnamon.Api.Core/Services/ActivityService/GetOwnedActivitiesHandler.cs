using System.Security.Claims;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Interactors;
using Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.ActivityService;

public class GetOwnedActivitiesHandler : IGetOwnedActivitiesHandler
{
    private readonly IHttpContextAccessor httpContext;
    private readonly IActivityData activityData;

    public GetOwnedActivitiesHandler(IHttpContextAccessor httpContext, IActivityData activityData)
    {
        this.httpContext = httpContext;
        this.activityData = activityData;
    }

    public AppResult<GetOwnedActivitiesResult> Execute(GetOwnedActivitiesArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<GetOwnedActivitiesResult>.CreateFailed(ex, "An error occured in GetOwnedActivitiesHandler");
        }
    }

    public async Task<AppResult<GetOwnedActivitiesResult>> ExecuteAsync(GetOwnedActivitiesArgs args)
    {
        try
        {
            // get customer id saved in claims
            var customerId = httpContext.HttpContext?.User.FindFirstValue("UserId");
            if(customerId == null)
            {
                return AppResult<GetOwnedActivitiesResult>.CreateFailed(
                    new ApplicationException("Unable to determine current account login"), "Unable to determine current account login");
            }
            int id = Convert.ToInt32(customerId);

            var result = await activityData.GetAllActivities(new Framework.ApiCommand.ApiData.Activity.Request.GetAllActivities {
                CustomerId         = id,
                IncludeAddress     = args.IncludeActivityAddress,
                IncludeDescription = args.IncludeActivityDescription,
                IncludeSchedules   = args.IncludeAtivitySchedules,
                IncludeImages      = args.IncludeActivityImages,
                IncludeSearchTags  = args.IncludeActivitySearchTags,
                IsActive           = args.IsActive,
                IncludeCustomer    = args.IncludeCustomer,
                IncludeStudents    = args.IncludeStudents,
                IncludeReviews     = true,
                IncludeOteSchedule = true
            });

            if(!result.Succeeded || result.Result == null)
            {
                return AppResult<GetOwnedActivitiesResult>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }

            if(result.Succeeded && !result.Result.IsSuccess)
            {
                return AppResult<GetOwnedActivitiesResult>.CreateFailed(
                    new ApplicationException(result.Result.ErrorInfo?.Message), "An error occured in GetOwnedActivitiesHandler");
            }

            return AppResult<GetOwnedActivitiesResult>.CreateSucceeded(new GetOwnedActivitiesResult {
                Activities = result.Result.Result.Select(a => {
                    return new GetOwnedActivitiesResult.Activity {
                        ActivityLevel = a.ActivityLevel,
                        AdditionalRequirements = a.AdditionalRequirements,
                        Address1 = a.Address1,
                        Address2 = a.Address2,
                        CanAdultsJoin = a.CanAdultsJoin,
                        City = a.City,
                        CityName = a.CityName,
                        Region= a.Region,
                        RegionName= a.RegionName,
                        Barangay = a.Barangay,
                        BarangayName= a.BarangayName,
                        PinnedLocation = a.PinnedLocation,
                        CustomerBringWithThem = a.CustomerBringWithThem,
                        Description = a.Description,
                        District = a.District,
                        ExperienceCategoryId = a.ExperienceCategoryId,
                        ExperienceTypeId = a.ExperienceTypeId,
                        ExperienceCreationType = a.ExperienceCreationType,
                        Id = a.Id,
                        IsPublished = a.IsPublished,
                        MinimumAge = a.MinimumAge,
                        Price = a.Price,
                        Remarks = a.Remarks,
                        SearchTags = a.SearchTags,
                        SkillLevel = a.SkillLevel,
                        SpecificsYouWillProvide = a.SpecificsYouWillProvide,
                        SubCategoryId = a.SubCategoryId,
                        Title = a.Title,
                        Handler = a.Handler,
                        IsNew = a.IsNew,
                        OngoingStudents = a.OngoingStudents,
                        CompletedStudents = a.CompletedStudents,
                        NumberOfReviews = a.NumberOfReviews,
                        Status = a.Status,
                        CreatedOn = a.CreatedOn,
                        ForceDisable = a.ForceDisable,
                        ActivitySchedules = a.Schedules != null ? a.Schedules.Select(s => {
                            return new GetOwnedActivitiesResult.Activity.ActivitySchedule {
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
                                IsActiveSchedule = s.IsActiveSchedule,
                                IsSetSession = s.IsSetSession,
                                SessionName = s.SessionName,
                                HasExpiration = s.HasExpiration,
                                StartDate = s.StartDate,
                            };
                        }) : Enumerable.Empty<GetOwnedActivitiesResult.Activity.ActivitySchedule>(),
                        Images = a.Images != null ? a.Images.OrderBy(i => i.Order).Select(i => {
                            return new GetOwnedActivitiesResult.Activity.ActivityImage {
                                ImageSrc = i.ImageLocation,
                                Name = i.ImageName,
                                Order = i.Order
                            };
                        }) : Enumerable.Empty<GetOwnedActivitiesResult.Activity.ActivityImage>(),
                        Owner = a.Owner != null ? new GetOwnedActivitiesResult.Activity.CustomerOwner {
                            Handler = a.Owner.Handler,
                            Id = a.Owner.Id,
                            IsVerified = a.Owner.IsVerified,
                        } : null,
                        Schedule = a.OteSchedule is not null ? new GetOwnedActivitiesResult.Activity.OteSchedule {
                            From = a.OteSchedule.ScheduleFrom,
                            To = a.OteSchedule.ScheduleTo
                        } : null
                    };
                })
            }, "Successfully get owned activities");
        }
        catch (Exception ex)
        {
            return AppResult<GetOwnedActivitiesResult>.CreateFailed(ex, "An error occured in GetOwnedActivitiesHandler");
        }
    }
}