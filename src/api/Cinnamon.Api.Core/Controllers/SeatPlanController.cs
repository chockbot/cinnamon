using Cinnamon.Api.Core.Services.SeatPlanService.Handler;
using Cinnamon.Framework.ApiCommand.ApiCore;
using Cinnamon.Framework.ApiCommand.ApiCore.SeatPlan.Request;
using Cinnamon.Framework.ApiCommand.ApiCore.SeatPlan.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cinnamon.Api.Core.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class SeatPlanController : ControllerBase 
{
    private readonly ICreateSeatPlanTemplateHandler createSeatPlanTemplateHandler;

    public SeatPlanController(ICreateSeatPlanTemplateHandler createSeatPlanTemplateHandler)
    {
        this.createSeatPlanTemplateHandler = createSeatPlanTemplateHandler;
    }

    [HttpPost]
    [ProducesResponseType(typeof(CreateSeatPlanTemplateResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateSeatPlanTemplate([FromForm] CreateSeatPlanTemplateArgs args)
    {
        try
        {
            var result = await createSeatPlanTemplateHandler.ExecuteAsync(new Services.SeatPlanService.Interactors.CreateSeatPlanTemplateArgs {
                Name = args.Name,
                Address = args.Address,
                FormatterId = args.FormatterId,
                ImageFile = args.ImageFile,
                JsonFile = args.JsonFile,
            });
            if(!result.Succeeded || result.Result is null)
            {
                return new JsonResult(new CreateSeatPlanTemplateResult {ErrorInfo = new ErrorInfo {Message = result.Message}});
            }

            return new JsonResult(new CreateSeatPlanTemplateResult {Result = result.Result.Payload, IsSuccess = true});
        }
        catch (System.Exception ex)
        {
            return new JsonResult(new CreateSeatPlanTemplateResult {ErrorInfo = new ErrorInfo {Message = ex.Message}});
        }
    }
}