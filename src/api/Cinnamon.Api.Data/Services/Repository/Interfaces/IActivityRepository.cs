using Cinnamon.Framework.Common;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.Activity;
using Cinnamon.Framework.Enums;
using static Cinnamon.Framework.Enums.Enums;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.OteSchedule;

namespace Cinnamon.Api.Data.Services.Repository.Interfaces;

public interface IActivityRepository
{
    Task<AppResult<ActivityDTO>> GetByIdAsync(int id, int? customerId = null,
        bool? includeAddres = false, bool? includeDescription = false, bool? includeSearchTags = false,
        bool? includeSchedules = false, bool? includeImages = false, bool? isActive = false, bool? includeCustomer = false, bool includeStudents = false, bool includeTickets = false, bool? includeAddOns = false);
    
    Task<AppResult<ActivityDTO>> GetByHandlerAsync(string handler, int? customerId = null,
        bool? includeAddres = false, bool? includeDescription = false, bool? includeSearchTags = false,
        bool? includeSchedules = false, bool? includeImages = false, bool? isActive = false, bool? includeCustomer = false, bool includeStudents = false, bool? includeAddOns = false);

    Task<AppResult<IEnumerable<ActivityDTO>>> GetAllAsync(int? customerId, bool? isActive, int? count, int? skip,
        int experienceCategoryId, string searchValue, bool? isDeactivated, Enums.ActivityStatus? status,
        bool includeAddres = false, bool includeDescription = false, bool includeSearchTags = false,
        bool includeSchedules = false, bool includeImages = false, IEnumerable<int>? ids = null, string? likeHandler = null,
        bool includeCustomer = false, bool includeExperienceTypes = false, bool includeExperienceCategories = false, bool includeSubCategories = false, bool includeStudents = false, bool includeReviews = false, bool includeTickets = false);

    Task<AppResult<IEnumerable<ActivityDTO>>> GetPopularActivitiesAsync(int? customerId, bool? isActive, int? count, int? skip, bool? isDeactivated,
        bool includeAddres = false, bool includeDescription = false, bool includeSearchTags = false,
        bool includeSchedules = false, bool includeImages = false, IEnumerable<int>? ids = null, bool includeCustomer = false, bool includeExperienceTypes = false, bool includeExperienceCategories = false, bool includeSubCategories = false, 
        bool includeStudents = false, bool includeReviews = false, bool includeTickets = false);
    Task<AppResult<IEnumerable<ActivityDTO>>> GetAllAsync();
    Task<AppResult<ActivityDTO>> CreateActivityAsync(int experienceTypeId, int customerId, string title, string description, string price,
        string scheduleIndicator, string remarks, bool isPublished, string address1, string address2, string district,
        string city,string subdivision, string region, string barangay,string postalcode, string specificsYouWillProvide, string customerBringWithThem, string? additionalRequirements,
        string activityLevel, string skillLevel, int minimumAge, bool canAdultsJoin, string? searchtag1, string? searhtag2,
        string? searchtag3, string? searchtag4, string? searchtag5, int experienceCategoryId, int subCategoryId, string handler, string pinnedLocation, Enums.ActivityStatus status, Enums.ExperienceCreationType experienceCreationType, string? classPolicies);
    Task<AppResult<ActivityDTO>> UpdateActivityAsync(int activityId, int? experienceTypeId, string? title, string? description, string? price,
        string? scheduleIndicator, string? remarks, bool? isPublished, string? address1, string? address2, string? district,
        string? city, string? subdivision, string? region, string? barangay, string? postalcode, string? specificsYouWillProvide, string? customerBringWithThem, string? additionalRequirements,
        string? activityLevel, string? skillLevel, int? minimumAge, bool? canAdultsJoin, string? searchtag1, string? searhtag2,
        string? searchtag3, string? searchtag4, string? searchtag5, int? experienceCategoryId, int? subCategoryId, string? pinnedLocation, bool? isDeactivated, Enums.ActivityStatus? status, string? handler, string? classPolicies);
    Task<AppResult<ActivityDTO>> GetActivitieByCategoriesAsync(int experienceCategoryId, int? customerId = null,
        bool? includeAddres = false, bool? includeDescription = false, bool? includeSearchTags = false,
        bool? includeSchedules = false, bool? includeImages = false, bool? isActive = false, bool? includeCustomer = false);

    Task<AppResult<bool>> UpdateActivityGuid();

    Task<AppResult<bool>> RemoveActivityAsync(int activityId);

    Task<AppResult<IEnumerable<ActivityDTO>>> GetRecommendedActivities(int primaryActivityId, int count);
    Task<AppResult<IEnumerable<PopularActivityDTO>>> PopularActivities(int? take, int? skip, int? categoryId);
    Task<AppResult<ActivityDTO>> CreateOteActivity(string eventName, string description, int experienceTypeId, int customerId, string stringPrice,
        string? houseNo, string? cityNumber, string? cityName, string? regionCode, string? regionName, string? barangayCode, string? barangayName,
        string? postalCode, string? pinnedLocation, DateTime scheduleFrom, DateTime scheduleTo, string recurrence, IList<OteSchedulePricingDTO> pricingDTOs,
        bool isPublished, string handler, int experienceCreationTypeId, bool comingSoon, string scheduleExtraOpt, DateTime recurrenceDateEnd, 
        DateTime recurrenceDateStart, int repeatEvery, string selectedDays, IList<OteDateDTO> oteDates);

    Task<AppResult<ActivityDTO>> UpdateOteActivity(int id, string eventName, string description, int experienceTypeId, string stringPrice,
        string houseNo, string cityNumber, string cityName, string regionCode, string regionName, string barangayCode, string barangayName,
        string postalCode, string pinnedLocation, DateTime scheduleFrom, DateTime scheduleTo, string recurrence, IList<OteSchedulePricingDTO> pricingDTOs,
        bool isPublished, string handler, int categoryId, bool comingSoon);

    Task<AppResult<OteActivityDTO>> FindOteByHandler(string handler, bool includeDescription = false, 
        bool includeAddress = false, bool includeSchedule = false, bool includePricing = false, bool includeProvider = false, bool includeImages = false);
    Task<AppResult<IEnumerable<OteActivityDTO>>> GetOTEByProvider(int Id);
    Task<AppResult<IEnumerable<OteSchedulePricingDTO>>> AddTicketSold(IEnumerable<OteSchedulePricingDTO> tickets);

    Task<AppResult<IEnumerable<OteOngoingDTO>>> CustomerOte(int customerId);
    Task<AppResult<IEnumerable<OteActivityPerDateDTO>>> OtePerDate(int? providerId);
}