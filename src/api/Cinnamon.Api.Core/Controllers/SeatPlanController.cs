using AutoMapper;
using Cinnamon.Api.Core.Services.SeatPlanService.Handler;
using Cinnamon.Framework.ApiCommand.ApiCore;
using Cinnamon.Framework.ApiCommand.ApiCore.DTO.SeatPlan;
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
    private readonly IGetTemplatesHandler getTemplatesHandler;
    private readonly IGetTemplateHandler getTemplateHandler;
    private readonly IChangeSeatPlanStatusHandler changeSeatPlanStatusHandler;
    private readonly IUpdateSeatStatusHandler updateSeatStatusHandler;
    private readonly IMapper mapper;

    public SeatPlanController(ICreateSeatPlanTemplateHandler createSeatPlanTemplateHandler,
        IGetTemplatesHandler getTemplatesHandler, IMapper mapper, IGetTemplateHandler getTemplateHandler,
        IChangeSeatPlanStatusHandler changeSeatPlanStatusHandler, IUpdateSeatStatusHandler updateSeatStatusHandler)
    {
        this.createSeatPlanTemplateHandler = createSeatPlanTemplateHandler;
        this.getTemplatesHandler = getTemplatesHandler;
        this.mapper = mapper;
        this.getTemplateHandler = getTemplateHandler;
        this.changeSeatPlanStatusHandler = changeSeatPlanStatusHandler;
        this.updateSeatStatusHandler = updateSeatStatusHandler; 
    }

    [HttpGet]
    [ProducesResponseType(typeof(GetTemplatesResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTemplates([FromQuery] GetTemplatesArgs args)
    {
        try
        {
            var result = await getTemplatesHandler.ExecuteAsync(new Services.SeatPlanService.Interactors.GetTemplatesArgs {
                Name = args.Name,
                Page = args.PageIndex,
                Limit = args.CountPerPage,
            });
            if(!result.Succeeded || result.Result is null)
            {
                return new JsonResult(new GetTemplatesResult {ErrorInfo = new ErrorInfo {Message = result.Message}});
            }

            var templates = mapper.Map<IEnumerable<SeatPlanTemplateDTO>>(result.Result.Templates);

            return new JsonResult(new GetTemplatesResult {Result = templates, IsSuccess = true});
        }
        catch (System.Exception ex)
        {
            return new JsonResult(new GetTemplatesResult {ErrorInfo = new ErrorInfo {Message = ex.Message}});
        }
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

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(GetTemplateResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTemplate(int id)
    {
        try
        {
            var result = await getTemplateHandler.ExecuteAsync(new Services.SeatPlanService.Interactors.GetTemplateArgs {
                Id = id,
            });
            if (!result.Succeeded || result.Result is null)
            {
                return new JsonResult(new GetTemplateResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            var template = mapper.Map<SeatPlanTemplateDTO>(result.Result);

            return new JsonResult(new GetTemplateResult { Result = template, IsSuccess = true });
        }
        catch (System.Exception ex)
        {
            return new JsonResult(new GetTemplateResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [HttpPut("{id}/status")]
    [ProducesResponseType(typeof(ChangeSeatPlanStatusResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> ChangeSeatPlanStatus(int id, [FromBody] ChangeSeatPlanStatusArgs args)
    {
        try
        {
            var result = await changeSeatPlanStatusHandler.ExecuteAsync(new Services.SeatPlanService.Interactors.ChangeSeatPlanStatusArgs
            {
                Id = id,
                Enabled = args.Enabled
            });
            if (!result.Succeeded || result.Result is null)
            {
                return new JsonResult(new ChangeSeatPlanStatusResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new ChangeSeatPlanStatusResult { Result = result.Result.Disable, IsSuccess = true });
        }
        catch (System.Exception ex)
        {
            return new JsonResult(new ChangeSeatPlanStatusResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("UpdateSeatStatus")]
    [HttpPost]
    [ProducesResponseType(typeof(UpdateSeatStatusResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateSeatStatus([FromForm] UpdateSeatStatusArgs args)
    {
        try
        {
            var result = await updateSeatStatusHandler.ExecuteAsync(new Services.SeatPlanService.Interactors.UpdateSeatStatusArgs
            {
                ActivityId = args.ActivityId,
                EventDate  = args.EventDate,
                CategoryId = args.CategoryId,
                RowId      = args.RowId,
                SeatId     = args.SeatId,
                Occupied   = args.Occupied
            });

            if (!result.Succeeded || result.Result is null)
            {
                return new JsonResult(new UpdateSeatStatusResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new UpdateSeatStatusResult 
            { 
                IsSuccess = result.Result.IsSuccess 
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new UpdateSeatStatusResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }
}