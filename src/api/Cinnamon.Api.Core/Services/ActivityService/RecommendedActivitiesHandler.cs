using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Interactors;
using Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.ActivityService;

public class RecommendedActivitiesHandler : IRecommendedActivitiesHandler
{
    private readonly IGetEnrolledActivitiesHandler getEnrolledActivitiesHandler;
    private readonly IActivityData activityData;

    public RecommendedActivitiesHandler(IGetEnrolledActivitiesHandler getEnrolledActivitiesHandler, IActivityData activityData)
    {
        this.getEnrolledActivitiesHandler = getEnrolledActivitiesHandler;
        this.activityData = activityData;
    }

    public AppResult<RecommendedActivityResult> Execute(RecommendedActivityArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<RecommendedActivityResult>.CreateFailed(ex, "An error occured in RecommendedActivitiesHandler");
        }
    }

    public async Task<AppResult<RecommendedActivityResult>> ExecuteAsync(RecommendedActivityArgs args)
    {
        try
        {
            var enrolledActivities = await getEnrolledActivitiesHandler.ExecuteAsync(new GetEnrolledActivitiesArgs {
                IncludeActivityImages = true,
            });
            if(!enrolledActivities.Succeeded || enrolledActivities.Result == null)
            {
                return AppResult<RecommendedActivityResult>.CreateFailed(new ApplicationException(enrolledActivities.Message), enrolledActivities.Message);
            }

            if(enrolledActivities.Result.Activities.Count() == 0)
            {
                return AppResult<RecommendedActivityResult>.CreateSucceeded(new RecommendedActivityResult {
                    RecommendedActivities = Enumerable.Empty<RecommendedActivityResult.Activity>(),
                }, "Successfully get recommended activities");
            }

            // get random enrolled activities
            int randomIndex = (new Random()).Next(0, enrolledActivities.Result.Activities.Count());
            var randomActivity = enrolledActivities.Result.Activities.ElementAt(randomIndex);
            if(randomActivity == null)
            {
                return AppResult<RecommendedActivityResult>.CreateFailed(new ApplicationException("An error occured in RecommendedActivitiesHandler"), "An error occured in RecommendedActivitiesHandler");
            }

            var recommendedRes = await activityData.RecommendedActivities(randomActivity.Id, args.Count);
            if(!recommendedRes.Succeeded || recommendedRes.Result == null || !recommendedRes.Result.IsSuccess)
            {
                return AppResult<RecommendedActivityResult>.CreateFailed(new ApplicationException(recommendedRes.Result?.ErrorInfo?.Message), recommendedRes.Message);
            }

            return AppResult<RecommendedActivityResult>.CreateSucceeded(new RecommendedActivityResult
            {
                RecommendedActivities = recommendedRes.Result.Result.Select(a =>
                {
                    return new RecommendedActivityResult.Activity
                    {
                        Description      = a.Description,
                        Handler          = a.Handler,
                        Id               = a.Id,
                        Address1         = a.Address1,
                        Address2         = a.Address2,
                        City             = a.City,
                        CityName         = a.CityName,
                        Subdivision      = a.Subdivision,
                        Region           = a.Region,
                        RegionName       = a.RegionName,
                        Barangay         = a.Barangay,
                        ExperienceTypeId = a.ExperienceTypeId,
                        Images           = a.Images.Select(i =>
                        {
                            return new RecommendedActivityResult.Activity.ActivityImage
                            {
                                Id = i.Id,
                                ImageSrc = i.ImageLocation,
                                Name = i.ImageName,
                                Order = i.Order
                            };
                        }),
                        Price = a.Price,
                        Title = a.Title,
                        ActivitySchedules = a.Schedules != null ? a.Schedules.Select(s => {
                            return new RecommendedActivityResult.Activity.ActivitySchedule
                            {
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
                        }) : Enumerable.Empty<RecommendedActivityResult.Activity.ActivitySchedule>(),
                        Schedule = a.OteSchedule is not null ? new RecommendedActivityResult.Activity.OteSchedule {
                        From = a.OteSchedule.ScheduleFrom,
                        To = a.OteSchedule.ScheduleTo
                        } : null,
                    };
                })
            }, "Successfully get recommended activities");

        }
        catch (Exception ex)
        {
            return AppResult<RecommendedActivityResult>.CreateFailed(ex, "An error occured in RecommendedActivitiesHandler");
        }
    }
}