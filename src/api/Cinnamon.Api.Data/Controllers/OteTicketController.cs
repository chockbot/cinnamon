using AutoMapper;
using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiData;
using Cinnamon.Framework.ApiCommand.ApiData.OteTicket.Request;
using Cinnamon.Framework.ApiCommand.ApiData.OteTicket.Response;
using Dto = Cinnamon.Framework.ApiCommand.ApiData.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Cinnamon.Api.Data.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OteTicketController : ControllerBase 
{
    private readonly IOteTicketRepository oteTicketRepository;
    private readonly IMapper mapper;

    public OteTicketController(IOteTicketRepository oteTicketRepository, IMapper mapper)
    {
        this.oteTicketRepository = oteTicketRepository;
        this.mapper = mapper;
    }

    [Route("create-many")]
    [HttpPost]
    [ProducesResponseType(typeof(CreateManyOteTicketsResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateTickets([FromBody] CreateManyOteTicketsArgs args)
    {
        try
        {
            var dtoTickets = mapper.Map<IEnumerable<Dto.OteTicket.OteTicketDTO>>(args.Tickets);
            var result = await oteTicketRepository.CreateMany(dtoTickets, args.IncludeImageAsResult);
            if(!result.Succeeded || result.Result is null)
            {
                return new JsonResult(new CreateManyOteTicketsResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new CreateManyOteTicketsResult { IsSuccess = true, Result = result.Result });
        }
        catch (Exception ex)
        {
            return new JsonResult(new CreateManyOteTicketsResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("by-activity/{activityId}")]
    [HttpGet]
    [ProducesResponseType(typeof(GetByActivityIdResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByActivityId(int activityId, [FromQuery] GetByActivityIdArgs args)
    {
        try
        {
            var result = await oteTicketRepository.GetByActivityId(activityId, args.IncludeCustomer ?? false, args.IncludeImageAsResult ?? false);
            if(!result.Succeeded || result.Result is null)
            {
                return new JsonResult(new GetByActivityIdResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new GetByActivityIdResult { IsSuccess = true, Result = result.Result });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetByActivityIdResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("by-code/{code}")]
    [HttpGet]
    [ProducesResponseType(typeof(GetByCodeResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByCode(string code)
    {
        try
        {
            var result = await oteTicketRepository.GetByCode(code);
            if(!result.Succeeded || result.Result is null)
            {
                return new JsonResult(new GetByCodeResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new GetByCodeResult { IsSuccess = true, Result = result.Result });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetByCodeResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("by-purchase-order/{purchaseOrderId}")]
    [HttpGet]
    [ProducesResponseType(typeof(GetByPurchaseOrderIdResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByPurchaseOrderId(int purchaseOrderId, [FromQuery] GetByPurchaseOrderIdArgs args)
    {
        try
        {
            var result = await oteTicketRepository.GetByPurchaseOrderId(purchaseOrderId, args.IncludeCustomer ?? false, args.IncludeImageAsResult ?? false);
            if(!result.Succeeded || result.Result is null)
            {
                return new JsonResult(new GetByPurchaseOrderIdResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new GetByPurchaseOrderIdResult { IsSuccess = true, Result = result.Result });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetByPurchaseOrderIdResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("update-ticket-status")]
    [HttpPost]
    [ProducesResponseType(typeof(UpdateTicketResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateTicket([FromBody] UpdateTicketArgs args)
    {
        try
        {
            var result = await oteTicketRepository.Update(new Dto.OteTicket.OteTicketDTO {
                Id = args.Id,
                Status = args.Status
            });
            if(!result.Succeeded || result.Result is null)
            {
                return new JsonResult(new UpdateTicketResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new UpdateTicketResult { IsSuccess = true, Result = result.Result });
        }
        catch (Exception ex)
        {
            return new JsonResult(new UpdateTicketResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }
}