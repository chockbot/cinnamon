using Cinnamon.Api.Data.Services.Repository.Activity.DTO;
using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Api.Data.Repository.Interfaces;
using Entities = Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Data.Services.Repository.Activity;

public class ActivityRepository : IActivityRepository
{
    private readonly IDataStore dataStore;

    public ActivityRepository(IDataStore dataStore)
    {
        this.dataStore = dataStore;
    }

    public async Task<AppResult<ActivityDTO>> CreateActivityAsync(int experienceTypeId, int customerId, string title, string description, string price, 
        string scheduleIndicator, string remarks, bool isPublished, string address1, string address2, string district, string city, 
        string specificsYouWillProvide, string customerBringWithThem, string? additionalRequirements, string activityLevel, string skillLevel, 
        int minimumAge, bool canAdultsJoin, string? searchtag1, string? searchtag2, string? searchtag3, string? searchtag4, string? searchtag5)
    {
        try
        {
            // check experiencetypeId exist
            var exp = await dataStore.ExperienceType.GetByIdAsync(experienceTypeId);
            if (!exp.Succeeded || exp.Result == null)
            {
                return AppResult<ActivityDTO>.CreateFailed(new ApplicationException("Can't find experience type"), "Can't find experience type");
            }

            // check customer if exist
            var customer = await dataStore.Customer.GetByIdAsync(customerId);
            if (!customer.Succeeded || customer.Result == null)
            {
                return AppResult<ActivityDTO>.CreateFailed(new ApplicationException("Can't find customer id associated to activity"), "Can't find customer id associated to activity");
            }

            // save activity entity
            var ativity = new Entities.Activity
            {
                Title = title,
                Description = description,
                Price = price,
                ScheduleIndicator = scheduleIndicator,
                Remarks = remarks,
                IsPublished = isPublished,
                CreatedBy = customerId
            };
            var createdActitivityRes = await dataStore.Activity.Add(ativity);
            if (!createdActitivityRes.Succeeded || createdActitivityRes.Result == null)
            {
                return AppResult<ActivityDTO>.CreateFailed(createdActitivityRes.Error.Exception, createdActitivityRes.Message);
            }

            // save activity description
            var activityDescription = new Entities.ActivityDescription
            {
                ActivityId = createdActitivityRes.Result.Id,
                ActivityLevel = activityLevel,
                AdditionalRequirements = additionalRequirements,
                CanAdultsJoin = canAdultsJoin,
                CustomerBringWithThem = customerBringWithThem,
                Description = description,
                MinimumAge = minimumAge,
                SkillLevel = skillLevel,
                SpecificsYouWillProvide = specificsYouWillProvide,
            };
            var createdActivityDescription = await dataStore.ActivityDescription.Add(activityDescription);
            if (!createdActivityDescription.Succeeded)
            {
                return AppResult<ActivityDTO>.CreateFailed(createdActivityDescription.Error.Exception, createdActivityDescription.Message);
            }

            // save activity address
            var activityAddress = new Entities.ActivityAddress
            {
                ActivityId = createdActitivityRes.Result.Id,
                Address1 = address1,
                Address2 = address2,
                City = city,
                District = district,
            };
            var createdActivityAddress = await dataStore.ActivityAddress.Add(activityAddress);
            if (!createdActivityAddress.Succeeded)
            {
                return AppResult<ActivityDTO>.CreateFailed(createdActivityAddress.Error.Exception, createdActivityAddress.Message);
            }

            // save activity searchTAg
            var activitySearcTag = new Entities.ActivitySearchTag
            {
                SearchTag1 = searchtag1,
                SearchTag2 = searchtag2,
                SearchTag3 = searchtag3,
                SearchTag4 = searchtag4,
                SearchTag5 = searchtag5,
            };
            var createdActivitySearchTag = await dataStore.ActivitySearchTag.Add(activitySearcTag);
            if (!createdActivitySearchTag.Succeeded)
            {
                return AppResult<ActivityDTO>.CreateFailed(createdActivitySearchTag.Error.Exception, createdActivitySearchTag.Message);
            }

            var createdActivityDTO = new ActivityDTO
            {
                Id = createdActitivityRes.Result.Id,
                ActivityLevel = activityLevel,
                AdditionalRequirements = additionalRequirements,
                Address1 = address1,
                Address2 = address2,
                CanAdultsJoin = canAdultsJoin,
                City = city,
                CustomerBringWithThem = customerBringWithThem,
                Description = description,
                District = district,
                IsPublished = isPublished,
                MinimumAge = minimumAge,
                Price = price,
                Remarks = remarks,
                SkillLevel = skillLevel,
                SpecificsYouWillProvide = specificsYouWillProvide,
                Title = title,
            };

            return AppResult<ActivityDTO>.CreateSucceeded(createdActivityDTO, "Activity successfully created");
        }
        catch (Exception ex)
        {
            return AppResult<ActivityDTO>.CreateFailed(ex, "An error occured when creating activity");
        }
    }

    public async Task<AppResult<IEnumerable<ActivityDTO>>> GetAllAsync(bool? isActive, int? count, int? skip)
    {
        try
        {
            var result = await dataStore.Activity.FindAsync(a => isActive.HasValue ? a.IsPublished == isActive.Value : true, count, skip);
            if (!result.Succeeded || result.Result == null)
            {
                return AppResult<IEnumerable<ActivityDTO>>.CreateFailed(result.Error.Exception, result.Message);
            }

            var activities = result.Result.Select(a =>
            {
                return new ActivityDTO
                {
                    Id = a.Id,
                    SubTitle = a.Subtitle,
                    Title = a.Title,
                    Description = a.Description,
                    Price = a.Price,
                    Remarks = a.Remarks,
                    IsPublished = a.IsPublished,
                };
            });

            return AppResult<IEnumerable<ActivityDTO>>.CreateSucceeded(activities, "Successfully get activities");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<ActivityDTO>>.CreateFailed(ex, "An error occured in getting activities");
        }
    }

    public async Task<AppResult<IEnumerable<ActivityDTO>>> GetAllAsync()
    {
        try
        {
            var result = await dataStore.Activity.GetAllAsync();
            if (!result.Succeeded || result.Result == null)
            {
                return AppResult<IEnumerable<ActivityDTO>>.CreateFailed(result.Error.Exception, result.Message);
            }

            var activities = result.Result.Select(a =>
            {
                return new ActivityDTO
                {
                    Id = a.Id,
                    SubTitle = a.Subtitle,
                    Title = a.Title,
                    Description = a.Description,
                    Price = a.Price,
                    Remarks = a.Remarks,
                    IsPublished = a.IsPublished,
                };
            });

            return AppResult<IEnumerable<ActivityDTO>>.CreateSucceeded(activities, "Successfully get activities");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<ActivityDTO>>.CreateFailed(ex, "An error occured in getting activities");
        }
    }

    public async Task<AppResult<ActivityDTO>> GetByIdAsync(int id)
    {
        try
        {
            var result = await dataStore.Activity.GetByIdAsync(id);
            if (!result.Succeeded || result.Result == null)
            {
                return AppResult<ActivityDTO>.CreateFailed(result.Error.Exception, result.Message);
            }

            var activityDao = new ActivityDTO
            {
                Id = result.Result.Id,
                SubTitle = result.Result.Subtitle,
                Title = result.Result.Title,
                Description = result.Result.Description,
                Price = result.Result.Price,
                Remarks = result.Result.Remarks,
                IsPublished = result.Result.IsPublished,
            };

            return AppResult<ActivityDTO>.CreateSucceeded(activityDao, "Successfully getting activity by id");
        }
        catch (Exception ex)
        {
            return AppResult<ActivityDTO>.CreateFailed(ex, "An error occured when getting activity by id");
        }
    }

    public async Task<AppResult<ActivityDTO>> UpdateActivityAsync(int activityId,int? experienceTypeId, string? title, string? description, string? price, 
        string? scheduleIndicator, string? remarks, bool? isPublished, string? address1, string? address2, string? district, string? city, 
        string? specificsYouWillProvide, string? customerBringWithThem, string? additionalRequirements, string? activityLevel, 
        string? skillLevel, int? minimumAge, bool? canAdultsJoin, string? searchtag1, string? searhtag2, string? searchtag3, string? searchtag4, 
        string? searchtag5)
    {
        try
        {
            // check first activity if exist
            var activityRes = await dataStore.Activity.GetByIdAsync(activityId);
            if(!activityRes.Succeeded || activityRes.Result == null)
            {
                return AppResult<ActivityDTO>.CreateFailed(activityRes.Error.Exception, activityRes.Message);
            }

            var activity = activityRes.Result;

            // update Activity fields
            if (experienceTypeId.HasValue)
            {
                // check experiencetypeId exist
                var exp = await dataStore.ExperienceType.GetByIdAsync(experienceTypeId.Value);
                if (!exp.Succeeded)
                {
                    return AppResult<ActivityDTO>.CreateFailed(exp.Error.Exception, exp.Message);
                }

                activity.ExperienceTypeId = experienceTypeId.Value;
            }

            activity.Title = title ?? activity.Title;
            activity.Description = description ?? activity.Description;
            activity.Price = price ?? activity.Price;
            activity.ScheduleIndicator = scheduleIndicator ?? activity.ScheduleIndicator;
            activity.Remarks = remarks ?? activity.Remarks;
            activity.IsPublished = isPublished ?? activity.IsPublished;

            var updatedActivity = await dataStore.Activity.Update(activity);
            if (!updatedActivity.Succeeded)
            {
                return AppResult<ActivityDTO>.CreateFailed(updatedActivity.Error.Exception, updatedActivity.Message);
            }

            // updateActivityAddress
            var activityAddressRes = await dataStore.ActivityAddress.FindFirstAsync(a => a.ActivityId == activity.Id);
            if(!activityAddressRes.Succeeded || activityAddressRes.Result == null)
            {
                return AppResult<ActivityDTO>.CreateFailed(updatedActivity.Error.Exception, updatedActivity.Message);
            }
            var activityAddress = activityAddressRes.Result;

            activityAddress.Address1 = address1 ?? activityAddress.Address1;
            activityAddress.Address2 = address2 ?? activityAddress.Address2;
            activityAddress.District = district ?? activityAddress.District;
            activityAddress.City = city ?? activityAddress.City;

            var updatedActivityAddress = await dataStore.ActivityAddress.Update(activityAddress);
            if (!updatedActivityAddress.Succeeded)
            {
                return AppResult<ActivityDTO>.CreateFailed(updatedActivityAddress.Error.Exception, updatedActivityAddress.Message);
            }

            // update activity description
            var activityDescriptionRes = await dataStore.ActivityDescription.FindFirstAsync(a => a.ActivityId == activity.Id);
            if (!activityDescriptionRes.Succeeded || activityDescriptionRes.Result == null)
            {
                return AppResult<ActivityDTO>.CreateFailed(activityDescriptionRes.Error.Exception, activityDescriptionRes.Message);
            }
            var activityDescription = activityDescriptionRes.Result;

            activityDescription.MinimumAge = minimumAge ?? activityDescription.MinimumAge;
            activityDescription.Description = description ?? activityDescription.Description;
            activityDescription.ActivityLevel = activityLevel ?? activityDescription.ActivityLevel;
            activityDescription.SkillLevel = skillLevel ?? activityDescription.SkillLevel;
            activityDescription.SpecificsYouWillProvide = specificsYouWillProvide ?? activityDescription.SpecificsYouWillProvide;
            activityDescription.AdditionalRequirements = additionalRequirements ?? activityDescription.AdditionalRequirements;
            activityDescription.CanAdultsJoin = canAdultsJoin ?? activityDescription.CanAdultsJoin;
            activityDescription.CustomerBringWithThem = customerBringWithThem ?? activityDescription.CustomerBringWithThem;

            var updatedActivityDescription = await dataStore.ActivityDescription.Update(activityDescription);
            if (!updatedActivityDescription.Succeeded)
            {
                return AppResult<ActivityDTO>.CreateFailed(updatedActivityDescription.Error.Exception, updatedActivityDescription.Message);
            }

            // update activity search tags
            var activitySearchTagRes = await dataStore.SearchTags.FindFirstAsync(s => s.ActivityId == activity.Id);
            if(!activitySearchTagRes.Succeeded || activitySearchTagRes.Result == null)
            {
                return AppResult<ActivityDTO>.CreateFailed(activitySearchTagRes.Error.Exception, activitySearchTagRes.Message);
            }
            var activitySearchTag = activitySearchTagRes.Result;

            activitySearchTag.SearchTag1 = searchtag1 ?? activitySearchTag.SearchTag1;
            activitySearchTag.SearchTag2 = searhtag2 ?? activitySearchTag.SearchTag2;
            activitySearchTag.SearchTag3 = searchtag3 ?? activitySearchTag.SearchTag3;
            activitySearchTag.SearchTag4 = searchtag4 ?? activitySearchTag.SearchTag4;
            activitySearchTag.SearchTag5 = searchtag5 ?? activitySearchTag.SearchTag5;

            var updatedSearchTag = await dataStore.SearchTags.Update(activitySearchTag);
            if (!updatedSearchTag.Succeeded)
            {
                return AppResult<ActivityDTO>.CreateFailed(updatedSearchTag.Error.Exception, updatedSearchTag.Message);
            }

            return AppResult<ActivityDTO>.CreateSucceeded(new ActivityDTO
            {
                Id = activity.Id,
                ActivityLevel = activityDescription.ActivityLevel,
                AdditionalRequirements = activityDescription.AdditionalRequirements,
                Address1 = activityAddress.Address1,
                Address2 = activityAddress.Address2,
                CanAdultsJoin = activityDescription.CanAdultsJoin,
                City = activityAddress.City,
                CustomerBringWithThem = activityDescription.CustomerBringWithThem,
                Description = activityDescription.Description,
                District = activityAddress.District,
                IsPublished = activity.IsPublished,
                MinimumAge = activityDescription.MinimumAge,
                Price = activity.Price,
                Remarks = activity.Remarks,
                SkillLevel = activityDescription.SkillLevel,
                SpecificsYouWillProvide = activityDescription.SpecificsYouWillProvide,
                Title = activity.Title,
                SubTitle = activity.Subtitle
            }, "Successfully updated activity details");

        }
        catch (Exception ex)
        {
            return AppResult<ActivityDTO>.CreateFailed(ex, "An error occured when updating activity");
        }
    }
}