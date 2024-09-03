using AutoMapper;
using Cinnamon.Api.Data.Extensions;
using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Repository.Interfaces;
using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.Activity;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.OteSchedule;
using Cinnamon.Framework.Common;
using Cinnamon.Framework.Enums;
using System.Linq.Expressions;
using static Cinnamon.Framework.Enums.Enums;
using Entities = Cinnamon.Api.Data.Repository.Entities;

namespace Cinnamon.Api.Data.Services.Repository.Activity;

public class ActivityRepository : IActivityRepository
{
    private readonly IDataStore dataStore;
    private readonly IMapper mapper;

    public ActivityRepository(IDataStore dataStore, IMapper mapper)
    {
        this.dataStore = dataStore;
        this.mapper = mapper;
    }

    public async Task<AppResult<ActivityDTO>> CreateActivityAsync(int experienceTypeId, int customerId, string title, string description, string price,
        string scheduleIndicator, string remarks, bool isPublished, string address1, string address2, string district, string city, string subdivision, string region, string barangay, string postalcode,
        string specificsYouWillProvide, string customerBringWithThem, string? additionalRequirements, string activityLevel, string skillLevel,
        int minimumAge, bool canAdultsJoin, string? searchtag1, string? searchtag2, string? searchtag3, string? searchtag4, string? searchtag5,
        int experienceCategoryId, int subCategoryId, string handler, string pinnedLocation, ActivityStatus status, Enums.ExperienceCreationType experienceCreationType, 
        string? classPolicies, string? videoLink)
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
                Guid = Guid.NewGuid().ToString(),
                Status = (int)status,
                ExperienceCreationTypeId = (int)experienceCreationType,
                VideoLink = videoLink
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
                ClassPolicies = classPolicies,
            };
            var createdActivityDescription = await dataStore.ActivityDescription.Add(activityDescription);
            if (!createdActivityDescription.Succeeded)
            {
                return AppResult<ActivityDTO>.CreateFailed(createdActivityDescription.Error.Exception, createdActivityDescription.Message);
            }

            var regionResult = new AppResult<Region>();
            var cityResult = new AppResult<City>();
            var barangayResult = new AppResult<Barangay>();

            if (experienceTypeId == 1 && !string.IsNullOrEmpty(region))
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
                IsComingSoon = createdActitivityRes.Result.IsComingSoon,
                ClassPolicies = classPolicies,
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
                IsComingSoon = activity.IsComingSoon
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
        bool includeSubCategories = false, bool includeStudents = false, bool includeReviews = false, bool includeTickets = false,
        bool? forceDisable = false)
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
            if (includeTickets) includes.Add(a => a.Tickets);

            Expression<Func<Entities.Activity, bool>> filter =
                a => (ids != null ? ids.Contains(a.Id) : true) &&
                        (isActive.HasValue ? a.IsPublished == isActive.Value : true) &&
                        (forceDisable.HasValue ? a.ForceDisable == forceDisable.Value : true) &&
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
                    Id                     = a.Id,
                    SubTitle               = a.Subtitle,
                    Title                  = a.Title,
                    Description            = a.Description,
                    Price                  = a.Price,
                    Remarks                = a.Remarks,
                    IsPublished            = a.IsPublished,
                    ExperienceCategoryId   = a.ExperienceCategoryId ?? 0,
                    SubCategoryId          = a.SubCategoryId ?? 0,
                    CreatedBy              = a.CreatedBy,
                    CreatedOn              = a.CreatedOn,
                    ExperienceTypeId       = a.ExperienceTypeId,
                    Handler                = a.Handler,
                    ExperienceType         = a.ExperienceType?.Name,
                    ExperienceCategory     = a.ExperienceCategory?.Category,
                    SubCategory            = a.SubCategory?.SubCatergory,
                    IsNew                  = (DateTime.UtcNow - a.CreatedOn).Days <= 30,
                    IsDeactivated          = a.IsDeactivated,
                    Status                 = (Enums.ActivityStatus)a.Status,
                    ExperienceCreationType = (Enums.ExperienceCreationType)a.ExperienceCreationTypeId,
                    IsComingSoon           = a.IsComingSoon,
                    ForceDisable           = a.ForceDisable
                };

                // address fields
                if (includeAddres && a.Address != null)
                {
                    activityDTO.Address1       = a.Address.Address1;
                    activityDTO.Address2       = a.Address.Address2;
                    activityDTO.City           = a.Address.City;
                    activityDTO.District       = a.Address.District;
                    activityDTO.Subdivision    = a.Address.Subdivision;
                    activityDTO.Region         = a.Address.Region;
                    activityDTO.Barangay       = a.Address.Barangay;
                    activityDTO.PostalCode     = a.Address.PostalCode;
                    activityDTO.CityName       = a.Address.CityName;
                    activityDTO.RegionName     = a.Address.RegionName;
                    activityDTO.BarangayName   = a.Address.BarangayName;
                    activityDTO.PinnedLocation = a.Address.PinnedLocation;
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
                            IsActiveSchedule = s.IsActiveSchedule,
                            IsSetSession = s.IsSetSession,
                            SessionName = s.SessionName,
                            HasExpiration = s.HasExpiration,
                            StartDate = s.StartDate,
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
                    activityDTO.CompletedStudents = students.Count(a => (a.SessionsAttended >= a.NumberOfSessions && activityDTO.Schedules.LastOrDefault(s => s.Id == a.ScheduleId)?.HasExpiration == 0)
                                                                  ||(activityDTO.Schedules.LastOrDefault(s => s.Id == a.ScheduleId)?.HasExpiration == 1 && a.ExpirationDateEnd < DateTime.Now.Date && a.ExpirationDateStart != DateTime.MinValue)
                                                                  ||(activityDTO.Schedules.LastOrDefault(s => s.Id == a.ScheduleId)?.HasExpiration == 2 && a.ExpirationDateEnd < DateTime.Now.Date && a.ExpirationDateStart != DateTime.MinValue)
                                                                  ||(activityDTO.Schedules.LastOrDefault(s => s.Id == a.ScheduleId)?.HasExpiration == 1 && a.SessionsAttended >= a.NumberOfSessions)
                                                                  ||(activityDTO.Schedules.LastOrDefault(s => s.Id == a.ScheduleId)?.HasExpiration == 2 && a.SessionsAttended >= a.NumberOfSessions)
                                                                  && a.ExpirationDateEnd != DateTime.MinValue);
                    activityDTO.OngoingStudents = students.Count(a => a.SessionsAttended < a.NumberOfSessions && (a.ExpirationDateEnd >= DateTime.Now.Date || a.ExpirationDateEnd == DateTime.MinValue));
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
                if (includeTickets && a.Tickets != null)
                {
                    var tickets = a.Tickets;
                    activityDTO.NumberOfTickets = tickets.Count;
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
                    Id                     = a.Id,
                    SubTitle               = a.Subtitle,
                    Title                  = a.Title,
                    Description            = a.Description,
                    Price                  = a.Price,
                    Remarks                = a.Remarks,
                    IsPublished            = a.IsPublished,
                    ExperienceCategoryId   = a.ExperienceCategoryId ?? 0,
                    SubCategoryId          = a.SubCategoryId ?? 0,
                    CreatedBy              = a.CreatedBy,
                    ExperienceTypeId       = a.ExperienceTypeId,
                    MapDetails             = a.MapDetails,
                    Handler                = a.Handler,
                    IsComingSoon           = a.IsComingSoon,
                    ExperienceCreationType = (Enums.ExperienceCreationType)a.ExperienceCreationTypeId
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
        bool? includeSchedules = false, bool? includeImages = false, bool? isActive = false, bool? includeCustomer = false, bool includeStudents = false,bool includeTickets = false, bool? includeAddOns = false)
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
            if(includeStudents) includes.Add(a => a.Students);
            if(includeTickets) includes.Add(a => a.Tickets);
            if(includeAddOns.HasValue && includeAddOns.Value) includes.Add(a => a.AddOns);

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
                Status = (Enums.ActivityStatus)activity.Status,
                ExperienceCreationType = (Enums.ExperienceCreationType)activity.ExperienceCreationTypeId,
                IsComingSoon = activity.IsComingSoon,
                VideoLink = activity.VideoLink,
                ForceDisable = activity.ForceDisable
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
                activityDTO.ClassPolicies = description.ClassPolicies;
            }

            // schedules
            if(includeSchedules.HasValue && includeSchedules.Value && activity.Schedules != null)
            {
                activityDTO.Schedules = activity.Schedules.Select(s => {
                    return new Framework.ApiCommand.ApiData.DTO.ActivitySchedule.ActivityScheduleDTO {
                        DateTime         = s.DateTime,
                        Id               = s.Id,
                        Name             = s.Name,
                        PerUnit1         = s.PerUnit1,
                        Price            = s.Price,
                        PriceUnit1       = s.PriceUnit1,
                        PriceUnit2       = s.PriceUnit2,
                        UnitPrice        = s.UnitPrice,
                        PerUnit2         = s.PerUnit2,
                        Order            = s.Order,
                        IsActiveSchedule = s.IsActiveSchedule,
                        IsSetSession     = s.IsSetSession,
                        SessionName      = s.SessionName,
                        HasExpiration    = s.HasExpiration,
                        StartDate        = s.StartDate,
                        PriceType        = (Enums.PriceType)s.PriceType,
                        ScheduleType     = (Enums.ScheduleType)s.ScheduleType,
                        SchedulingUrl    = s.SchedulingUrl,
                    };
                }).ToList();

                if ((Enums.ExperienceCreationType)activity.ExperienceCreationTypeId == Enums.ExperienceCreationType.ExperienceViaAppointment)
                {
                    if (activityDTO.Schedules.Count > 0)
                    {
                        foreach (var schedule in activityDTO.Schedules)
                        {
                            var scheduleTimeResult = await dataStore.ActivityScheduleTime.FindAsync(a => a.ActivityScheduleId == schedule.Id);
                            if (!scheduleTimeResult.Succeeded || scheduleTimeResult.Result == null)
                            {
                                return AppResult<ActivityDTO>.CreateFailed(scheduleTimeResult.Error.Exception, scheduleTimeResult.Message);
                            }

                            schedule.ActivityScheduleTimes = scheduleTimeResult.Result.Select(s => new Framework.ApiCommand.ApiData.DTO.ActivitySchedule.ActivityScheduleTimeModelDTO
                            {
                                ActivityScheduleId = s.ActivityScheduleId,
                                ActivityScheduleTimeId = s.Id,
                                DayOfWeek = s.DayOfWeek,
                                EndTime = s.EndTime,
                                StartTime = s.StartTime,
                                IsEnabled = s.IsEnabled
                            }).ToList();
                        }
                    }
                }
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
            // student
            if (includeStudents && activity.Students != null)
            {
                var students = activity.Students;
                activityDTO.CompletedStudents = students.Count(a => (a.SessionsAttended >= a.NumberOfSessions && activityDTO.Schedules.LastOrDefault(s => s.Id == a.ScheduleId)?.HasExpiration == 0)
                                                              || (activityDTO.Schedules.LastOrDefault(s => s.Id == a.ScheduleId)?.HasExpiration == 1 && a.ExpirationDateEnd < DateTime.Now.Date && a.ExpirationDateStart != DateTime.MinValue)
                                                              || (activityDTO.Schedules.LastOrDefault(s => s.Id == a.ScheduleId)?.HasExpiration == 2 && a.ExpirationDateEnd < DateTime.Now.Date && a.ExpirationDateStart != DateTime.MinValue)
                                                              || (activityDTO.Schedules.LastOrDefault(s => s.Id == a.ScheduleId)?.HasExpiration == 1 && a.SessionsAttended >= a.NumberOfSessions)
                                                              || (activityDTO.Schedules.LastOrDefault(s => s.Id == a.ScheduleId)?.HasExpiration == 2 && a.SessionsAttended >= a.NumberOfSessions)
                                                              && a.ExpirationDateEnd != DateTime.MinValue);
                activityDTO.OngoingStudents = students.Count(a => a.SessionsAttended < a.NumberOfSessions && (a.ExpirationDateEnd >= DateTime.Now.Date || a.ExpirationDateEnd == DateTime.MinValue));
            }
            //Tickets
            if (includeTickets && activity.Tickets != null)
            {
                var tickets = activity.Tickets;
                activityDTO.NumberOfTickets = tickets.Count;
            }
            if (includeAddOns.HasValue && includeAddOns.Value && activity.AddOns != null)
            {
                var addOns = activity.AddOns;
                activityDTO.AddOns = activity.AddOns.Select(s =>
                {
                    return new Framework.ApiCommand.ApiData.DTO.AddOns.AddOnsDTO
                    {
                        Id          = s.Id,
                        ActivityId  = s.ActivityId,
                        Name        = s.Name,
                        Price       = s.Price,
                        UnitPrice   = s.UnitPrice,
                        Description = s.Description,
                        Order       = s.Order
                    };
                }).ToList();
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
        bool? includeSchedules = false, bool? includeImages = false, bool? isActive = false, bool? includeCustomer = false, bool includeStudents = false, bool? includeAddOns = false)
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
            if (includeAddOns.HasValue && includeAddOns.Value) includes.Add(a => a.AddOns);

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
                ExperienceCreationType = (Enums.ExperienceCreationType)activity.ExperienceCreationTypeId,
                IsComingSoon = activity.IsComingSoon,
                VideoLink = activity.VideoLink 
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
                activityDTO.ClassPolicies = description.ClassPolicies;
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
                        IsActiveSchedule = s.IsActiveSchedule,
                        IsSetSession = s.IsSetSession,
                        SessionName = s.SessionName,
                        HasExpiration = s.HasExpiration,
                        StartDate = s.StartDate,
                        SchedulingUrl = s.SchedulingUrl,
                        ScheduleType = (Enums.ScheduleType)s.ScheduleType
                    };
                }).ToList();

                if ((Enums.ExperienceCreationType)activity.ExperienceCreationTypeId == Enums.ExperienceCreationType.ExperienceViaAppointment)
                {
                    if (activityDTO.Schedules.Count > 0)
                    {
                        foreach (var schedule in activityDTO.Schedules)
                        {
                            var scheduleTimeResult = await dataStore.ActivityScheduleTime.FindAsync(a => a.ActivityScheduleId == schedule.Id);
                            if (!scheduleTimeResult.Succeeded || scheduleTimeResult.Result == null)
                            {
                                return AppResult<ActivityDTO>.CreateFailed(scheduleTimeResult.Error.Exception, scheduleTimeResult.Message);
                            }

                            schedule.ActivityScheduleTimes = scheduleTimeResult.Result.Select(s => new Framework.ApiCommand.ApiData.DTO.ActivitySchedule.ActivityScheduleTimeModelDTO
                            {
                                ActivityScheduleId = s.ActivityScheduleId,
                                ActivityScheduleTimeId = s.Id,
                                DayOfWeek = s.DayOfWeek,
                                EndTime = s.EndTime,
                                StartTime = s.StartTime,
                                IsEnabled = s.IsEnabled
                            }).ToList();
                        }
                    }
                }
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

            // ongoing and completed students count
            if (includeStudents)
            {
                int ongoingStudentsCount = 0;
                int completedStudentsCount = 0;

                var ongoingStudentsRes = await dataStore.Student.OngoingStudentCount(activity.Id);
                if(ongoingStudentsRes.Succeeded)
                {
                    ongoingStudentsCount += ongoingStudentsRes.Result;
                }

                var completedStudentRes = await dataStore.Student.CompletedStudentCount(activity.Id);
                if(completedStudentRes.Succeeded)
                {
                    completedStudentsCount += completedStudentRes.Result;
                }

                var directStudentOngoingCountRes = await dataStore.DirectStudentSession.OngoingStudentCount(activity.Id);
                if(directStudentOngoingCountRes.Succeeded)
                {
                    ongoingStudentsCount += directStudentOngoingCountRes.Result;
                }

                var directStudentCompletedCountRes = await dataStore.DirectStudentSession.CompletedStudentCount(activity.Id);
                if(directStudentCompletedCountRes.Succeeded)
                {
                    completedStudentsCount += directStudentCompletedCountRes.Result;
                }

                activityDTO.CompletedStudents = completedStudentsCount;
                activityDTO.OngoingStudents = ongoingStudentsCount;
            }

            if (includeAddOns.HasValue && includeAddOns.Value && activity.AddOns != null)
            {
                var addOns = activity.AddOns;
                activityDTO.AddOns = activity.AddOns.Select(s =>
                {
                    return new Framework.ApiCommand.ApiData.DTO.AddOns.AddOnsDTO
                    {
                        Id = s.Id,
                        ActivityId = s.ActivityId,
                        Name = s.Name,
                        Price = s.Price,
                        UnitPrice = s.UnitPrice,
                        Description = s.Description,
                        Order = s.Order
                    };
                }).ToList();
            }

            return AppResult<ActivityDTO>.CreateSucceeded(activityDTO, "Successfully getting activity by id");
        }
        catch (Exception ex)
        {
            return AppResult<ActivityDTO>.CreateFailed(ex, "An error occured when getting activity by id");
        }
    }

    public async Task<AppResult<ActivityDTO>> UpdateActivityAsync(int activityId, int? experienceTypeId, string? title, string? description, string? price,
        string? scheduleIndicator, string? remarks, bool? isPublished, string? address1, string? address2, string? district, string? city, string? subdivision, string? region,
        string? barangay, string? postalcode, string? specificsYouWillProvide, string? customerBringWithThem, string? additionalRequirements, string? activityLevel,
        string? skillLevel, int? minimumAge, bool? canAdultsJoin, string? searchtag1, string? searhtag2, string? searchtag3, string? searchtag4,
        string? searchtag5, int? experienceCategoryId, int? subCategoryId, string pinnedLocation,
        bool? isDeactivated, Enums.ActivityStatus? status, string? handler, string? classPolicies, string? videoLink)
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
            activity.IsDeactivated = isDeactivated ?? activity.IsDeactivated;
            activity.Status = status.HasValue ? (int)status.GetValueOrDefault() : activity.Status;
            activity.Handler = handler ?? activity.Handler;
            activity.VideoLink = videoLink ?? activity.VideoLink;

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

            if (experienceTypeId == 1 && !string.IsNullOrEmpty(region))
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
            else
            {
                activityAddress.CityName     = string.Empty;
                activityAddress.RegionName   = string.Empty;
                activityAddress.BarangayName = string.Empty;
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
            activityDescription.ClassPolicies = classPolicies ?? activityDescription.ClassPolicies;

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
                ClassPolicies = activityDescription.ClassPolicies
            }, "Successfully updated activity details");

        }
        catch (Exception ex)
        {
            return AppResult<ActivityDTO>.CreateFailed(ex, "An error occured when updating activity");
        }
    }

    public async Task<AppResult<IEnumerable<ActivityDTO>>> GetPopularActivitiesAsync(int? customerId, bool? isActive, int? count, int? skip, bool? isDeactivated, bool includeAddres = false, bool includeDescription = false, bool includeSearchTags = false, bool includeSchedules = false, bool includeImages = false, IEnumerable<int>? ids = null, bool includeCustomer = false, bool includeExperienceTypes = false, bool includeExperienceCategories = false, bool includeSubCategories = false, 
        bool includeStudents = false, bool includeReviews = false, bool includeTickets = false)
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
            if (includeTickets) includes.Add(a => a.Tickets);

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
                    Id                     = a.Id,
                    SubTitle               = a.Subtitle,
                    Title                  = a.Title,
                    Description            = a.Description,
                    Price                  = a.Price,
                    Remarks                = a.Remarks,
                    IsPublished            = a.IsPublished,
                    ExperienceCategoryId   = a.ExperienceCategoryId ?? 0,
                    SubCategoryId          = a.SubCategoryId ?? 0,
                    CreatedBy              = a.CreatedBy,
                    ExperienceTypeId       = a.ExperienceTypeId,
                    Handler                = a.Handler,
                    ExperienceType         = a.ExperienceType?.Name,
                    ExperienceCategory     = a.ExperienceCategory?.Category,
                    SubCategory            = a.SubCategory?.SubCatergory,
                    IsNew                  = (DateTime.UtcNow - a.CreatedOn).Days <= 30,
                    ExperienceCreationType = (Enums.ExperienceCreationType)a.ExperienceCreationTypeId,
                    IsComingSoon           = a.IsComingSoon
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
                    activityDTO.PinnedLocation = a.Address.PinnedLocation;
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
                            IsActiveSchedule = s.IsActiveSchedule,
                            IsSetSession = s.IsSetSession,
                            SessionName = s.SessionName,
                            HasExpiration = s.HasExpiration,
                            StartDate = s.StartDate
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
                    activityDTO.CompletedStudents = students.Count(a => (a.SessionsAttended >= a.NumberOfSessions && activityDTO.Schedules.LastOrDefault(s => s.Id == a.ScheduleId)?.HasExpiration == 0)
                                                                  || (activityDTO.Schedules.LastOrDefault(s => s.Id == a.ScheduleId)?.HasExpiration == 1 && a.ExpirationDateEnd < DateTime.Now.Date && a.ExpirationDateStart != DateTime.MinValue)
                                                                  || (activityDTO.Schedules.LastOrDefault(s => s.Id == a.ScheduleId)?.HasExpiration == 2 && a.ExpirationDateEnd < DateTime.Now.Date && a.ExpirationDateStart != DateTime.MinValue)
                                                                  || (activityDTO.Schedules.LastOrDefault(s => s.Id == a.ScheduleId)?.HasExpiration == 1 && a.SessionsAttended >= a.NumberOfSessions)
                                                                  || (activityDTO.Schedules.LastOrDefault(s => s.Id == a.ScheduleId)?.HasExpiration == 2 && a.SessionsAttended >= a.NumberOfSessions)
                                                                  && a.ExpirationDateEnd != DateTime.MinValue);
                    activityDTO.OngoingStudents = students.Count(a => a.SessionsAttended < a.NumberOfSessions && (a.ExpirationDateEnd >= DateTime.Now.Date || a.ExpirationDateEnd == DateTime.MinValue));
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
                if (includeTickets && a.Tickets != null)
                {
                    var tickets = a.Tickets;
                    activityDTO.NumberOfTickets = tickets.Count;
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

    public async Task<AppResult<IEnumerable<ActivityDTO>>> GetRecommendedActivities(int primaryActivityId, int count)
    {
        try
        {
            var result = await dataStore.Activity.GetRecommendedActivities(primaryActivityId, count);
            if(!result.Succeeded || result.Result == null)
            {
                return AppResult<IEnumerable<ActivityDTO>>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }

            return AppResult<IEnumerable<ActivityDTO>>.CreateSucceeded(result.Result.Select(s =>
            {
                return new ActivityDTO
                {
                    Description          = s.Description,
                    ExperienceCategoryId = s.ExperienceCategoryId ?? 0,
                    ExperienceTypeId     = s.ExperienceTypeId,
                    Handler              = s.Handler,
                    Id                   = s.Id,
                    Address1             = s.Address.Address1,
                    Address2             = s.Address.Address2,
                    City                 = s.Address.City,
                    CityName             = s.Address.CityName,
                    District             = s.Address.District,
                    Subdivision          = s.Address.Subdivision,
                    Region               = s.Address.Region,
                    RegionName           = s.Address.RegionName,
                    Images = s.Images.Select(i => new Framework.ApiCommand.ApiData.DTO.ActivityImage.ActivityImageDTO
                    {
                        ActivityId    = i.ActivityId,
                        Id            = i.Id,
                        ImageLocation = i.ImageLocation,
                        ImageName     = i.ImageName,
                        Order         = i.Order
                    }).ToList(),
                    IsDeactivated = s.IsDeactivated,
                    IsNew         = s.IsNew,
                    IsPublished   = s.IsPublished,
                    Price         = s.Price,
                    Remarks       = s.Remarks,
                    Status        = s.Status == 0 ? ActivityStatus.InProgress : ActivityStatus.Submitted,
                    SubCategoryId = s.SubCategoryId ?? 0,
                    SubTitle      = s.Subtitle,
                    Title         = s.Title,
                    IsComingSoon  = s.IsComingSoon,
                    Schedules = s.Schedules.Select(i => new Framework.ApiCommand.ApiData.DTO.ActivitySchedule.ActivityScheduleDTO
                    {
                        DateTime         = i.DateTime,
                        Id               = i.Id,
                        Name             = i.Name,
                        PerUnit1         = i.PerUnit1,
                        Price            = i.Price,
                        PriceUnit1       = i.PriceUnit1,
                        PriceUnit2       = i.PriceUnit2,
                        UnitPrice        = i.UnitPrice,
                        PerUnit2         = i.PerUnit2,
                        Order            = i.Order,
                        IsActiveSchedule = i.IsActiveSchedule,
                        IsSetSession     = i.IsSetSession,
                        SessionName      = i.SessionName,
                        HasExpiration    = i.HasExpiration,
                        StartDate        = i.StartDate,
                    }).ToList(),
                };
            }), "Successfully get recommended activities");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<ActivityDTO>>.CreateFailed(ex, "An error occured when getting recommended activities");
        }
    }

    public async Task<AppResult<IEnumerable<ActivityFeedDTO>>> PopularActivities(int? take, int? skip, int? categoryId)
    {
        try
        {
            var result = await dataStore.Activity.PopularActivities(take, skip, categoryId);
            if(!result.Succeeded || result.Result is null)
            {
                return AppResult<IEnumerable<ActivityFeedDTO>>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }

            return AppResult<IEnumerable<ActivityFeedDTO>>.CreateSucceeded(result.Result, "Successfully get popular activities");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<ActivityFeedDTO>>.CreateFailed(ex, "An error occured when getting popular activities");
        }
    }

    public async Task<AppResult<ActivityDTO>> CreateOteActivity(string eventName, string description, int experienceTypeId, int customerId, string stringPrice,
        string? houseNo, string? cityNumber, string? cityName, string? regionCode, string? regionName, string? barangayCode, string? barangayName,
        string? postalCode, string? pinnedLocation, DateTime scheduleFrom, DateTime scheduleTo, string recurrence, IList<OteSchedulePricingDTO> pricingDTOs,
        bool isPublished, string handler, int experienceCreationTypeId, bool comingSoon, string scheduleExtraOpt, DateTime recurrenceDateEnd, DateTime recurrenceDateStart, 
        int repeatEvery, string selectedDays, IList<OteScheduleDateDTO> oteDates,int eventDurationCount, string eventDurationTimeUnit, 
        int eventTicketLimit, bool IsOpen, bool isCapacity, int capacityCount, IList<OteDateOverrideDTO>? dateOverrides, 
        IList<OteOnlineEventsDTO> oteOnlineEventsDTOs, int categoryId, int emailReminderDays, int emailFeedbackDays,
        bool reserveSeat, int seatPlanTemplateId, string seatPlanPayload)
    {
        try
        {
            var activity = new Entities.Activity {
                Description = description,
                Title = eventName,
                ExperienceTypeId = experienceTypeId,
                Price = stringPrice,
                IsPublished = isPublished,
                ExperienceCategoryId = categoryId,
                SubCategoryId = 1,
                Handler = handler,
                IsNew = true,
                IsDeactivated = false,
                Guid = Guid.NewGuid().ToString(),
                Status = 1,
                ExperienceCreationTypeId = experienceCreationTypeId,
                CreatedBy = customerId,
                IsComingSoon = comingSoon,
                VideoLink = string.Empty
            };

            var activityDescription = new Entities.ActivityDescription {
                Description = description
            };

            var address = new Entities.ActivityAddress {
              Address1 = houseNo ?? string.Empty,
              City = cityNumber ?? string.Empty,
              CityName = cityName ?? string.Empty,
              Barangay = barangayCode ?? string.Empty,
              BarangayName = barangayName ?? string.Empty,
              Region = regionCode ?? string.Empty,
              RegionName = regionName ?? string.Empty,
              PinnedLocation = pinnedLocation ?? string.Empty,
              PostalCode = postalCode ?? string.Empty,
            };

            var schedule = new Entities.OteSchedule {
                From                  = scheduleFrom.SetKindUtc(),
                To                    = scheduleTo.SetKindUtc(),
                Recurrences           = recurrence,
                ExtraOptions          = scheduleExtraOpt ?? String.Empty,
                RecurrenceDateEnd     = recurrenceDateEnd.SetKindUtc(),
                RecurrenceDateStart   = recurrenceDateStart.SetKindUtc(),
                RepeatEvery           = repeatEvery,
                SelectedDays          = selectedDays,
                EventDurationCount    = eventDurationCount,
                EventDurationTimeUnit = eventDurationTimeUnit,
                EventTicketLimit      = eventTicketLimit,
                IsOpen                = IsOpen,
                IsCapacity            = isCapacity,
                CapacityCount         = capacityCount,
                EmailFeedbackDays     = emailFeedbackDays,
                EmailReminderDays     = emailReminderDays,
                ReserveSeat           = reserveSeat,
                SeatPlanTemplateId    = seatPlanTemplateId,
            };

            var pricingsGroup = pricingDTOs.Select(p => {
                return new OteSchedulePricingGroup {
                    Description = p.Description,
                    IsAbsorbFees = p.IsAbsorbFees,
                    MaxSlots = p.MaxSlots,
                    Price = p.Price,
                    Name = p.Name,
                    RequiredApproval = p.RequiredApproval,
                    IsUnlimited = p.IsUnlimited,
                    OteSchedule = schedule
                };
            }).ToList();

            var onlineEvent = oteOnlineEventsDTOs is not null ? oteOnlineEventsDTOs.Select(s => {
                return new OteOnlineEvent {
                    Title                     = s.Title,
                    Description               = s.Description,
                    TicketRestriction         = s.TicketRestriction,
                    Videolink                 = s.Videolink,
                    OteSchedule               = schedule,
                };
            }).ToList(): null;

            var dates = oteDates.Select(d => {
                return new Entities.OteDate {
                    Date = d.Date.SetKindUtc(),
                    DateEnd = d.DateEnd.SetKindUtc(),
                    DateStart = d.DateStart.SetKindUtc(),
                    SeatPlanPayload = seatPlanPayload,
                    OteSchedulePricing = pricingsGroup.Select(p => {
                        return new OteSchedulePricing {
                            Description             = p.Description,
                            IsAbsorbFees            = p.IsAbsorbFees,
                            MaxSlots                = p.MaxSlots,
                            Price                   = p.Price,
                            Name                    = p.Name,
                            RequiredApproval        = p.RequiredApproval,
                            IsUnlimited             = p.IsUnlimited,
                            OteSchedule             = schedule,
                            OteSchedulePricingGroup = p
                        };
                    }).ToList(),
                    OteSchedule = schedule
                };
            }).ToList();

            // create date overrides
            List<Entities.OteDateOverride> overrides = new();
            if(dateOverrides is not null)
            {
                foreach(var item in dateOverrides)
                {
                    var oteDate = dates.FirstOrDefault(d => d.Date.Date == item.Date.Date);
                    if(oteDate is not null)
                    {
                        overrides.Add(new OteDateOverride {
                            Date = item.Date.SetKindUtc(),
                            DateStart = item.DateStart.SetKindUtc(),
                            DateEnd = item.DateEnd.SetKindUtc(),
                            OteDate = oteDate,
                        });
                    }
                }
            }

            var createRes = await this.dataStore.Activity.CreateOteActivity(activity, activityDescription, address, 
                schedule, pricingsGroup, dates, overrides, onlineEvent);
            if(!createRes.Succeeded || createRes.Result is null)
            {
                return AppResult<ActivityDTO>.CreateFailed(new ApplicationException(createRes.Message), createRes.Message);
            }

            return AppResult<ActivityDTO>.CreateSucceeded(new ActivityDTO {
                Address1 = houseNo ?? string.Empty,
                Barangay = barangayCode ?? string.Empty,
                BarangayName = barangayName ?? string.Empty,
                City = cityNumber ?? string.Empty,
                CityName = cityName ?? string.Empty,
                Description = description,
                ExperienceCategoryId = 1,
                ExperienceCreationType = Enums.ExperienceCreationType.OneTimeEvents,
                Handler = handler,
                ExperienceTypeId = experienceTypeId,
                Id = createRes.Result.Id,
                IsDeactivated = false,
                IsNew = true,
                PinnedLocation = pinnedLocation ?? string.Empty,
                PostalCode = postalCode ?? string.Empty,
                Price = stringPrice,
                Title = eventName,
                IsPublished = isPublished,
                Region = regionCode ?? string.Empty,
                RegionName = regionName ?? string.Empty,
                SubCategoryId = 1,
            }, "One time event successfully created.");
        }
        catch (Exception ex)
        {
            return AppResult<ActivityDTO>.CreateFailed(ex, "An error occured when creating One time event.");
        }
    }

    public async Task<AppResult<ActivityDTO>> UpdateOteActivity(int id, string eventName, string description, int experienceTypeId, string stringPrice,
        string houseNo, string cityNumber, string cityName, string regionCode, string regionName, string barangayCode, string barangayName,
        string postalCode, string pinnedLocation, DateTime scheduleFrom, DateTime scheduleTo, string recurrence, IList<OteSchedulePricingDTO> pricingDTOs,
        bool isPublished, string handler, int categoryId, bool comingSoon, int ticketEventLimit, bool IsOpen, bool isCapacity, int capacityCount, 
        string scheduleExtraOpt, DateTime recurrenceDateEnd, DateTime recurrenceDateStart,int repeatEvery, string selectedDays, IList<OteScheduleDateDTO> oteDates, 
        int eventDurationCount, string eventDurationTimeUnit, IList<OteDateOverrideDTO>? dateOverrides, IList<OteOnlineEventsDTO> oteOnlineEventsDTOs, 
        bool recreateSchedule, IList<OteRescheduleDTO>? oteReschedules, int emailReminderDays, int emailFeedbackDays)
    {
        try
        {
            var activity = new Entities.Activity {
                Id = id,
                Description = description,
                Title = eventName,
                ExperienceTypeId = experienceTypeId,
                Price = stringPrice,
                IsPublished = isPublished,
                ExperienceCategoryId = categoryId,
                Handler = handler,
                IsComingSoon = comingSoon,
            };

            var activityDescription = new Entities.ActivityDescription {
                Description = description
            };

            var address = new Entities.ActivityAddress {
              Address1 = houseNo,
              City = cityNumber,
              CityName = cityName,
              Barangay = barangayCode,
              BarangayName = barangayName,
              Region = regionCode,
              RegionName = regionName,
              PinnedLocation = pinnedLocation,
              PostalCode = postalCode,  
            };

            var schedule = new Entities.OteSchedule {
                From                  = scheduleFrom.SetKindUtc(),
                To                    = scheduleTo.SetKindUtc(),
                Recurrences           = recurrence,
                ExtraOptions          = scheduleExtraOpt ?? String.Empty,
                RecurrenceDateEnd     = recurrenceDateEnd.SetKindUtc(),
                RecurrenceDateStart   = recurrenceDateStart.SetKindUtc(),
                RepeatEvery           = repeatEvery,
                SelectedDays          = selectedDays,
                EventDurationCount    = eventDurationCount,
                EventDurationTimeUnit = eventDurationTimeUnit,
                EventTicketLimit      = ticketEventLimit,
                IsOpen                = IsOpen,
                IsCapacity            = isCapacity,
                CapacityCount         = capacityCount,
                EmailFeedbackDays     = emailFeedbackDays,
                EmailReminderDays     = emailReminderDays
            };

            var pricingsGroup = pricingDTOs.Select(p => {
                return new OteSchedulePricingGroup
                {
                    Id               = p.Id,
                    Description      = p.Description,
                    IsAbsorbFees     = p.IsAbsorbFees,
                    MaxSlots         = p.MaxSlots,
                    Price            = p.Price,
                    Name             = p.Name,
                    IsUnlimited      = p.IsUnlimited,
                    RequiredApproval = p.RequiredApproval,
                    OteSchedule      = schedule,
                };
            }).ToList();

            var dates = oteDates.Select(d => {
                return new Entities.OteDate
                {
                    Id = d.Id,
                    Date = d.Date.SetKindUtc(),
                    DateEnd = d.DateEnd.SetKindUtc(),
                    DateStart = d.DateStart.SetKindUtc(),
                    OteSchedulePricing = pricingsGroup.Select(p => {
                        return new OteSchedulePricing
                        {
                            Description = p.Description,
                            IsAbsorbFees = p.IsAbsorbFees,
                            MaxSlots = p.MaxSlots,
                            Price = p.Price,
                            Name = p.Name,
                            OteSchedulePricingGroup = p
                        };
                    }).ToList(),
                };
            }).ToList();

            schedule.OteSchedulePricing = pricingDTOs.Select(p => {
                return new OteSchedulePricing {
                    Id = p.Id,
                    Description = p.Description,
                    IsAbsorbFees = p.IsAbsorbFees,
                    MaxSlots = p.MaxSlots,
                    Price = p.Price,
                    Name = p.Name,
                    RequiredApproval = p.RequiredApproval,
                    IsUnlimited = p.IsUnlimited
                };
            }).ToList();

            schedule.OteOnlineEvent = oteOnlineEventsDTOs is not null ? oteOnlineEventsDTOs.Select(s =>{
                return new OteOnlineEvent {
                    Id                        = s.Id,
                    Title                     = s.Title,
                    Description               = s.Description,
                    TicketRestriction         = s.TicketRestriction,
                    Videolink                 = s.Videolink,
                };
            }).ToList() : null;

            var updatedRes = await this.dataStore.Activity.UpdateOteActivity(activity, activityDescription, 
                address, schedule, dates, pricingsGroup, recreateSchedule, oteReschedules);
            if(!updatedRes.Succeeded || updatedRes.Result is null)
            {
                return AppResult<ActivityDTO>.CreateFailed(new ApplicationException(updatedRes.Message), updatedRes.Message);
            }

            return AppResult<ActivityDTO>.CreateSucceeded(new ActivityDTO {
                Address1 = houseNo,
                Barangay = barangayCode,
                BarangayName = barangayName,
                City = cityNumber,
                CityName = cityName,
                Description = description,
                ExperienceCategoryId = categoryId,
                ExperienceCreationType = Enums.ExperienceCreationType.OneTimeEvents,
                Handler = handler,
                ExperienceTypeId = experienceTypeId,
                Id = id,
                PinnedLocation = pinnedLocation,
                PostalCode = postalCode,
                Price = stringPrice,
                Title = eventName,
                IsPublished = isPublished,
                Region = regionCode,
                RegionName = regionName,
            }, "One time event successfully updated.");
        }
        catch (Exception ex)
        {
            return AppResult<ActivityDTO>.CreateFailed(ex, "An error occured when updating One time event.");
        }
    }
    
    public async Task<AppResult<OteActivityDTO>> FindOteByHandler(string handler, bool includeDescription = false, 
        bool includeAddress = false, bool includeSchedule = false, bool includePricing = false, 
        bool includeProvider = false, bool includeImages = false, bool includeOnlineEvents = false, bool includeTickets = false)
    {
        try
        {
            var result = await dataStore.Activity.FindOteByHandler(handler, includeDescription, includeAddress, 
                includeSchedule, includePricing, includeProvider, includeImages, includeOnlineEvents, includeTickets);
            if(!result.Succeeded || result.Result is null)
            {
                return AppResult<OteActivityDTO>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }
            var model = mapper.Map<OteActivityDTO>(result.Result);
            return AppResult<OteActivityDTO>.CreateSucceeded(model, "Sucessfully find one time event");
        }
        catch (Exception ex)
        {
            return AppResult<OteActivityDTO>.CreateFailed(ex, "An error occured when getting one time event by handler.");
        }
    }
    public async Task<AppResult<IEnumerable<OteActivityDTO>>> GetOTEByProvider(int Id)
    {
        try
        {
            var result = await dataStore.Activity.GetOTEByProvider(Id);
            if (!result.Succeeded || result.Result == null)
            {
                return AppResult<IEnumerable<OteActivityDTO>>.CreateFailed(result.Error.Exception, result.Message);
            }
            var ote = result.Result.Select(s =>
            {
                var oteDTO = new OteActivityDTO
                {
                    Id               = s.Id,    
                    ExperienceTypeId = s.ExperienceTypeId,
                    EventName        = s.Title,
                    Description      = s.Description,
                    Handler          = s.Handler,
                    CityName         = s.CityName,
                    RegionName       = s.RegionName,
                    PinnedLocation   = s.PinnedLocation,
                    EventImage       = s.OteSchedule.EventImage,
                    ScheduleFrom     = s.OteSchedule.ScheduleFrom,
                    ScheduleTo       = s.OteSchedule.ScheduleTo,
                    Slots            = s.OteSchedule.Slots,
                    Sold             = s.OteSchedule.Sold,
                    Available        = s.OteSchedule.Slots - s.OteSchedule.Sold,
                };
                return oteDTO;
            });
            return AppResult<IEnumerable<OteActivityDTO>>.CreateSucceeded(ote, "Successfully get ote activities");

        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<OteActivityDTO>>.CreateFailed(ex, "An error occured when getting ote activities");
        }
    }

    public async Task<AppResult<IEnumerable<OteSchedulePricingDTO>>> AddTicketSold(IEnumerable<OteSchedulePricingDTO> tickets)
    {
        try
        {
            var ids = tickets.Select(t => t.Id);
            var ticketPricingsRes = await dataStore.OteSchedulePricing.FindAsync(t => ids.Contains(t.Id));
            if(!ticketPricingsRes.Succeeded || ticketPricingsRes.Result is null)
            {
                return AppResult<IEnumerable<OteSchedulePricingDTO>>.CreateFailed(new ApplicationException(ticketPricingsRes.Message), ticketPricingsRes.Message);
            }
            var ticketPricings = ticketPricingsRes.Result;

            // update only ticket sold field
            foreach(var item in tickets)
            {
                var ticketPrice = ticketPricings.FirstOrDefault(t => t.Id == item.Id);
                if(ticketPrice is not null)
                {
                    ticketPrice.TicketSold += item.TicketSold;
                }
            }

            var updatedRes = await dataStore.OteSchedulePricing.UpdateRange(ticketPricings);
            if(!updatedRes.Succeeded || updatedRes.Result is null)
            {
                return AppResult<IEnumerable<OteSchedulePricingDTO>>.CreateFailed(new ApplicationException(updatedRes.Message), updatedRes.Message);
            }

            var updated = mapper.Map<IEnumerable<OteSchedulePricingDTO>>(updatedRes.Result);
            return AppResult<IEnumerable<OteSchedulePricingDTO>>.CreateSucceeded(updated, "Ote ticket pricing successfully updated");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<OteSchedulePricingDTO>>.CreateFailed(ex, "An error occured when updating ticket sold.");
        }
    }

    public async Task<AppResult<IEnumerable<OteOngoingDTO>>> CustomerOte(int customerId)
    {
        try
        {
            var result = await dataStore.Activity.CustomerOte(customerId);
            if(!result.Succeeded || result.Result is null)
            {
                return AppResult<IEnumerable<OteOngoingDTO>>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }
            
            return AppResult<IEnumerable<OteOngoingDTO>>.CreateSucceeded(result.Result, "Sucessfully find customer ote");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<OteOngoingDTO>>.CreateFailed(ex, "An error occured when getting customet ote.");
        }
    }

    public async Task<AppResult<IEnumerable<OteActivityPerDateDTO>>> OtePerDate(int? providerId)
    {
        try
        {
            var result = await dataStore.Activity.OtePerDate(providerId);
            if(!result.Succeeded || result.Result is null)
            {
                return AppResult<IEnumerable<OteActivityPerDateDTO>>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }

            return AppResult<IEnumerable<OteActivityPerDateDTO>>.CreateSucceeded(result.Result, "Successfully find customers ote per date");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<OteActivityPerDateDTO>>.CreateFailed(ex, "An error occured when getting customer ote.");
        }
    }

    public async Task<AppResult<IEnumerable<ActivityDTO>>> ExpiredEvents()
    {
        try
        {
            var result = await dataStore.Activity.GetActivitiesNeedToDisable();
            if(!result.Succeeded || result.Result is null)
            {
                return AppResult<IEnumerable<ActivityDTO>>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }

            return AppResult<IEnumerable<ActivityDTO>>.CreateSucceeded(result.Result, "Successfully get expired events.");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<ActivityDTO>>.CreateFailed(ex, "An error occured when getting expired events.");
        }
    }

    public async Task<AppResult<IEnumerable<ActivityDTO>>> ForceDisableActivities(IList<int> activityIds)
    {
        try
        {
            var result = await dataStore.Activity.ForceDisableActivities(activityIds);
            if(!result.Succeeded || result.Result is null)
            {
                return AppResult<IEnumerable<ActivityDTO>>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }

            return AppResult<IEnumerable<ActivityDTO>>.CreateSucceeded(result.Result, "Successfully get expired events.");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<ActivityDTO>>.CreateFailed(ex, "An error occured when getting expired events.");
        }
    }
    public async Task<AppResult<bool>> DeleteTicket(int Id)
    {
        try
        {
            var oteGroupTicketResult = await dataStore.OteSchedulePricingGroup.GetByIdAsync(Id);
            var oteGroupTicket = oteGroupTicketResult.Result;

            if (oteGroupTicket == null)
            {
                return AppResult<bool>.CreateFailed(new ApplicationException("No group ticket found with the provided ID"), "No group ticket found with the provided ID");
            }
            var oteTicketResult = await dataStore.OteSchedulePricing.GetPricings(oteGroupTicket.Id);
            var oteTicket = oteTicketResult.Result;


            if (oteTicket == null || !oteTicket.Any())
            {
                return AppResult<bool>.CreateFailed(new ApplicationException("No associated tickets found for the group ticket"), "No associated tickets found for the group ticket");
            }
            var associatedTicketsRemovalResult = await dataStore.OteSchedulePricing.RemoveRange(oteTicket);
            var groupTicketRemovalResult = await dataStore.OteSchedulePricingGroup.Remove(oteGroupTicket);

            if (!groupTicketRemovalResult.Succeeded || !associatedTicketsRemovalResult.Succeeded)
            {
                return AppResult<bool>.CreateFailed(new ApplicationException("Failed to delete group ticket or associated tickets"), "Failed to delete group ticket or associated tickets");
            }

            return AppResult<bool>.CreateSucceeded(true, "Successfully deleted group ticket and associated tickets");
        }
        catch (Exception ex)
        {
            return AppResult<bool>.CreateFailed(ex, "An error occurred while deleting ticket and associated tickets");
        }
    }

    public async Task<AppResult<IEnumerable<ActivityFeedDTO>>> ActivityFeed(int take, int skip, string? search = null, 
        int? categoryId = null, int? starReview = null, int? experienceType = null, int? experienceCategory = null)
    {
        try
        {
            var result = await dataStore.Activity.ActivityFeed(take, skip, search, categoryId, starReview, experienceType, experienceCategory);
            if(!result.Succeeded || result.Result is null)
            {
                return AppResult<IEnumerable<ActivityFeedDTO>>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }

            return AppResult<IEnumerable<ActivityFeedDTO>>.CreateSucceeded(result.Result, "Successfully get activity feed.");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<ActivityFeedDTO>>.CreateFailed(ex, "An error occured when getting activity feed.");
        }
    }

    public async Task<AppResult<bool>> BatchSummaryUpdate()
    {
        try
        {
            var result = await dataStore.Activity.BatchSummaryUpdate();
            if(!result.Succeeded || !result.Result)
            {
                return AppResult<bool>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }

            return AppResult<bool>.CreateSucceeded(result.Result, "Successfully update summary activities.");
        }
        catch (Exception ex)
        {
            return AppResult<bool>.CreateFailed(ex, "An error occured when updating summary by batch.");
        }
    }

    public async Task<AppResult<IEnumerable<OteAlreadyBookDate>>> OteAlreadyBooked(int activityId)
    {
        try
        {
            var result = await dataStore.Activity.OteAlreadyBookDates(activityId);
            if(!result.Succeeded || result.Result is null)
            {
                return AppResult<IEnumerable<OteAlreadyBookDate>>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }

            return AppResult<IEnumerable<OteAlreadyBookDate>>.CreateSucceeded(result.Result, "Successfully get ote already booked dates.");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<OteAlreadyBookDate>>.CreateFailed(ex, "An error occured when getting ote already booked dates.");
        }
    }
}