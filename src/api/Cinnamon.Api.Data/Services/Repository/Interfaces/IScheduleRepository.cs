using Cinnamon.Api.Data.Services.Repository.ActivityDescription.DTO;
using Cinnamon.Api.Data.Services.Repository.Schedule.DTO;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Data.Services.Repository.Interfaces
{
    public interface IScheduleRepository
    {
        Task<AppResult<ScheduleDTO>> GetByIdAsync(int id);
        Task<AppResult<IEnumerable<ScheduleDTO>>> GetAllAsync();
        Task<AppResult<ScheduleDTO>> UpdateSchedule(int ScheduleId, string Name, string datetime, decimal Price, string UnitPrice, int PerUnit1, string PriceUnit1, int PerUnit2, string PriceUnit2);
        Task<AppResult<ScheduleDTO>> CreateSchedule(int ActivityId, string Name, string datetime, decimal Price, string UnitPrice, int PerUnit1, string PriceUnit1, int PerUnit2, string PriceUnit2);
    }
}
