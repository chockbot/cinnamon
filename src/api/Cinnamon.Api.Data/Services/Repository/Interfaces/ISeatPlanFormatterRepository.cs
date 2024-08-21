using Cinnamon.Framework.ApiCommand.ApiData.DTO.SeatPlan;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Data.Services.Repository.Interfaces;

public interface ISeatPlanFormatterRepository 
{
    Task<AppResult<IEnumerable<SeatPlanFormatterDTO>>> GetSeatPlanFormatterAsync();
}