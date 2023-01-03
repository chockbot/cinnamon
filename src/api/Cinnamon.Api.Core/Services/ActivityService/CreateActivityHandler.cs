using System.Security.Claims;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Interactors;
using Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.ActivityService;

public class CreateActivityHandler : ICreateActivityHandler
{
    private readonly IHttpContextAccessor httpContext;
    private readonly IActivityData activityData;
    private readonly IScheduleData scheduleData;

    public CreateActivityHandler(IHttpContextAccessor httpContext, IActivityData activityData, IScheduleData scheduleData)
    {
        this.httpContext = httpContext;
        this.activityData = activityData;
        this.scheduleData = scheduleData;
    }

    public AppResult<CreateActivityResult> Execute(CreateActivityArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<CreateActivityResult>.CreateFailed(ex, "An error occured in CreateActivityHandler");
        }
    }

    public async Task<AppResult<CreateActivityResult>> ExecuteAsync(CreateActivityArgs args)
    {
        try
        {
            // get customer id saved in claims
            var customerId = httpContext.HttpContext?.User.FindFirstValue("UserId");
            if(customerId == null)
            {
                return AppResult<CreateActivityResult>.CreateFailed(
                    new ApplicationException("Unable to determine current account login"), "Unable to determine current account login");
            }
            int id = Convert.ToInt32(customerId);

            var activityRes = await activityData.CreateActivity(new Framework.ApiCommand.ApiData.Activity.Request.CreateActivityArgs {
                ActivityLevel = args.ActivityLevel,
                AdditionalRequirements = args.AdditionalRequirements,
                Address1 = args.Address1,
                Address2 = args.Address2,
                CanAdultsJoin = args.CanAdultsJoin,
                City = args.City,
                CustomerBringWithThem = args.CustomerBringWithThem,
                CustomerId = id,
                Description = args.Description,
                District = args.District,
                ExperienceCategoryId = args.ExperienceCategoryId,
                ExperienceTypeId = args.ExperienceTypeId,
                IsPublished = args.IsPublished,
                MinimumAge = args.MinimumAge,
                Price = args.Price,
                Remarks = args.Remarks,
                ScheduleIndicator = args.ScheduleIndicator,
                Searchtag1 = args.SearchTags.Count() >= 1 ? args.SearchTags.ElementAt(0) : null,
                Searhtag2 = args.SearchTags.Count() >= 2 ? args.SearchTags.ElementAt(1) : null,
                Searhtag3 = args.SearchTags.Count() >= 3 ? args.SearchTags.ElementAt(2) : null,
                Searchtag4 = args.SearchTags.Count() >= 4 ? args.SearchTags.ElementAt(3) : null,
                Searchtag5 = args.SearchTags.Count() >= 5 ? args.SearchTags.ElementAt(4) : null,
                SkillLevel = args.SkillLevel,
                SpecificsYouWillProvide = args.SpecificsYouWillProvide,
                SubCategoryId = args.SubCategoryId,
                Title = args.Title
            });

            if(!activityRes.Succeeded || activityRes.Result == null)
            {
                return AppResult<CreateActivityResult>.CreateFailed(new ApplicationException(activityRes.Message), activityRes.Message);
            }

            if(activityRes.Succeeded && !activityRes.Result.IsSuccess)
            {
                return AppResult<CreateActivityResult>.CreateFailed(new ApplicationException(activityRes.Result.ErrorInfo?.Message), "An error occured in CreateActivityHandler");
            }
            var activity = activityRes.Result.Result;

            // create activity schedules
            var createdSchedules = await scheduleData.CreateManySchedules(new Framework.ApiCommand.ApiData.Schedule.Request.CreateManySchedulesArgs {
                ActivityId = activity.Id,
                Schedules = args.ActivitySchedules.Select(s => {
                    return new Framework.ApiCommand.ApiData.Schedule.Request.CreateManySchedulesArgs.Schedule {
                        DateTime = s.DateTime,
                        Name = s.Name,
                        PerUnit1 = s.PerUnit1,
                        PerUnit2 = s.PerUnit2,
                        Price = s.Price,
                        PriceUnit1 = s.PriceUnit1,
                        PriceUnit2 = s.PriceUnit2,
                        UnitPrice = s.UnitPrice
                    };
                })
            });

            if(!createdSchedules.Succeeded || createdSchedules.Result == null)
            {
                return AppResult<CreateActivityResult>.CreateFailed(new ApplicationException(createdSchedules.Message), createdSchedules.Message);
            }

            if(createdSchedules.Succeeded && !createdSchedules.Result.IsSuccess)
            {
                return AppResult<CreateActivityResult>.CreateFailed(new ApplicationException(createdSchedules.Result.ErrorInfo?.Message), "An error occured in CreateActivityHandler");
            }

            return AppResult<CreateActivityResult>.CreateSucceeded(new CreateActivityResult {
                ActivityId = activity.Id,
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
                ExperienceTypeId = args.ExperienceTypeId,
                IsPublished = activity.IsPublished,
                MinimumAge = activity.MinimumAge,
                Price = activity.Price,
                Remarks = activity.Remarks,
                SearchTags = activity.SearchTags,
                SkillLevel = activity.SkillLevel,
                SpecificsYouWillProvide = activity.SpecificsYouWillProvide,
                SubCategoryId = activity.SubCategoryId,
                Title = activity.Title

            }, "Successfully creating activity");
        }
        catch (Exception ex)
        {
            return AppResult<CreateActivityResult>.CreateFailed(ex, "An error occured in CreateActivityHandler");
        }
    }
}