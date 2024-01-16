using AutoMapper;
using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiData;
using Cinnamon.Framework.ApiCommand.ApiData.OteTicket.Request;
using Cinnamon.Framework.ApiCommand.ApiData.OteTicket.Response;
using Dto = Cinnamon.Framework.ApiCommand.ApiData.DTO;
using Microsoft.AspNetCore.Mvc;
using Cinnamon.Api.Data.Services.Repository.Student;
using Cinnamon.Api.Data.Services.Repository.Activity;

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
            var result = await oteTicketRepository.GetByActivityId(activityId, args.DateId, args.SearchValue ?? string.Empty, args.SearchBy ?? 0,
                args.CountPerPage, (args.PageIndex - 1) * args.CountPerPage, args.IncludeCustomer ?? false, args.IncludeImageAsResult ?? false);
            
            if(!result.Succeeded || result.Result is null)
            {
                return new JsonResult(new GetByActivityIdResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            // get all without pagination to get all rows
            var all = await oteTicketRepository.GetByActivityId(activityId, args.DateId, args.SearchValue ?? string.Empty, args.SearchBy ?? 0,
                null , null, args.IncludeCustomer ?? false, args.IncludeImageAsResult ?? false);
            if (!all.Succeeded || all.Result == null)
            {
                return new JsonResult(new GetByActivityIdResult { ErrorInfo = new ErrorInfo { Message = all.Message } });
            }

            var totalRecords = all.Result.Count();
            return new JsonResult(new GetByActivityIdResult
            {
                Result = result.Result,
                IsSuccess = true,
                Pagination = new Pagination
                {
                    PageIndex = args.PageIndex,
                    PerPage = args.CountPerPage,
                    TotalRecords = totalRecords,
                    TotalPages = args.CountPerPage.HasValue && args.PageIndex.HasValue ?
                                (int)Math.Ceiling((double)totalRecords / args.CountPerPage.Value) : null
                }
            });
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
    [Route("GetTicketDetails")]
    [HttpGet]
    [ProducesResponseType(typeof(GetTicketDetailsResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOTEByProvider([FromQuery] GetTicketDetailsArgs args)
    {
        try
        {
            var result = await oteTicketRepository.GetTicketDetails(args.ActivityId, args.DateId);
            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetTicketDetailsResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new GetTicketDetailsResult { Result = result.Result, IsSuccess = true });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetTicketDetailsResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }
}