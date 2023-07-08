using Cinnamon.Framework.ApiCommand.ApiData.DTO.Schedule;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Data.Services.Repository.Interfaces
{
    public interface IScheduleRepository
    {
        Task<AppResult<ScheduleDTO>> GetByIdAsync(int id, bool? includeActivity = null);
        Task<AppResult<IEnumerable<ScheduleDTO>>> GetAllAsync();
        Task<AppResult<ScheduleDTO>> UpdateSchedule(int? ScheduleId, string? Name, string? datetime, decimal? Price, string? UnitPrice, int? PerUnit1, string? PriceUnit1, int? PerUnit2, string? PriceUnit2, int? order, bool? IsActiveSchedule, bool? IsSetSession, string? SessionName, int? HasExpiration, DateTime? startDate);
        Task<AppResult<ScheduleDTO>> CreateSchedule(int ActivityId, string Name, string datetime, decimal Price, string UnitPrice, int PerUnit1, string PriceUnit1, int PerUnit2, string PriceUnit2, int order, bool IsActiveSchedule, bool IsSetSession, string SessionName, int HasExpiration, DateTime? startDate);
        Task<AppResult<IEnumerable<ScheduleDTO>>> CreateSchedules(int activityId, IEnumerable<ScheduleDTO> schedules);
        Task<AppResult<IEnumerable<ScheduleDTO>>> UpdateManySchedules(IEnumerable<ScheduleDTO> schedules);
        Task<AppResult<bool>> DeleteManySchedules(IEnumerable<int> schedulesIds);
    }
}
