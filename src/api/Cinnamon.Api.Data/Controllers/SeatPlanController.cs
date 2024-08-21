using AutoMapper;
using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiData;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.SeatPlan;
using Cinnamon.Framework.ApiCommand.ApiData.SeatPlan.Request;
using Cinnamon.Framework.ApiCommand.ApiData.SeatPlan.Response;
using Microsoft.AspNetCore.Mvc;

namespace Cinnamon.Api.Data.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SeatPlanController : ControllerBase
{
    private readonly ISeatPlanTemplateRepository seatPlanTemplateRepository;
    private readonly ISeatPlanFormatterRepository seatPlanFormatterRepository;
    private readonly IMapper mapper;

    public SeatPlanController(ISeatPlanTemplateRepository seatPlanTemplateRepository, 
        ISeatPlanFormatterRepository seatPlanFormatterRepository, IMapper mapper)
    {
        this.seatPlanTemplateRepository = seatPlanTemplateRepository;
        this.seatPlanFormatterRepository = seatPlanFormatterRepository;
        this.mapper = mapper;
    }

    [HttpGet("template")]
    [ProducesResponseType(typeof(GetSeatPlanTemplateResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSeatPlanTemplateAsync([FromQuery] GetSeatPlanTemplateArgs args)
    {
        try
        {
            var result = await seatPlanTemplateRepository
                .GetSeatPlanTemplateAsync(args.TemplateName ?? string.Empty, args.PageIndex ?? 1, args.CountPerPage ?? 10);
            
            if(!result.Succeeded || result.Result is null)
            {
                return new JsonResult(new GetSeatPlanTemplateResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new GetSeatPlanTemplateResult { Result = result.Result, IsSuccess = true });
        }
        catch (System.Exception ex)
        {
            return new JsonResult(new GetSeatPlanTemplateResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [HttpGet("template/{id}")]
    [ProducesResponseType(typeof(GetSeatPlanTemplateByIdResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSeatPlanTemplateByIdAsync(int id)
    {
        try
        {
            var result = await seatPlanTemplateRepository.GetSeatPlanTemplateByIdAsync(id);
            if(!result.Succeeded || result.Result is null)
            {
                return new JsonResult(new GetSeatPlanTemplateByIdResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new GetSeatPlanTemplateByIdResult { Result = result.Result, IsSuccess = true });
        }
        catch (System.Exception ex)
        {
            return new JsonResult(new GetSeatPlanTemplateByIdResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [HttpPost("template")]
    [ProducesResponseType(typeof(CreateSeatPlanTemplateResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateSeatPlanTemplateAsync(CreateSeatPlanTemplateArgs args)
    {
        try
        {
            var dto = mapper.Map<SeatPlanTemplateDTO>(args);
            var result = await seatPlanTemplateRepository.CreateSeatPlanTemplateAsync(dto);
            if(!result.Succeeded || result.Result is null)
            {
                return new JsonResult(new CreateSeatPlanTemplateResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }
            
            return new JsonResult(new CreateSeatPlanTemplateResult { Result = result.Result, IsSuccess = true });
        }
        catch (System.Exception ex)
        {
            return new JsonResult(new CreateSeatPlanTemplateResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [HttpPut("template")]
    [ProducesResponseType(typeof(UpdateSeatPlanTemplateResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateSeatPlanTemplateAsync(UpdateSeatPlanTemplateArgs args)
    {
        try
        {
            var dto = mapper.Map<SeatPlanTemplateDTO>(args);
            var result = await seatPlanTemplateRepository.UpdateSeatPlanTemplateAsync(dto);
            if(!result.Succeeded || result.Result is null)
            {
                return new JsonResult(new UpdateSeatPlanTemplateResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new UpdateSeatPlanTemplateResult { Result = result.Result, IsSuccess = true });
        }
        catch (System.Exception ex)
        {
            return new JsonResult(new UpdateSeatPlanTemplateResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [HttpGet("formatter")]
    [ProducesResponseType(typeof(GetSeatPlanFormatterResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSeatPlanFormatterAsync()
    {
        try
        {
            var result = await seatPlanFormatterRepository.GetSeatPlanFormatterAsync();
            if(!result.Succeeded || result.Result is null)
            {
                return new JsonResult(new GetSeatPlanFormatterResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new GetSeatPlanFormatterResult { Result = result.Result, IsSuccess = true });
        }
        catch (System.Exception ex)
        {
            return new JsonResult (new GetSeatPlanFormatterResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }
}