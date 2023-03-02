using Cinnamon.Framework.Common;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.Activity;

namespace Cinnamon.Api.Data.Services.Repository.Interfaces;

public interface IActivityRepository
{
    Task<AppResult<ActivityDTO>> GetByIdAsync(int id, int? customerId = null,
        bool? includeAddres = false, bool? includeDescription = false, bool? includeSearchTags = false,
        bool? includeSchedules = false, bool? includeImages = false, bool? isActive = false, bool? includeCustomer = false);
    
    Task<AppResult<ActivityDTO>> GetByHandlerAsync(string handler, int? customerId = null,
        bool? includeAddres = false, bool? includeDescription = false, bool? includeSearchTags = false,
        bool? includeSchedules = false, bool? includeImages = false, bool? isActive = false, bool? includeCustomer = false);

    Task<AppResult<IEnumerable<ActivityDTO>>> GetAllAsync(int? customerId, bool? isActive, int? count, int? skip,
        int experienceCategoryId, string searchValue,
        bool includeAddres = false, bool includeDescription = false, bool includeSearchTags = false,
        bool includeSchedules = false, bool includeImages = false, IEnumerable<int>? ids = null, string? likeHandler = null,
        bool includeCustomer = false, bool includeExperienceTypes = false, bool includeExperienceCategories = false, bool includeSubCategories = false);
    Task<AppResult<IEnumerable<ActivityDTO>>> GetAllAsync();
    Task<AppResult<ActivityDTO>> CreateActivityAsync(int experienceTypeId, int customerId, string title, string description, string price,
        string scheduleIndicator, string remarks, bool isPublished, string address1, string address2, string district,
        string city,string subdivision, string region, string barangay,string postalcode, string specificsYouWillProvide, string customerBringWithThem, string? additionalRequirements,
        string activityLevel, string skillLevel, int minimumAge, bool canAdultsJoin, string? searchtag1, string? searhtag2,
        string? searchtag3, string? searchtag4, string? searchtag5, int experienceCategoryId, int subCategoryId, string handler, bool IsSetSession, string SessionName);
    Task<AppResult<ActivityDTO>> UpdateActivityAsync(int activityId, int? experienceTypeId,string? title, string? description, string? price,
        string? scheduleIndicator, string? remarks, bool? isPublished, string? address1, string? address2, string? district,
        string? city, string? subdivision, string? region, string? barangay, string? postalcode, string? specificsYouWillProvide, string? customerBringWithThem, string? additionalRequirements,
        string? activityLevel, string? skillLevel, int? minimumAge, bool? canAdultsJoin, string? searchtag1, string? searhtag2,
        string? searchtag3, string? searchtag4, string? searchtag5, int? experienceCategoryId, int? subCategoryId, bool? IsSetSession, string? SessionName);
    Task<AppResult<ActivityDTO>> GetActivitieByCategoriesAsync(int experienceCategoryId, int? customerId = null,
        bool? includeAddres = false, bool? includeDescription = false, bool? includeSearchTags = false,
        bool? includeSchedules = false, bool? includeImages = false, bool? isActive = false, bool? includeCustomer = false);
}