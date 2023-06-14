using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Api.Data.Repository.Interfaces;
using Entities = Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Framework.Common;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.Activity;
using System.Linq.Expressions;
using System.Globalization;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;
using Cinnamon.Api.Data.Repository.Entities;
using static Cinnamon.Framework.Enums.Enums;
using System;
using Cinnamon.Framework.Enums;

namespace Cinnamon.Api.Data.Services.Repository.Activity;

public class ActivityRepository : IActivityRepository
{
    private readonly IDataStore dataStore;

    public ActivityRepository(IDataStore dataStore)
    {
        this.dataStore = dataStore;
    }

    public async Task<AppResult<ActivityDTO>> CreateActivityAsync(int experienceTypeId, int customerId, string title, string description, string price, 
        string scheduleIndicator, string remarks, bool isPublished, string address1, string address2, string district, string city, string subdivision, string region, string barangay, string postalcode,
        string specificsYouWillProvide, string customerBringWithThem, string? additionalRequirements, string activityLevel, string skillLevel, 
        int minimumAge, bool canAdultsJoin, string? searchtag1, string? searchtag2, string? searchtag3, string? searchtag4, string? searchtag5,
        int experienceCategoryId, int subCategoryId, string handler, bool IsSetSession, string SessionName, string pinnedLocation, Enums.ActivityStatus status)
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

            // check experience category id
            var category = await dataStore.ExperienceCategory.GetByIdAsync(experienceCategoryId);
            if(!category.Succeeded || category.Result == null)
            {
                return AppResult<ActivityDTO>.CreateFailed(new ApplicationException("Can't find experience category id associated to activity"), "Can't find experience category id associated to activity");
            }

            // check sub category
            var subcategory = await dataStore.SubCategory.GetByIdAsync(subCategoryId);
            if(!subcategory.Succeeded || subcategory.Result == null)
            {
                return AppResult<ActivityDTO>.CreateFailed(new ApplicationException("Can't find sub category id associated to activity"), "Can't find sub category id associated to activity");
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
                CreatedBy = customerId,
                ExperienceCategoryId = experienceCategoryId,
                ExperienceTypeId = experienceTypeId,
                SubCategoryId = subCategoryId,
                Handler = handler,
                IsNew = true,
                IsSetSession = IsSetSession,
                SessionName = SessionName,
                Guid = Guid.NewGuid().ToString(),
                Status = (int)status
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

            var regionResult = new AppResult<Region>();
            var cityResult = new AppResult<City>();
            var barangayResult = new AppResult<Barangay>();

            if (experienceTypeId == 1)
            {
                if (status == ActivityStatus.Submitted)
                {
                    regionResult = await dataStore.Region.FindFirstAsync(r => r.Code == region);
                    if (!regionResult.Succeeded || regionResult == null)
                    {
                        return AppResult<ActivityDTO>.CreateFailed(new ApplicationException("Can't find region"), "Can't find region");
                    }

                    cityResult = await dataStore.City.FindFirstAsync(r => r.Code == city);
                    if (!cityResult.Succeeded || cityResult == null)
                    {
                        return AppResult<ActivityDTO>.CreateFailed(new ApplicationException("Can't find city"), "Can't find city");
                    }

                    barangayResult = await dataStore.Barangay.FindFirstAsync(r => r.Code == barangay);
                    if (!barangayResult.Succeeded || barangayResult == null)
                    {
                        return AppResult<ActivityDTO>.CreateFailed(new ApplicationException("Can't find barangay"), "Can't find barangay");
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(region))
                    {
                        regionResult = await dataStore.Region.FindFirstAsync(r => r.Code == region);
                        if (!regionResult.Succeeded || regionResult == null)
                        {
                            return AppResult<ActivityDTO>.CreateFailed(new ApplicationException("Can't find region"), "Can't find region");
                        }
                    }

                    if (!string.IsNullOrEmpty(city))
                    {
                        cityResult = await dataStore.City.FindFirstAsync(r => r.Code == city);
                        if (!cityResult.Succeeded || cityResult == null)
                        {
                            return AppResult<ActivityDTO>.CreateFailed(new ApplicationException("Can't find city"), "Can't find city");
                        }
                    }

                    if (!string.IsNullOrEmpty(barangay))
                    {
                        barangayResult = await dataStore.Barangay.FindFirstAsync(r => r.Code == barangay);
                        if (!barangayResult.Succeeded || barangayResult == null)
                        {
                            return AppResult<ActivityDTO>.CreateFailed(new ApplicationException("Can't find barangay"), "Can't find barangay");
                        }
                    }
                }
               
            }

            // save activity address
            var activityAddress = new Entities.ActivityAddress
            {
                ActivityId = createdActitivityRes.Result.Id,
                Address1 = address1,
                Address2 = address2,
                City = city,
                CityName = cityResult.Result != null ? cityResult.Result.Name : string.Empty,
                District = district,
                Subdivision = subdivision,
                Region = region,
                RegionName = regionResult.Result != null ? regionResult.Result.Name : string.Empty,
                Barangay = barangay,
                BarangayName = barangayResult.Result != null ? barangayResult.Result.Name : string.Empty,
                PostalCode = postalcode,
                PinnedLocation = pinnedLocation
            };
            var createdActivityAddress = await dataStore.ActivityAddress.Add(activityAddress);
            if (!createdActivityAddress.Succeeded)
            {
                return AppResult<ActivityDTO>.CreateFailed(createdActivityAddress.Error.Exception, createdActivityAddress.Message);
            }

            // save activity searchTAg
            var activitySearcTag = new Entities.SearchTags
            {
                SearchTag1 = searchtag1,
                SearchTag2 = searchtag2,
                SearchTag3 = searchtag3,
                SearchTag4 = searchtag4,
                SearchTag5 = searchtag5,
                ActivityId = createdActitivityRes.Result.Id,
            };
            var createdActivitySearchTag = await dataStore.SearchTags.Add(activitySearcTag);
            if (!createdActivitySearchTag.Succeeded)
            {
                return AppResult<ActivityDTO>.CreateFailed(createdActivitySearchTag.Error.Exception, createdActivitySearchTag.Message);
            }

            var createdActivityDTO = new ActivityDTO
            {
                Id = createdActitivityRes.Result.Id,
                ExperienceTypeId = experienceTypeId,
                ActivityLevel = activityLevel,
                AdditionalRequirements = additionalRequirements,
                Address1 = address1,
                Address2 = address2,
                CanAdultsJoin = canAdultsJoin,
                City = city,
                Subdivision = subdivision,
                Region = region,
                Barangay = barangay,
                PostalCode = postalcode,
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
                Handler = handler,
                IsSetSession = IsSetSession,
                SessionName = SessionName
            };

            return AppResult<ActivityDTO>.CreateSucceeded(createdActivityDTO, "Activity successfully created");
        }
        catch (Exception ex)
        {
            return AppResult<ActivityDTO>.CreateFailed(ex, "An error occured when creating activity");
        }
    }

    public async Task<AppResult<ActivityDTO>> GetActivitieByCategoriesAsync(int experienceCategoryId, int? customerId = null, 
        bool? includeAddres = false, bool? includeDescription = false, bool? includeSearchTags = false, 
        bool? includeSchedules = false, bool? includeImages = false, bool? isActive = false, bool? includeCustomer = false)
    {
        try
        {
            var includes = new List<Expression<Func<Entities.Activity, object>>>();
            if (includeAddres.HasValue && includeAddres.Value) includes.Add(a => a.Address);
            if (includeDescription.HasValue && includeDescription.Value) includes.Add(a => a.ActivityDescription);
            if (includeSearchTags.HasValue && includeSearchTags.Value) includes.Add(a => a.SearchTag);
            if (includeSchedules.HasValue && includeSchedules.Value) includes.Add(a => a.Schedules);
            if (includeImages.HasValue && includeImages.Value) includes.Add(a => a.Images);
            if(includeCustomer.HasValue && includeCustomer.Value) includes.Add(a => a.Customer);

            Expression<Func<Entities.Activity, bool>> filter = a => (a.ExperienceCategoryId == experienceCategoryId) &&
                (customerId.HasValue ? a.CreatedBy == customerId : true) &&
                (isActive.HasValue ? a.IsPublished == isActive : true);

            var result = await dataStore.Activity.FindFirstAsync(filter, includes);
            if (!result.Succeeded || result.Result == null)
            {
                return AppResult<ActivityDTO>.CreateFailed(result.Error.Exception, result.Message);
            }
            var activity = result.Result;

            var activityDTO = new ActivityDTO
            {
                Id = activity.Id,
                SubTitle = activity.Subtitle,
                Title = activity.Title,
                Description = activity.Description,
                Price = activity.Price,
                Remarks = activity.Remarks,
                IsPublished = activity.IsPublished,
                ExperienceCategoryId = activity.ExperienceCategoryId ?? 0,
                ExperienceTypeId = activity.ExperienceTypeId,
                SubCategoryId = activity.SubCategoryId ?? 0,
                Handler = activity.Handler,
                IsSetSession = activity.IsSetSession,
                SessionName = activity.SessionName
            };

            // address fields
            if (includeAddres.HasValue && includeAddres.Value && activity.Address != null)
            {
                activityDTO.Address1 = activity.Address.Address1;
                activityDTO.Address2 = activity.Address.Address2;
                activityDTO.City = activity.Address.City;
                activityDTO.District = activity.Address.District;
                activityDTO.Subdivision = activity.Address.Subdivision;
                activityDTO.Region = activity.Address.Region;
                activityDTO.Barangay = activity.Address.Barangay;
                activityDTO.PostalCode =activity.Address.PostalCode;
            }

            // description fields
            if (includeDescription.HasValue && includeDescription.Value && activity.ActivityDescription != null)
            {
                var description = activity.ActivityDescription;
                activityDTO.ActivityLevel = description.ActivityLevel;
                activityDTO.AdditionalRequirements = description.AdditionalRequirements;
                activityDTO.CanAdultsJoin = description.CanAdultsJoin;
                activityDTO.CustomerBringWithThem = description.CustomerBringWithThem;
                activityDTO.Description = description.Description;
                activityDTO.MinimumAge = description.MinimumAge;
                activityDTO.SkillLevel = description.SkillLevel;
                activityDTO.SpecificsYouWillProvide = description.SpecificsYouWillProvide;
            }

            // schedules
            if (includeSchedules.HasValue && includeSchedules.Value && activity.Schedules != null)
            {
                activityDTO.Schedules = activity.Schedules.Select(s => {
                    return new Framework.ApiCommand.ApiData.DTO.ActivitySchedule.ActivityScheduleDTO
                    {
                        DateTime = s.DateTime,
                        Id = s.Id,
                        Name = s.Name,
                        PerUnit1 = s.PerUnit1,
                        Price = s.Price,
                        PriceUnit1 = s.PriceUnit1,
                        PriceUnit2 = s.PriceUnit2,
                        UnitPrice = s.UnitPrice,
                        PerUnit2 = s.PerUnit2,
                        Order = s.Order,
                        IsActiveSchedule = s.IsActiveSchedule
                    };
                }).ToList();
            }

            // search tags
            if (includeSearchTags.HasValue && includeSearchTags.Value && activity.SearchTag != null)
            {
                var tags = new List<string>();
                if (activity.SearchTag.SearchTag1 != null) tags.Add(activity.SearchTag.SearchTag1);
                if (activity.SearchTag.SearchTag2 != null) tags.Add(activity.SearchTag.SearchTag2);
                if (activity.SearchTag.SearchTag3 != null) tags.Add(activity.SearchTag.SearchTag3);
                if (activity.SearchTag.SearchTag4 != null) tags.Add(activity.SearchTag.SearchTag4);
                if (activity.SearchTag.SearchTag5 != null) tags.Add(activity.SearchTag.SearchTag5);

                activityDTO.SearchTags = tags;
            }

            // activity images
            if (includeImages.HasValue && includeImages.Value && activity.Images != null)
            {
                activityDTO.Images = activity.Images.Select(s => {
                    return new Framework.ApiCommand.ApiData.DTO.ActivityImage.ActivityImageDTO
                    {
                        ActivityId = s.ActivityId,
                        Id = s.Id,
                        ImageLocation = s.ImageLocation,
                        ImageName = s.ImageName
                    };
                }).ToList();
            }

            // owner
            if(includeCustomer.HasValue && includeCustomer.Value && activity.Customer != null)
            {
                var customer = activity.Customer;
                activityDTO.Owner = new Framework.ApiCommand.ApiData.DTO.Customer.CustomerDTO {
                    About = customer.About,
                    Birthdate = customer.Birthdate,
                    DateJoined = customer.DateJoined,
                    Email = customer.Email,
                    ExternalLogin = customer.ExternalLogin,
                    FirstName = customer.FirstName,
                    Handler = customer.Handler,
                    Id = customer.Id,
                    IsMaker = customer.IsMaker,
                    IsVerified = customer.IsVerifiedBadge,
                    LastName = customer.LastName,
                    ProfileImg = customer.ProfilePath
                };
            }

            return AppResult<ActivityDTO>.CreateSucceeded(activityDTO, "Successfully getting activity by id");
        }
        catch (Exception ex)
        {
            return AppResult<ActivityDTO>.CreateFailed(ex, "An error occured when getting activity by id");
        }
    }

    public async Task<AppResult<IEnumerable<ActivityDTO>>> GetAllAsync(int? customerId, bool? isActive, int? count, int? skip,
        int experienceCategoryId, string searchValue, bool? isDeactivated, Enums.ActivityStatus? status,
        bool includeAddres = false, bool includeDescription = false, bool includeSearchTags = false,
        bool includeSchedules = false, bool includeImages = false, IEnumerable<int>? ids = null, string? likeHandler = null,
        bool includeCustomer = false, bool includeExperienceTypes = false, bool includeExperienceCategories = false, 
        bool includeSubCategories = false, bool includeStudents = false, bool includeReviews = false)
    {
        try
        {
            var includes = new List<Expression<Func<Entities.Activity, object>>>();
            if (includeAddres) includes.Add(a => a.Address);
            if (includeDescription) includes.Add(a => a.ActivityDescription);
            if (includeSearchTags) includes.Add(a => a.SearchTag);
            if (includeSchedules) includes.Add(a => a.Schedules);
            if (includeImages) includes.Add(a => a.Images);
            if (includeCustomer) includes.Add(a => a.Customer);
            if (includeExperienceTypes) includes.Add(a => a.ExperienceType);
            if (includeExperienceCategories) includes.Add(a => a.ExperienceCategory);
            if (includeSubCategories) includes.Add(a => a.SubCategory);
            if (includeStudents) includes.Add(a => a.Students);
            if (includeReviews) includes.Add(a => a.Reviews);

            Expression<Func<Entities.Activity, bool>> filter =
                a => (ids != null ? ids.Contains(a.Id) : true) &&
                        (isActive.HasValue ? a.IsPublished == isActive.Value : true) &&
                        (customerId.HasValue ? a.CreatedBy == customerId.Value : true) &&
                        (string.IsNullOrEmpty(likeHandler) ? true : a.Handler.ToLower().Contains(likeHandler.ToLower())) &&
                        (experienceCategoryId != 0 ? experienceCategoryId == 1 ? (DateTime.UtcNow - a.CreatedOn).Days <= 30 : a.ExperienceCategoryId == experienceCategoryId : true) &&
                        (isDeactivated.HasValue ? a.IsDeactivated == isDeactivated.Value : true) &&
                        (status.HasValue ? a.Status == (int)status.Value : true);

            var result = await dataStore.Activity.FindActivitiesAsync(filter, searchValue, count, skip, includes);
            if (!result.Succeeded || result.Result == null)
            {
                return AppResult<IEnumerable<ActivityDTO>>.CreateFailed(result.Error.Exception, result.Message);
            }

            var activities = result.Result.Select(a =>
            {
                var activityDTO = new ActivityDTO
                {
                    Id                   = a.Id,
                    SubTitle             = a.Subtitle,
                    Title                = a.Title,
                    Description          = a.Description,
                    Price                = a.Price,
                    Remarks              = a.Remarks,
                    IsPublished          = a.IsPublished,
                    ExperienceCategoryId = a.ExperienceCategoryId ?? 0,
                    SubCategoryId        = a.SubCategoryId ?? 0,
                    CreatedBy            = a.CreatedBy,
                    CreatedOn            = a.CreatedOn,
                    ExperienceTypeId     = a.ExperienceTypeId,
                    Handler              = a.Handler,
                    ExperienceType       = a.ExperienceType?.Name,
                    ExperienceCategory   = a.ExperienceCategory?.Category,
                    SubCategory          = a.SubCategory?.SubCatergory,
                    IsNew                = (DateTime.UtcNow - a.CreatedOn).Days <= 30,
                    IsSetSession         = a.IsSetSession,
                    SessionName          = a.SessionName,
                    IsDeactivated        = a.IsDeactivated,
                    Status               = (Enums.ActivityStatus)a.Status
                };

                // address fields
                if (includeAddres && a.Address != null)
                {
                    activityDTO.Address1 = a.Address.Address1;
                    activityDTO.Address2 = a.Address.Address2;
                    activityDTO.City = a.Address.City;
                    activityDTO.District = a.Address.District;
                    activityDTO.Subdivision = a.Address.Subdivision;
                    activityDTO.Region = a.Address.Region;
                    activityDTO.Barangay = a.Address.Barangay;
                    activityDTO.PostalCode = a.Address.PostalCode;
                    activityDTO.CityName = a.Address.CityName;
                    activityDTO.RegionName = a.Address.RegionName;
                    activityDTO.BarangayName = a.Address.BarangayName;
                }

                // description fields
                if (includeDescription && a.ActivityDescription != null)
                {
                    var description = a.ActivityDescription;
                    activityDTO.ActivityLevel = description.ActivityLevel;
                    activityDTO.AdditionalRequirements = description.AdditionalRequirements;
                    activityDTO.CanAdultsJoin = description.CanAdultsJoin;
                    activityDTO.CustomerBringWithThem = description.CustomerBringWithThem;
                    activityDTO.Description = description.Description;
                    activityDTO.MinimumAge = description.MinimumAge;
                    activityDTO.SkillLevel = description.SkillLevel;
                    activityDTO.SpecificsYouWillProvide = description.SpecificsYouWillProvide;
                }

                // schedules
                if (includeSchedules && a.Schedules != null)
                {
                    activityDTO.Schedules = a.Schedules.Select(s =>
                    {
                        return new Framework.ApiCommand.ApiData.DTO.ActivitySchedule.ActivityScheduleDTO
                        {
                            DateTime = s.DateTime,
                            Id = s.Id,
                            Name = s.Name,
                            PerUnit1 = s.PerUnit1,
                            Price = s.Price,
                            PriceUnit1 = s.PriceUnit1,
                            PriceUnit2 = s.PriceUnit2,
                            UnitPrice = s.UnitPrice,
                            PerUnit2 = s.PerUnit2,
                            Order = s.Order,
                            IsActiveSchedule = s.IsActiveSchedule
                        };
                    }).ToList();
                }

                // search tags
                if (includeSearchTags && a.SearchTag != null)
                {
                    var tags = new List<string>();
                    if (a.SearchTag.SearchTag1 != null) tags.Add(a.SearchTag.SearchTag1);
                    if (a.SearchTag.SearchTag2 != null) tags.Add(a.SearchTag.SearchTag2);
                    if (a.SearchTag.SearchTag3 != null) tags.Add(a.SearchTag.SearchTag3);
                    if (a.SearchTag.SearchTag4 != null) tags.Add(a.SearchTag.SearchTag4);
                    if (a.SearchTag.SearchTag5 != null) tags.Add(a.SearchTag.SearchTag5);

                    activityDTO.SearchTags = tags;
                }

                // activity images
                if (includeImages && a.Images != null)
                {
                    activityDTO.Images = a.Images.Select(s =>
                    {
                        return new Framework.ApiCommand.ApiData.DTO.ActivityImage.ActivityImageDTO
                        {
                            ActivityId = s.ActivityId,
                            Id = s.Id,
                            ImageLocation = s.ImageLocation,
                            ImageName = s.ImageName,
                            Order = s.Order
                        };
                    }).ToList();
                }

                // customer
                if (includeCustomer && a.Customer != null)
                {
                    var customer = a.Customer;
                    activityDTO.Owner = new Framework.ApiCommand.ApiData.DTO.Customer.CustomerDTO
                    {
                        About = customer.About,
                        Birthdate = customer.Birthdate,
                        DateJoined = customer.DateJoined,
                        Email = customer.Email,
                        ExternalLogin = customer.ExternalLogin,
                        FirstName = customer.FirstName,
                        Handler = customer.Handler,
                        Id = customer.Id,
                        IsMaker = customer.IsMaker,
                        IsVerified = customer.IsVerifiedBadge,
                        IsOG = customer.IsOG,
                        IsOfficial = customer.IsOfficialPartner,
                        LastName = customer.LastName,
                        ProfileImg = customer.ProfilePath
                    };
                }

                // student
                if (includeStudents && a.Students != null)
                {
                    var students = a.Students;
                    activityDTO.CompletedStudents = students.Count(a => a.SessionsAttended >= a.NumberOfSessions);
                    activityDTO.OngoingStudents = students.Count(a => a.SessionsAttended < a.NumberOfSessions);
                }

                // reviews
                if (includeReviews && a.Reviews != null)
                {
                    var reviews = a.Reviews;
                    double sumOfRating = reviews.Sum(a => a.Rating);
                    int numberOfRatee = reviews.Count;
                    activityDTO.NumberOfReviews = numberOfRatee;
                    activityDTO.AverageRating = Math.Round(sumOfRating / numberOfRatee, 1);
                }

                return activityDTO;
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
                    ExperienceCategoryId = a.ExperienceCategoryId ?? 0,
                    SubCategoryId = a.SubCategoryId ?? 0,
                    CreatedBy = a.CreatedBy,
                    ExperienceTypeId = a.ExperienceTypeId,
                    MapDetails = a.MapDetails,
                    Handler = a.Handler,
                    IsSetSession = a.IsSetSession,
                    SessionName = a.SessionName
                };
            });

            return AppResult<IEnumerable<ActivityDTO>>.CreateSucceeded(activities, "Successfully get activities");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<ActivityDTO>>.CreateFailed(ex, "An error occured in getting activities");
        }
    }

    public async Task<AppResult<ActivityDTO>> GetByIdAsync(int id, int? customerId = null,
        bool? includeAddres = false, bool? includeDescription = false, bool? includeSearchTags = false,
        bool? includeSchedules = false, bool? includeImages = false, bool? isActive = false, bool? includeCustomer = false)
    {
        try
        {
            var includes = new List<Expression<Func<Entities.Activity, object>>>();
            if(includeAddres.HasValue && includeAddres.Value) includes.Add(a => a.Address);
            if(includeDescription.HasValue && includeDescription.Value) includes.Add(a => a.ActivityDescription);
            if(includeSearchTags.HasValue && includeSearchTags.Value) includes.Add(a => a.SearchTag);
            if(includeSchedules.HasValue && includeSchedules.Value) includes.Add(a => a.Schedules);
            if(includeImages.HasValue && includeImages.Value) includes.Add(a => a.Images);
            if(includeCustomer.HasValue && includeCustomer.Value) includes.Add(a => a.Customer);

            Expression<Func<Entities.Activity, bool>> filter = a => (a.Id == id) &&
                (customerId.HasValue ? a.CreatedBy == customerId : true) &&
                (isActive.HasValue ? a.IsPublished == isActive : true);

            var result = await dataStore.Activity.FindFirstAsync(filter, includes);
            if (!result.Succeeded || result.Result == null)
            {
                return AppResult<ActivityDTO>.CreateFailed(result.Error.Exception, result.Message);
            }
            var activity = result.Result;

            var activityDTO = new ActivityDTO
            {
                Id = activity.Id,
                SubTitle = activity.Subtitle,
                Title = activity.Title,
                Description = activity.Description,
                Price = activity.Price,
                Remarks = activity.Remarks,
                IsPublished = activity.IsPublished,
                ExperienceCategoryId = activity.ExperienceCategoryId ?? 0,
                ExperienceTypeId = activity.ExperienceTypeId,
                SubCategoryId = activity.SubCategoryId ?? 0,
                CreatedBy = activity.CreatedBy,
                MapDetails = activity.MapDetails,
                Handler = activity.Handler,
                IsSetSession = activity.IsSetSession,
                SessionName = activity.SessionName,
                Status = (Enums.ActivityStatus)activity.Status
            };

            // address fields
            if(includeAddres.HasValue && includeAddres.Value && activity.Address != null)
            {
                activityDTO.Address1 = activity.Address.Address1;
                activityDTO.Address2 = activity.Address.Address2;
                activityDTO.City = activity.Address.City;
                activityDTO.CityName = activity.Address.CityName;
                activityDTO.District = activity.Address.District;
                activityDTO.Subdivision = activity.Address.Subdivision;
                activityDTO.Region = activity.Address.Region;
                activityDTO.RegionName = activity.Address.RegionName;
                activityDTO.Barangay = activity.Address.Barangay;
                activityDTO.BarangayName = activity.Address.BarangayName;
                activityDTO.PostalCode = activity.Address.PostalCode;
                activityDTO.PinnedLocation = activity.Address.PinnedLocation;
            }

            // description fields
            if(includeDescription.HasValue && includeDescription.Value && activity.ActivityDescription != null)
            {
                var description = activity.ActivityDescription;
                activityDTO.ActivityLevel = description.ActivityLevel;
                activityDTO.AdditionalRequirements = description.AdditionalRequirements;
                activityDTO.CanAdultsJoin = description.CanAdultsJoin;
                activityDTO.CustomerBringWithThem = description.CustomerBringWithThem;
                activityDTO.Description = description.Description;
                activityDTO.MinimumAge = description.MinimumAge;
                activityDTO.SkillLevel = description.SkillLevel;
                activityDTO.SpecificsYouWillProvide = description.SpecificsYouWillProvide;
            }

            // schedules
            if(includeSchedules.HasValue && includeSchedules.Value && activity.Schedules != null)
            {
                activityDTO.Schedules = activity.Schedules.Select(s => {
                    return new Framework.ApiCommand.ApiData.DTO.ActivitySchedule.ActivityScheduleDTO {
                        DateTime = s.DateTime,
                        Id = s.Id,
                        Name = s.Name,
                        PerUnit1 = s.PerUnit1,
                        Price = s.Price,
                        PriceUnit1 = s.PriceUnit1,
                        PriceUnit2 = s.PriceUnit2,
                        UnitPrice = s.UnitPrice,
                        PerUnit2 = s.PerUnit2,
                        Order = s.Order,
                        IsActiveSchedule = s.IsActiveSchedule
                    };
                }).ToList();
            }

            // search tags
            if(includeSearchTags.HasValue && includeSearchTags.Value && activity.SearchTag != null)
            {
                var tags = new List<string>();
                if(activity.SearchTag.SearchTag1 != null) tags.Add(activity.SearchTag.SearchTag1);
                if(activity.SearchTag.SearchTag2 != null) tags.Add(activity.SearchTag.SearchTag2);
                if(activity.SearchTag.SearchTag3 != null) tags.Add(activity.SearchTag.SearchTag3);
                if(activity.SearchTag.SearchTag4 != null) tags.Add(activity.SearchTag.SearchTag4);
                if(activity.SearchTag.SearchTag5 != null) tags.Add(activity.SearchTag.SearchTag5);

                activityDTO.SearchTags = tags;
            }

            // activity images
            if(includeImages.HasValue && includeImages.Value && activity.Images != null)
            {
                activityDTO.Images = activity.Images.Select(s => {
                    return new Framework.ApiCommand.ApiData.DTO.ActivityImage.ActivityImageDTO {
                        ActivityId = s.ActivityId,
                        Id = s.Id,
                        ImageLocation = s.ImageLocation,
                        ImageName = s.ImageName,
                        Order = s.Order,
                    };
                }).ToList();
            }

            // customer
            if(includeCustomer.HasValue && includeCustomer.Value && activity.Customer != null)
            {
                var customer = activity.Customer;
                activityDTO.Owner = new Framework.ApiCommand.ApiData.DTO.Customer.CustomerDTO {
                    About = customer.About,
                    Birthdate = customer.Birthdate,
                    DateJoined = customer.DateJoined,
                    Email = customer.Email,
                    ExternalLogin = customer.ExternalLogin,
                    FirstName = customer.FirstName,
                    Handler = customer.Handler,
                    Id = customer.Id,
                    IsMaker = customer.IsMaker,
                    IsVerified = customer.IsVerifiedBadge,
                    IsOG = customer.IsOG,
                    IsOfficial = customer.IsOfficialPartner,
                    LastName = customer.LastName,
                    ProfileImg = customer.ProfilePath,
                    PhoneNumber = customer.PhoneNumber
                };
            }

            return AppResult<ActivityDTO>.CreateSucceeded(activityDTO, "Successfully getting activity by id");
        }
        catch (Exception ex)
        {
            return AppResult<ActivityDTO>.CreateFailed(ex, "An error occured when getting activity by id");
        }
    }

    public async Task<AppResult<ActivityDTO>> GetByHandlerAsync(string handler, int? customerId = null,
        bool? includeAddres = false, bool? includeDescription = false, bool? includeSearchTags = false,
        bool? includeSchedules = false, bool? includeImages = false, bool? isActive = false, bool? includeCustomer = false)
    {
        try
        {
            var includes = new List<Expression<Func<Entities.Activity, object>>>();
            if(includeAddres.HasValue && includeAddres.Value) includes.Add(a => a.Address);
            if(includeDescription.HasValue && includeDescription.Value) includes.Add(a => a.ActivityDescription);
            if(includeSearchTags.HasValue && includeSearchTags.Value) includes.Add(a => a.SearchTag);
            if(includeSchedules.HasValue && includeSchedules.Value) includes.Add(a => a.Schedules);
            if(includeImages.HasValue && includeImages.Value) includes.Add(a => a.Images);
            if(includeCustomer.HasValue && includeCustomer.Value) includes.Add(a => a.Customer);

            Expression<Func<Entities.Activity, bool>> filter = a => (a.Handler == handler) &&
                (customerId.HasValue ? a.CreatedBy == customerId : true) &&
                (isActive.HasValue ? a.IsPublished == isActive : true);

            var result = await dataStore.Activity.FindFirstAsync(filter, includes);
            if (!result.Succeeded || result.Result == null)
            {
                return AppResult<ActivityDTO>.CreateFailed(result.Error.Exception, result.Message);
            }
            var activity = result.Result;

            var activityDTO = new ActivityDTO
            {
                Id = activity.Id,
                SubTitle = activity.Subtitle,
                Title = activity.Title,
                Description = activity.Description,
                Price = activity.Price,
                Remarks = activity.Remarks,
                IsPublished = activity.IsPublished,
                ExperienceCategoryId = activity.ExperienceCategoryId ?? 0,
                ExperienceTypeId = activity.ExperienceTypeId,
                SubCategoryId = activity.SubCategoryId ?? 0,
                CreatedBy = activity.CreatedBy,
                MapDetails = activity.MapDetails,
                Handler = activity.Handler,
                IsSetSession = activity.IsSetSession,
                SessionName = activity.SessionName
            };

            // address fields
            if(includeAddres.HasValue && includeAddres.Value && activity.Address != null)
            {
                activityDTO.Address1 = activity.Address.Address1;
                activityDTO.Address2 = activity.Address.Address2;
                activityDTO.City = activity.Address.City;
                activityDTO.CityName = activity.Address.CityName;
                activityDTO.District = activity.Address.District;
                activityDTO.Subdivision = activity.Address.Subdivision;
                activityDTO.Region = activity.Address.Region;
                activityDTO.RegionName = activity.Address.RegionName;
                activityDTO.Barangay = activity.Address.Barangay;
                activityDTO.BarangayName = activity.Address.BarangayName;
                activityDTO.PostalCode = activity.Address.PostalCode;
                activityDTO.PinnedLocation = activity.Address.PinnedLocation;
            }

            // description fields
            if(includeDescription.HasValue && includeDescription.Value && activity.ActivityDescription != null)
            {
                var description = activity.ActivityDescription;
                activityDTO.ActivityLevel = description.ActivityLevel;
                activityDTO.AdditionalRequirements = description.AdditionalRequirements;
                activityDTO.CanAdultsJoin = description.CanAdultsJoin;
                activityDTO.CustomerBringWithThem = description.CustomerBringWithThem;
                activityDTO.Description = description.Description;
                activityDTO.MinimumAge = description.MinimumAge;
                activityDTO.SkillLevel = description.SkillLevel;
                activityDTO.SpecificsYouWillProvide = description.SpecificsYouWillProvide;
            }

            // schedules
            if(includeSchedules.HasValue && includeSchedules.Value && activity.Schedules != null)
            {
                activityDTO.Schedules = activity.Schedules.Select(s => {
                    return new Framework.ApiCommand.ApiData.DTO.ActivitySchedule.ActivityScheduleDTO {
                        DateTime = s.DateTime,
                        Id = s.Id,
                        Name = s.Name,
                        PerUnit1 = s.PerUnit1,
                        Price = s.Price,
                        PriceUnit1 = s.PriceUnit1,
                        PriceUnit2 = s.PriceUnit2,
                        UnitPrice = s.UnitPrice,
                        PerUnit2 = s.PerUnit2,
                        Order = s.Order,
                        IsActiveSchedule = s.IsActiveSchedule
                    };
                }).ToList();
            }

            // search tags
            if(includeSearchTags.HasValue && includeSearchTags.Value && activity.SearchTag != null)
            {
                var tags = new List<string>();
                if(activity.SearchTag.SearchTag1 != null) tags.Add(activity.SearchTag.SearchTag1);
                if(activity.SearchTag.SearchTag2 != null) tags.Add(activity.SearchTag.SearchTag2);
                if(activity.SearchTag.SearchTag3 != null) tags.Add(activity.SearchTag.SearchTag3);
                if(activity.SearchTag.SearchTag4 != null) tags.Add(activity.SearchTag.SearchTag4);
                if(activity.SearchTag.SearchTag5 != null) tags.Add(activity.SearchTag.SearchTag5);

                activityDTO.SearchTags = tags;
            }

            // activity images
            if(includeImages.HasValue && includeImages.Value && activity.Images != null)
            {
                activityDTO.Images = activity.Images.Select(s => {
                    return new Framework.ApiCommand.ApiData.DTO.ActivityImage.ActivityImageDTO {
                        ActivityId = s.ActivityId,
                        Id = s.Id,
                        ImageLocation = s.ImageLocation,
                        ImageName = s.ImageName,
                        Order = s.Order
                    };
                }).ToList();
            }

            // customer
            if(includeCustomer.HasValue && includeCustomer.Value && activity.Customer != null)
            {
                var customer = activity.Customer;
                activityDTO.Owner = new Framework.ApiCommand.ApiData.DTO.Customer.CustomerDTO {
                    About = customer.About,
                    Birthdate = customer.Birthdate,
                    DateJoined = customer.DateJoined,
                    Email = customer.Email,
                    ExternalLogin = customer.ExternalLogin,
                    FirstName = customer.FirstName,
                    Handler = customer.Handler,
                    Id = customer.Id,
                    IsMaker = customer.IsMaker,
                    IsVerified = customer.IsVerifiedBadge,
                    IsOG = customer.IsOG,
                    IsOfficial = customer.IsOfficialPartner,
                    LastName = customer.LastName,
                    ProfileImg = customer.ProfilePath
                };
            }

            return AppResult<ActivityDTO>.CreateSucceeded(activityDTO, "Successfully getting activity by id");
        }
        catch (Exception ex)
        {
            return AppResult<ActivityDTO>.CreateFailed(ex, "An error occured when getting activity by id");
        }
    }

    public async Task<AppResult<ActivityDTO>> UpdateActivityAsync(int activityId,int? experienceTypeId, string? title, string? description, string? price, 
        string? scheduleIndicator, string? remarks, bool? isPublished, string? address1, string? address2, string? district, string? city, string? subdivision, string? region,
        string? barangay, string? postalcode,string? specificsYouWillProvide, string? customerBringWithThem, string? additionalRequirements, string? activityLevel, 
        string? skillLevel, int? minimumAge, bool? canAdultsJoin, string? searchtag1, string? searhtag2, string? searchtag3, string? searchtag4, 
        string? searchtag5, int? experienceCategoryId, int? subCategoryId, bool? IsSetSession, string? SessionName, string pinnedLocation, 
        bool? isDeactivated, Enums.ActivityStatus? status, string? handler)
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

            // update experience category field
            if(experienceCategoryId.HasValue)
            {
                // check id if existed
                var cat = await dataStore.ExperienceCategory.GetByIdAsync(experienceCategoryId.Value);
                if(!cat.Succeeded || cat.Result == null)
                {
                    return AppResult<ActivityDTO>.CreateFailed(cat.Error.Exception, cat.Message);
                }

                activity.ExperienceCategoryId = experienceCategoryId.Value;
            }

            // update sub category field
            if(subCategoryId.HasValue)
            {
                var sub = await dataStore.SubCategory.GetByIdAsync(subCategoryId.Value);
                if(!sub.Succeeded || sub.Result == null)
                {
                    return AppResult<ActivityDTO>.CreateFailed(sub.Error.Exception, sub.Message);
                }

                activity.SubCategoryId = subCategoryId.Value;
            }

            activity.Title = title ?? activity.Title;
            activity.Description = description ?? activity.Description;
            activity.Price = price ?? activity.Price;
            activity.ScheduleIndicator = scheduleIndicator ?? activity.ScheduleIndicator;
            activity.Remarks = remarks ?? activity.Remarks;
            activity.IsPublished = isPublished ?? activity.IsPublished;
            activity.SessionName = SessionName ?? activity.SessionName;
            activity.IsSetSession = IsSetSession ?? activity.IsSetSession;
            activity.IsDeactivated = isDeactivated ?? activity.IsDeactivated;
            activity.Status = status.HasValue ? (int)status.GetValueOrDefault() : activity.Status;
            activity.Handler = handler ?? activity.Handler;

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

            var regionResult = new AppResult<Region>();
            var cityResult = new AppResult<City>();
            var barangayResult = new AppResult<Barangay>();

            if (experienceTypeId == 1)
            {
                if (status == ActivityStatus.Submitted)
                {
                    regionResult = await dataStore.Region.FindFirstAsync(r => r.Code == (region ?? activityAddress.Region));
                    if (!regionResult.Succeeded || regionResult == null)
                    {
                        return AppResult<ActivityDTO>.CreateFailed(new ApplicationException("Can't find region"), "Can't find region");
                    }

                    cityResult = await dataStore.City.FindFirstAsync(r => r.Code == (city ?? activityAddress.City));
                    if (!cityResult.Succeeded || cityResult == null)
                    {
                        return AppResult<ActivityDTO>.CreateFailed(new ApplicationException("Can't find city"), "Can't find city");
                    }

                    barangayResult = await dataStore.Barangay.FindFirstAsync(r => r.Code == (barangay ?? activityAddress.Barangay));
                    if (!barangayResult.Succeeded || barangayResult == null)
                    {
                        return AppResult<ActivityDTO>.CreateFailed(new ApplicationException("Can't find barangay"), "Can't find barangay");
                    }

                    activityAddress.CityName = cityResult.Result != null ? cityResult.Result.Name : string.Empty;
                    activityAddress.RegionName = regionResult.Result != null ? regionResult.Result.Name : string.Empty;
                    activityAddress.BarangayName = barangayResult.Result != null ? barangayResult.Result.Name : string.Empty;
                }
                else
                {
                    if (!string.IsNullOrEmpty(region) || !string.IsNullOrEmpty(activityAddress.Region))
                    {
                        regionResult = await dataStore.Region.FindFirstAsync(r => r.Code == (region ?? activityAddress.Region));
                        if (!regionResult.Succeeded || regionResult == null)
                        {
                            return AppResult<ActivityDTO>.CreateFailed(new ApplicationException("Can't find region"), "Can't find region");
                        }
                    }

                    if (!string.IsNullOrEmpty(city) || !string.IsNullOrEmpty(activityAddress.City))
                    {
                        cityResult = await dataStore.City.FindFirstAsync(r => r.Code == (city ?? activityAddress.City));
                        if (!cityResult.Succeeded || cityResult == null)
                        {
                            return AppResult<ActivityDTO>.CreateFailed(new ApplicationException("Can't find city"), "Can't find city");
                        }
                    }

                    if (!string.IsNullOrEmpty(barangay) || !string.IsNullOrEmpty(activityAddress.Barangay))
                    {
                        barangayResult = await dataStore.Barangay.FindFirstAsync(r => r.Code == (barangay ?? activityAddress.Barangay));
                        if (!barangayResult.Succeeded || barangayResult == null)
                        {
                            return AppResult<ActivityDTO>.CreateFailed(new ApplicationException("Can't find barangay"), "Can't find barangay");
                        }
                    }

                    activityAddress.CityName = cityResult.Result != null ? cityResult.Result.Name : string.Empty;
                    activityAddress.RegionName = regionResult.Result != null ? regionResult.Result.Name : string.Empty;
                    activityAddress.BarangayName = barangayResult.Result != null ? barangayResult.Result.Name : string.Empty;
                }
               
            }

            activityAddress.Address1       = address1 ?? activityAddress.Address1;
            activityAddress.Address2       = address2 ?? activityAddress.Address2;
            activityAddress.District       = district ?? activityAddress.District;
            activityAddress.City           = city ?? activityAddress.City;
            activityAddress.Subdivision    = subdivision?? activityAddress.Subdivision;    
            activityAddress.Region         = region?? activityAddress.Region;
            activityAddress.Barangay       = barangay?? activityAddress.Barangay; 
            activityAddress.PostalCode     = postalcode?? activityAddress.PostalCode;
            activityAddress.PinnedLocation = pinnedLocation?? activityAddress.PinnedLocation;

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
                Subdivision = activityAddress.Subdivision,
                Region = activityAddress.Region,
                Barangay = activityAddress.Barangay,
                PostalCode = activityAddress.PostalCode,    
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
                SubTitle = activity.Subtitle,
                ExperienceCategoryId = activity.ExperienceCategoryId ?? 0,
                SubCategoryId = activity.SubCategoryId ?? 0,
                Handler = activity.Handler,
                IsSetSession = activity.IsSetSession,
                SessionName = activity.SessionName
            }, "Successfully updated activity details");

        }
        catch (Exception ex)
        {
            return AppResult<ActivityDTO>.CreateFailed(ex, "An error occured when updating activity");
        }
    }

    public async Task<AppResult<IEnumerable<ActivityDTO>>> GetPopularActivitiesAsync(int? customerId, bool? isActive, int? count, int? skip, bool? isDeactivated, bool includeAddres = false, bool includeDescription = false, bool includeSearchTags = false, bool includeSchedules = false, bool includeImages = false, IEnumerable<int>? ids = null, bool includeCustomer = false, bool includeExperienceTypes = false, bool includeExperienceCategories = false, bool includeSubCategories = false, bool includeStudents = false, bool includeReviews = false)
    {
        try
        {
            var includes = new List<Expression<Func<Entities.Activity, object>>>();
            if (includeAddres) includes.Add(a => a.Address);
            if (includeDescription) includes.Add(a => a.ActivityDescription);
            if (includeSearchTags) includes.Add(a => a.SearchTag);
            if (includeSchedules) includes.Add(a => a.Schedules);
            if (includeImages) includes.Add(a => a.Images);
            if (includeCustomer) includes.Add(a => a.Customer);
            if (includeExperienceTypes) includes.Add(a => a.ExperienceType);
            if (includeExperienceCategories) includes.Add(a => a.ExperienceCategory);
            if (includeSubCategories) includes.Add(a => a.SubCategory);
            if (includeStudents) includes.Add(a => a.Students);
            if (includeReviews) includes.Add(a => a.Reviews);

            Expression<Func<Entities.Activity, bool>> filter =
                a => (ids != null ? ids.Contains(a.Id) : true) &&
                        (isActive.HasValue ? a.IsPublished == isActive.Value : true) &&
                        (customerId.HasValue ? a.CreatedBy == customerId.Value : true) && a.PurchaseOrderCount > 0 && !a.IsNew &&
                        (isDeactivated.HasValue ? a.IsDeactivated == isDeactivated.Value : true);


            var result = await dataStore.Activity.GetPopularActivities(filter, count, skip, includes);
            if (!result.Succeeded || result.Result == null)
            {
                return AppResult<IEnumerable<ActivityDTO>>.CreateFailed(result.Error.Exception, result.Message);
            }

            var activities = result.Result.Select(a =>
            {
                var activityDTO = new ActivityDTO
                {
                    Id                   = a.Id,
                    SubTitle             = a.Subtitle,
                    Title                = a.Title,
                    Description          = a.Description,
                    Price                = a.Price,
                    Remarks              = a.Remarks,
                    IsPublished          = a.IsPublished,
                    ExperienceCategoryId = a.ExperienceCategoryId ?? 0,
                    SubCategoryId        = a.SubCategoryId ?? 0,
                    CreatedBy            = a.CreatedBy,
                    ExperienceTypeId     = a.ExperienceTypeId,
                    Handler              = a.Handler,
                    ExperienceType       = a.ExperienceType?.Name,
                    ExperienceCategory   = a.ExperienceCategory?.Category,
                    SubCategory          = a.SubCategory?.SubCatergory,
                    IsNew                = (DateTime.UtcNow - a.CreatedOn).Days <= 30,
                    IsSetSession         = a.IsSetSession,
                    SessionName          = a.SessionName
                };

                // address fields
                if (includeAddres && a.Address != null)
                {
                    activityDTO.Address1 = a.Address.Address1;
                    activityDTO.Address2 = a.Address.Address2;
                    activityDTO.City = a.Address.City;
                    activityDTO.District = a.Address.District;
                    activityDTO.Subdivision = a.Address.Subdivision;
                    activityDTO.Region = a.Address.Region;
                    activityDTO.Barangay = a.Address.Barangay;
                    activityDTO.PostalCode = a.Address.PostalCode;
                    activityDTO.CityName = a.Address.CityName;
                    activityDTO.RegionName = a.Address.RegionName;
                    activityDTO.BarangayName = a.Address.BarangayName;
                }

                // description fields
                if (includeDescription && a.ActivityDescription != null)
                {
                    var description = a.ActivityDescription;
                    activityDTO.ActivityLevel = description.ActivityLevel;
                    activityDTO.AdditionalRequirements = description.AdditionalRequirements;
                    activityDTO.CanAdultsJoin = description.CanAdultsJoin;
                    activityDTO.CustomerBringWithThem = description.CustomerBringWithThem;
                    activityDTO.Description = description.Description;
                    activityDTO.MinimumAge = description.MinimumAge;
                    activityDTO.SkillLevel = description.SkillLevel;
                    activityDTO.SpecificsYouWillProvide = description.SpecificsYouWillProvide;
                }

                // schedules
                if (includeSchedules && a.Schedules != null)
                {
                    activityDTO.Schedules = a.Schedules.Select(s =>
                    {
                        return new Framework.ApiCommand.ApiData.DTO.ActivitySchedule.ActivityScheduleDTO
                        {
                            DateTime = s.DateTime,
                            Id = s.Id,
                            Name = s.Name,
                            PerUnit1 = s.PerUnit1,
                            Price = s.Price,
                            PriceUnit1 = s.PriceUnit1,
                            PriceUnit2 = s.PriceUnit2,
                            UnitPrice = s.UnitPrice,
                            PerUnit2 = s.PerUnit2,
                            Order = s.Order,
                            IsActiveSchedule = s.IsActiveSchedule
                        };
                    }).ToList();
                }

                // search tags
                if (includeSearchTags && a.SearchTag != null)
                {
                    var tags = new List<string>();
                    if (a.SearchTag.SearchTag1 != null) tags.Add(a.SearchTag.SearchTag1);
                    if (a.SearchTag.SearchTag2 != null) tags.Add(a.SearchTag.SearchTag2);
                    if (a.SearchTag.SearchTag3 != null) tags.Add(a.SearchTag.SearchTag3);
                    if (a.SearchTag.SearchTag4 != null) tags.Add(a.SearchTag.SearchTag4);
                    if (a.SearchTag.SearchTag5 != null) tags.Add(a.SearchTag.SearchTag5);

                    activityDTO.SearchTags = tags;
                }

                // activity images
                if (includeImages && a.Images != null)
                {
                    activityDTO.Images = a.Images.Select(s =>
                    {
                        return new Framework.ApiCommand.ApiData.DTO.ActivityImage.ActivityImageDTO
                        {
                            ActivityId = s.ActivityId,
                            Id = s.Id,
                            ImageLocation = s.ImageLocation,
                            ImageName = s.ImageName,
                            Order = s.Order
                        };
                    }).ToList();
                }

                // customer
                if (includeCustomer && a.Customer != null)
                {
                    var customer = a.Customer;
                    activityDTO.Owner = new Framework.ApiCommand.ApiData.DTO.Customer.CustomerDTO
                    {
                        About = customer.About,
                        Birthdate = customer.Birthdate,
                        DateJoined = customer.DateJoined,
                        Email = customer.Email,
                        ExternalLogin = customer.ExternalLogin,
                        FirstName = customer.FirstName,
                        Handler = customer.Handler,
                        Id = customer.Id,
                        IsMaker = customer.IsMaker,
                        IsVerified = customer.IsVerifiedBadge,
                        IsOG = customer.IsOG,
                        IsOfficial = customer.IsOfficialPartner,
                        LastName = customer.LastName,
                        ProfileImg = customer.ProfilePath
                    };
                }

                // student
                if (includeStudents && a.Students != null)
                {
                    var students = a.Students;
                    activityDTO.CompletedStudents = students.Count(a => a.SessionsAttended >= a.NumberOfSessions);
                    activityDTO.OngoingStudents = students.Count(a => a.SessionsAttended < a.NumberOfSessions);
                }

                // reviews
                if (includeReviews && a.Reviews != null)
                {
                    var reviews = a.Reviews;
                    double sumOfRating = reviews.Sum(a => a.Rating);
                    int numberOfRatee = reviews.Count;
                    activityDTO.NumberOfReviews = numberOfRatee;
                    activityDTO.AverageRating = Math.Round(sumOfRating / numberOfRatee, 1);
                }

                return activityDTO;
            });

            return AppResult<IEnumerable<ActivityDTO>>.CreateSucceeded(activities, "Successfully get activities");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<ActivityDTO>>.CreateFailed(ex, "An error occured in getting activities");
        }
    }

    public async Task<AppResult<bool>> UpdateActivityGuid()
    {
        try
        {
            bool isSuccess = false;

            var includes = new List<Expression<Func<Entities.Activity, object>>>();
            Expression<Func<Entities.Activity, bool>> filter = a => (true);

            var result = await dataStore.Activity.FindActivitiesAsync(filter, string.Empty, int.MaxValue, 0, includes);

            if (!result.Succeeded || result.Result == null)
            {
                return AppResult<bool>.CreateFailed(result.Error.Exception, result.Message);
            }

            if (result != null)
            {
                var activities = result.Result.ToList();

                foreach (var item in activities)
                {
                    item.Guid = Guid.NewGuid().ToString();
                }

                await dataStore.Activity.UpdateRange(activities);

                isSuccess = true;
            }

            return AppResult<bool>.CreateSucceeded(isSuccess, "Successfully updated activities");
        }
        catch (Exception ex)
        {
            return AppResult<bool>.CreateFailed(ex, "An error occured in updating activities");
        }
    }

    public async Task<AppResult<bool>> RemoveActivityAsync(int activityId)
    {
        bool isSuccess = false;

        var activityRes = await dataStore.Activity.GetByIdAsync(activityId);
        if (!activityRes.Succeeded || activityRes.Result == null)
        {
            return AppResult<bool>.CreateFailed(activityRes.Error.Exception, activityRes.Message);
        }

        var activity = activityRes.Result;

        var removeResult = await dataStore.Activity.Remove(activity);
        if (!removeResult.Succeeded)
        {
            return AppResult<bool>.CreateFailed(removeResult.Error.Exception, removeResult.Message);
        }

        isSuccess = true;

        return AppResult<bool>.CreateSucceeded(isSuccess, "Successfully removed activity");
    }
}