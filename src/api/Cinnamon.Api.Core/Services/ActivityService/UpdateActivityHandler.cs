using System.Security.Claims;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Interactors;
using Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.ActivityService;

public class UpdateActivityHandler : IUpdateActivityHandler
{
    private readonly IHttpContextAccessor httpContext;
    private readonly IActivityData activityData;
    private readonly IExperienceCategoryData categoryData;
    private readonly ISubCategoryData subCategoryData;
    private readonly IExperienceTypeData experienceTypeData;
    private readonly IScheduleData scheduleData;

    public UpdateActivityHandler(IActivityData activityData, IExperienceCategoryData categoryData,
        ISubCategoryData subCategoryData, IExperienceTypeData experienceTypeData, IScheduleData scheduleData,
        IHttpContextAccessor httpContext)
    {
        this.activityData = activityData;
        this.categoryData = categoryData;
        this.subCategoryData = subCategoryData;
        this.experienceTypeData = experienceTypeData;
        this.scheduleData = scheduleData;
        this.httpContext = httpContext;
    }

    public AppResult<UpdateActivityResult> Execute(UpdateActivityArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<UpdateActivityResult>.CreateFailed(ex, "An error occured in UpdateActivityHandler");
        }
    }

    public async Task<AppResult<UpdateActivityResult>> ExecuteAsync(UpdateActivityArgs args)
    {
        try
        {
            // get customer id saved in claims
            var customerId = httpContext.HttpContext?.User.FindFirstValue("UserId");
            if(customerId == null)
            {
                return AppResult<UpdateActivityResult>.CreateFailed(
                    new ApplicationException("Unable to determine current account login"), "Unable to determine current account login");
            }
            int id = Convert.ToInt32(customerId);

            // check activity if existed
            var activity = await activityData.GetActivityById(args.ActivityId, 
                            new Framework.ApiCommand.ApiData.Activity.Request.GetActivityArgs {IncludeSchedules = true, CustomerId = id });
            if(!activity.Succeeded || activity.Result == null)
            {
                return AppResult<UpdateActivityResult>.CreateFailed(new ApplicationException(activity.Message), activity.Message);
            }
            if(activity.Succeeded && !activity.Result.IsSuccess)
            {
                return AppResult<UpdateActivityResult>.CreateFailed(new ApplicationException("Can't find activity to update"), "Can't find activity to update");
            }

            // check category id
            if(args.ExperienceCategoryId.HasValue)
            {
                var category = await categoryData.GetCategotyById(args.ExperienceCategoryId.Value);
                if(!category.Succeeded || category.Result == null)
                {
                    return AppResult<UpdateActivityResult>.CreateFailed(new ApplicationException(category.Message), category.Message);
                }
                if(category.Succeeded && !category.Result.IsSuccess)
                {
                    return AppResult<UpdateActivityResult>.CreateFailed(new ApplicationException("Can't find category id to update"), "Can't find category id to update");
                }
            }

            // check sub category Id
            if(args.SubCategoryId.HasValue)
            {
                var subCategory = await subCategoryData.GetSubCategoryById(args.SubCategoryId.Value);
                if(!subCategory.Succeeded || subCategory.Result == null)
                {
                    return AppResult<UpdateActivityResult>.CreateFailed(new ApplicationException(subCategory.Message), subCategory.Message);
                }
                if(subCategory.Succeeded && !subCategory.Result.IsSuccess)
                {
                    return AppResult<UpdateActivityResult>.CreateFailed(new ApplicationException("Can't find sub category id to update"), "Can't find sub category id to update");
                }
            }

            // check experience type id
            if(args.ExperienceTypeId.HasValue)
            {
                var experienceType = await experienceTypeData.GetExperienceTypeById(args.ExperienceTypeId.Value);
                if(!experienceType.Succeeded || experienceType.Result == null)
                {
                    return AppResult<UpdateActivityResult>.CreateFailed(new ApplicationException(experienceType.Message), experienceType.Message);
                }
                if(experienceType.Succeeded && !experienceType.Result.IsSuccess)
                {
                    return AppResult<UpdateActivityResult>.CreateFailed(new ApplicationException("Can't find experience type id to update"), "Can't find experience type id to update");
                }
            }

            // update activity details
            var entity = new Framework.ApiCommand.ApiData.Activity.Request.UpdateActivity {
                ActivityId = args.ActivityId,
                ActivityLevel = args.ActivityLevel,
                AdditionalRequirements = args.AdditionalRequirements,
                Address1 = args.Address1,
                Address2 = args.Address2,
                CanAdultsJoin = args.CanAdultsJoin,
                City = args.City,
                CustomerBringWithThem = args.CustomerBringWithThem,
                Description = args.Description,
                District = args.District,
                ExperienceCategoryId = args.ExperienceCategoryId,
                ExperienceTypeId = args.ExperienceTypeId,
                IsPublished = args.IsPublished,
                MinimumAge = args.MinimumAge,
                Price = args.Price,
                Remarks = args.Remarks,
                ScheduleIndicator = args.ScheduleIndicator,
                SkillLevel = args.SkillLevel,
                SpecificsYouWillProvide = args.SpecificsYouWillProvide,
                SubCategoryId = args.SubCategoryId,
                Title = args.Title
            };

            if(args.SearchTags != null)
            {
                entity.Searchtag1 = args.SearchTags.Count() >= 1 ? args.SearchTags.ElementAt(0) : null;
                entity.Searhtag2 = args.SearchTags.Count() >= 2 ? args.SearchTags.ElementAt(1) : null;
                entity.Searhtag3 = args.SearchTags.Count() >= 3 ? args.SearchTags.ElementAt(2) : null;
                entity.Searchtag4 = args.SearchTags.Count() >= 4 ? args.SearchTags.ElementAt(3) : null;
                entity.Searchtag5 = args.SearchTags.Count() >=5 ? args.SearchTags.ElementAt(4) : null;
            }

            var updatedActivity = await activityData.UpdateActivity(entity);
            if(!updatedActivity.Succeeded || updatedActivity.Result == null)
            {
                return AppResult<UpdateActivityResult>.CreateFailed(new ApplicationException(updatedActivity.Message), updatedActivity.Message);
            }
            if(updatedActivity.Succeeded && !updatedActivity.Result.IsSuccess)
            {
                return AppResult<UpdateActivityResult>.CreateFailed(
                    new ApplicationException(updatedActivity.Result.ErrorInfo?.Message), "An error occured in UpdateActivityHandler");
            }
            var updated = updatedActivity.Result.Result;

            // update activity schedules
            if(args.ActivitySchedules != null)
            {
                var associatedIds = activity.Result.Result.Schedules.Select(s => s.Id);
                var newSchedules = args.ActivitySchedules.Where(s => s.Id == 0);
                var updatedSchedules = args.ActivitySchedules.Where(s => associatedIds.Contains(s.Id));

                if(newSchedules.Count() > 0)
                {
                    // create new schedules
                    var createdSchedules = await scheduleData.CreateManySchedules(new Framework.ApiCommand.ApiData.Schedule.Request.CreateManySchedulesArgs {
                        ActivityId = args.ActivityId,
                        Schedules = newSchedules.Select(s => {
                            return new Framework.ApiCommand.ApiData.Schedule.Request.CreateManySchedulesArgs.Schedule {
                                DateTime = s.DateTime ?? string.Empty,
                                Name = s.Name ?? string.Empty,
                                PerUnit1 = s.PerUnit1 ?? 1,
                                PerUnit2 = s.PerUnit2 ?? 1,
                                Price = s.Price ?? 1,
                                PriceUnit1 = s.PriceUnit1 ?? string.Empty,
                                PriceUnit2 = s.PriceUnit2 ?? string.Empty,
                                UnitPrice = s.UnitPrice ?? string.Empty
                            };
                        })
                    });
                    if(!createdSchedules.Succeeded || createdSchedules.Result == null)
                    {
                        return AppResult<UpdateActivityResult>.CreateFailed(new ApplicationException(createdSchedules.Message), createdSchedules.Message);
                    }
                    if(createdSchedules.Succeeded && !createdSchedules.Result.IsSuccess)
                    {
                        return AppResult<UpdateActivityResult>.CreateFailed(
                            new ApplicationException(createdSchedules.Result.ErrorInfo?.Message), "An error occured in UpdateActivityHandler");
                    }
                }

                if(updatedSchedules.Count() > 0)
                {
                    var updatedSchedulesRes = await scheduleData.UpdateManySchedules(new Framework.ApiCommand.ApiData.Schedule.Request.UpdateManySchedulesArgs {
                        Schedules = updatedSchedules.Select(s => {
                            return new Framework.ApiCommand.ApiData.Schedule.Request.UpdateManySchedulesArgs.UpdateSchedule {
                                ActivityId = args.ActivityId,
                                DateTime = s.DateTime ?? string.Empty,
                                Id = s.Id,
                                Name = s.Name ?? string.Empty,
                                PerUnit1 = s.PerUnit1 ?? 1,
                                PerUnit2 = s.PerUnit2 ?? 1,
                                Price = s.Price ?? 1,
                                PriceUnit1 = s.PriceUnit1 ?? string.Empty,
                                PriceUnit2 = s.PriceUnit2 ?? string.Empty,
                                UnitPrice = s.UnitPrice ?? string.Empty
                            };
                        })
                    });
                    if(!updatedSchedulesRes.Succeeded || updatedSchedulesRes.Result == null)
                    {
                        return AppResult<UpdateActivityResult>.CreateFailed(new ApplicationException(updatedSchedulesRes.Message), updatedSchedulesRes.Message);
                    }
                    if(updatedSchedulesRes.Succeeded && !updatedSchedulesRes.Result.IsSuccess)
                    {
                        return AppResult<UpdateActivityResult>.CreateFailed(
                            new ApplicationException(updatedSchedulesRes.Result.ErrorInfo?.Message), "An error occured in UpdateActivityHandler");
                    }
                }
            }
            
            // delete schedules
            if(args.DeletedScheduleIds != null && args.DeletedScheduleIds.Count() > 0)
            {
                var associatedIds = activity.Result.Result.Schedules.Select(s => s.Id);
                var idsMustDelete = args.DeletedScheduleIds.Where(i => i > 0 && associatedIds.Contains(i));

                if(idsMustDelete.Count() > 0)
                {
                    var deletedSchedules = await scheduleData.DeleteManySchedules(new Framework.ApiCommand.ApiData.Schedule.Request.DeleteManySchedulesArgs {
                        ScheduleIds = idsMustDelete
                    });
                    if(!deletedSchedules.Succeeded || deletedSchedules.Result == null)
                    {
                        return AppResult<UpdateActivityResult>.CreateFailed(new ApplicationException(deletedSchedules.Message), deletedSchedules.Message);
                    }
                    if(deletedSchedules.Succeeded && !deletedSchedules.Result.IsSuccess)
                    {
                        return AppResult<UpdateActivityResult>.CreateFailed(
                            new ApplicationException(deletedSchedules.Result.ErrorInfo?.Message), "An error occured in UpdateActivityHandler");
                    }
                }
            }

            return AppResult<UpdateActivityResult>.CreateSucceeded(new UpdateActivityResult {
                ActivityId = updated.Id,
                ActivityLevel = updated.ActivityLevel,
                AdditionalRequirements = updated.AdditionalRequirements,
                Address1 = updated.Address1,
                Address2 = updated.Address2,
                CanAdultsJoin = updated.CanAdultsJoin,
                City = updated.City,
                CustomerBringWithThem = updated.CustomerBringWithThem,
                Description = updated.Description,
                District = updated.District,
                ExperienceCategoryId = updated.ExperienceCategoryId,
                ExperienceTypeId = updated.ExperienceTypeId,
                IsPublished = updated.IsPublished,
                MinimumAge = updated.MinimumAge,
                Price = updated.Price,
                Remarks = updated.Remarks,
                SearchTags = updated.SearchTags,
                SkillLevel = updated.SkillLevel,
                SpecificsYouWillProvide = updated.SpecificsYouWillProvide,
                SubCategoryId = updated.SubCategoryId,
                Title = updated.Title
            }, "Successfully update activity details");
        }
        catch (Exception ex)
        {
            return AppResult<UpdateActivityResult>.CreateFailed(ex, "An error occured in UpdateActivityHandler");
        }
    }
}