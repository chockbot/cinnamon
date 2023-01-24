using Cinnamon.Framework.ApiCommand.ApiData.DTO.OngoingActivity;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Data.Services.Repository.Interfaces;

public interface IOngoingActivityRepository 
{
    Task<AppResult<OngoingActivityDTO>> GetByIdAsync(int id);
    Task<AppResult<IEnumerable<OngoingActivityDTO>>> GetAllAsync(int? count, int? skip, int? customerId, bool isIncludeActivity = false);
    Task<AppResult<IEnumerable<OngoingActivityDTO>>> GetAllAsync();
    Task<AppResult<OngoingActivityDTO>> Create(int activityId, int customerId, int scheduleId, int purchaseOrderId);
    Task<AppResult<OngoingActivityDTO>> Update(int ongoingActivityId, int? activityId, int? customerId, int? purchaseOrderId);
}