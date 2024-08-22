using Cinnamon.Framework.ApiCommand.ApiData.SeatPlan.Request;
using Cinnamon.Framework.ApiCommand.ApiData.SeatPlan.Response;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Modules.DataAccess.Handlers;

public interface ISeatPlanData
{
    Task<AppResult<GetSeatPlanTemplateResult>> GetSeatPlanTemplateAsync(GetSeatPlanTemplateArgs args);
    Task<AppResult<GetSeatPlanTemplateByIdResult>> GetSeatPlanTemplateByIdAsync(int id);
    Task<AppResult<GetSeatPlanFormatterResult>> GetSeatPlanFormatterAsync();
    Task<AppResult<CreateSeatPlanTemplateResult>> CreateSeatPlanTemplateAsync(CreateSeatPlanTemplateArgs args);
    Task<AppResult<UpdateSeatPlanTemplateResult>> UpdateSeatPlanTemplateAsync(UpdateSeatPlanTemplateArgs args, int id);
}
