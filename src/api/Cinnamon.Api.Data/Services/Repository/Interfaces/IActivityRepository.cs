using Cinnamon.Framework.Common;
using Cinnamon.Api.Data.Services.Repository.Activity.DTO;

namespace Cinnamon.Api.Data.Services.Repository.Interfaces;

public interface IActivityRepository
{
    Task<AppResult<ActivityDTO>> GetByIdAsync(int id);
    Task<AppResult<IEnumerable<ActivityDTO>>> GetAllAsync(bool? isActive, int? count, int? skip);
    Task<AppResult<IEnumerable<ActivityDTO>>> GetAllAsync();
    Task<AppResult<ActivityDTO>> CreateActivityAsync(int experienceTypeId, int customerId, string title, string description, string price,
        string scheduleIndicator, string remarks, bool isPublished, string address1, string address2, string district,
        string city, string specificsYouWillProvide, string customerBringWithThem, string? additionalRequirements,
        string activityLevel, string skillLevel, int minimumAge, bool canAdultsJoin, string? searchtag1, string? searhtag2,
        string? searchtag3, string? searchtag4, string? searchtag5);
    Task<AppResult<ActivityDTO>> UpdateActivityAsync(int activityId, int? experienceTypeId,string? title, string? description, string? price,
        string? scheduleIndicator, string? remarks, bool? isPublished, string? address1, string? address2, string? district,
        string? city, string? specificsYouWillProvide, string? customerBringWithThem, string? additionalRequirements,
        string? activityLevel, string? skillLevel, int? minimumAge, bool? canAdultsJoin, string? searchtag1, string? searhtag2,
        string? searchtag3, string? searchtag4, string? searchtag5);
}