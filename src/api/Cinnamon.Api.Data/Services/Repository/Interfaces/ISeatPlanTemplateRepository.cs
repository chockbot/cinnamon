using Cinnamon.Framework.ApiCommand.ApiData.DTO.SeatPlan;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Data.Services.Repository.Interfaces;

public interface ISeatPlanTemplateRepository 
{
    Task<AppResult<IEnumerable<SeatPlanTemplateDTO>>> GetSeatPlanTemplateAsync(string templateName, int page, int limit);
    Task<AppResult<SeatPlanTemplateDTO>> GetSeatPlanTemplateByIdAsync(int id);
    Task<AppResult<SeatPlanTemplateDTO>> CreateSeatPlanTemplateAsync(SeatPlanTemplateDTO seatPlanTemplateDTO);
    Task<AppResult<SeatPlanTemplateDTO>> UpdateSeatPlanTemplateAsync(SeatPlanTemplateDTO seatPlanTemplateDTO);
}