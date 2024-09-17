using System.Security.Claims;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Interactors;
using Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.ActivityService;

public class GetEnrolledActivitiesHandler : IGetEnrolledActivitiesHandler
{
    private readonly IActivityData activityData;
    private readonly IOngoingActivitiesData ongoingActivitiesData;
    private readonly IHttpContextAccessor httpContextAccessor;

    public GetEnrolledActivitiesHandler(IActivityData activityData, IHttpContextAccessor httpContextAccessor,
        IOngoingActivitiesData ongoingActivitiesData)
    {
        this.activityData = activityData;
        this.httpContextAccessor = httpContextAccessor;
        this.ongoingActivitiesData = ongoingActivitiesData;
    }

    public AppResult<GetEnrolledActivitiesResult> Execute(GetEnrolledActivitiesArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<GetEnrolledActivitiesResult>.CreateFailed(ex, "An error occured in GetEnrolledActivitiesHandler");
        }
    }

    public async Task<AppResult<GetEnrolledActivitiesResult>> ExecuteAsync(GetEnrolledActivitiesArgs args)
    {
        try
        {
            // get customer id saved in claims
            var customerId = httpContextAccessor.HttpContext?.User.FindFirstValue("UserId");
            if(customerId == null)
            {
                return AppResult<GetEnrolledActivitiesResult>.CreateFailed(
                    new ApplicationException("Unable to determine current account login"), "Unable to determine current account login");
            }
            int id = Convert.ToInt32(customerId);

            var ongoingActivityRes = await ongoingActivitiesData.GetAllOngoingActivities(new Framework.ApiCommand.ApiData.OngoingActivity.Request.GetAllOngoingActivityArgs {
                CustomerId = id
            });
            if(!ongoingActivityRes.Succeeded || ongoingActivityRes.Result == null || !ongoingActivityRes.Result.IsSuccess)
            {
                return AppResult<GetEnrolledActivitiesResult>.CreateFailed(
                    new ApplicationException(ongoingActivityRes.Result?.ErrorInfo?.Message), ongoingActivityRes.Message);
            }

            if(ongoingActivityRes.Result.Result.Count() == 0)
            {
                return AppResult<GetEnrolledActivitiesResult>.CreateSucceeded(new GetEnrolledActivitiesResult {
                    Activities = Enumerable.Empty<GetEnrolledActivitiesResult.Activity>()
                }, "Successfully get enrolled activities");
            }

            var stringIds = string.Empty;
            foreach(var item in ongoingActivityRes.Result.Result)
            {
                stringIds += $"{item.ActivityId},";
            }

            var activitiesRes = await activityData.GetAllActivities(new Framework.ApiCommand.ApiData.Activity.Request.GetAllActivities {
                Ids = stringIds,
                IncludeAddress = args.IncludeActivityAddress,
                IncludeDescription = args.IncludeActivityDescription,
                IncludeImages = args.IncludeActivityImages,
                IncludeSchedules = args.IncludeAtivitySchedules,
                IsActive = args.IsActive,
                IncludeSearchTags = args.IncludeActivitySearchTags,
                IncludeCustomer = args.IncludeCustomer,
                IncludeStudents = args.IncludeStudents,
                IncludeOteSchedule = true
            });

            if(!activitiesRes.Succeeded || activitiesRes.Result == null || !activitiesRes.Result.IsSuccess)
            {
                return AppResult<GetEnrolledActivitiesResult>.CreateFailed(
                    new ApplicationException(activitiesRes.Result?.ErrorInfo?.Message), activitiesRes.Message);
            }

            return AppResult<GetEnrolledActivitiesResult>.CreateSucceeded(new GetEnrolledActivitiesResult {
                Activities = activitiesRes.Result.Result.Select(e => {
                    return new GetEnrolledActivitiesResult.Activity {
                        Id = e.Id,
                        ExperienceCategoryId = e.ExperienceCategoryId,
                        ExperienceTypeId = e.ExperienceTypeId,
                        SubCategoryId = e.SubCategoryId,
                        Title =e.Title,  
                        Description=e.Description,  
                        SpecificsYouWillProvide=e.SpecificsYouWillProvide,
                        CustomerBringWithThem = e.CustomerBringWithThem,
                        AdditionalRequirements = e.AdditionalRequirements,
                        ActivityLevel = e.ActivityLevel,
                        SkillLevel = e.SkillLevel,
                        MinimumAge = e.MinimumAge,
                        CanAdultsJoin = e.CanAdultsJoin,
                        Price= e.Price,
                        Remarks = e.Remarks,
                        Address1 = e.Address1,
                        Address2 = e.Address2,
                        District = e.District,
                        City = e.City,
                        Subdivision = e.Subdivision,
                        Region = e.Region,
                        Barangay= e.Barangay,
                        PostalCode = e.PostalCode,
                        PinnedLocation = e.PinnedLocation,
                        SearchTags = e.SearchTags != null ? e.SearchTags.ToList() : Enumerable.Empty<string>().ToList(),
                        IsPublished = e.IsPublished,
                        CreatedBy = e.CreatedBy,
                        OngoingStudents = e.OngoingStudents,
                        CompletedStudents = e.CompletedStudents,
                        ActivitySchedules = e.Schedules != null ? e.Schedules.Select(s => {
                            return new GetEnrolledActivitiesResult.ActivitySchedule
                            {
                                DateTime = s.DateTime,
                                Name = s.Name,
                                PerUnit1 = s.PerUnit1,
                                PerUnit2 = s.PerUnit2,
                                Price = s.Price,
                                PriceUnit1 = s.PriceUnit1,
                                PriceUnit2 = s.PriceUnit2,
                                UnitPrice = s.UnitPrice,
                                Id = s.Id,
                                Order = s.Order,
                                IsActiveSchedule = s.IsActiveSchedule,
                                IsSetSession = s.IsSetSession,
                                SessionName = s.SessionName,
                                HasExpiration = s.HasExpiration,
                                StartDate = s.StartDate
                            };
                        }) : Enumerable.Empty<GetEnrolledActivitiesResult.ActivitySchedule>(),
                        Images = e.Images != null ? e.Images.OrderBy(i => i.Order).Select(i => {
                            return new GetEnrolledActivitiesResult.ActivityImage
                            {
                                ImageSrc = i.ImageLocation,
                                Name = i.ImageName,
                                Order = i.Order
                            };
                        }) : Enumerable.Empty<GetEnrolledActivitiesResult.ActivityImage>(),
                        Owner = e.Owner != null ? new GetEnrolledActivitiesResult.CustomerOwner {
                            Handler = e.Owner.Handler,
                            Id = e.Owner.Id,
                            FirstName = e.Owner.FirstName,
                            LastName = e.Owner.LastName,
                            Email = e.Owner.Email,
                            ImageSrc = e.Owner.ProfileImg ?? string.Empty,
                            IsVerified = e.Owner.IsVerified,
                            IsOG = e.Owner.IsOG,
                            IsOfficial = e.Owner.IsOfficial,
                        } : null,
                        Schedule = e.OteSchedule is not null ? new GetEnrolledActivitiesResult.OteSchedule {
                        From = e.OteSchedule.ScheduleFrom,
                        To = e.OteSchedule.ScheduleTo
                        } : null,
                    };
                })
            }, "Successfully get enrolled activities");
        }
        catch (Exception ex)
        {
            return AppResult<GetEnrolledActivitiesResult>.CreateFailed(ex, "An error occured in GetEnrolledActivitiesHandler");
        }
    }
}