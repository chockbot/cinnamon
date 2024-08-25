using Cinnamon.Framework.ApiCommand.ApiCore.SeatPlan.Request;
using Cinnamon.Framework.ApiCommand.ApiCore.SeatPlan.Response;
using Cinnamon.Framework.Common;

namespace Cinnamon.Web.Modules.ApiAccess.Handlers;

public interface ISeatPlanApiHandler 
{
    Task<AppResult<CreateSeatPlanTemplateResult>> CreateSeatPlanTemplate(CreateSeatPlanTemplateArgs args, string token);

    Task<AppResult<GetTemplatesResult>> GetTemplates(GetTemplatesArgs args, string token);
} 
