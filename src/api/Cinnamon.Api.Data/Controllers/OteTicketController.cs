using AutoMapper;
using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiData;
using Cinnamon.Framework.ApiCommand.ApiData.OteTicket.Request;
using Cinnamon.Framework.ApiCommand.ApiData.OteTicket.Response;
using Dto = Cinnamon.Framework.ApiCommand.ApiData.DTO;
using Microsoft.AspNetCore.Mvc;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.OteSchedule;
using Microsoft.AspNetCore.Authorization;

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
    [AllowAnonymous]
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

    [Route("CreateSharedLink")]
    [HttpPost]
    [ProducesResponseType(typeof(CreateSharedLinkResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateSharedLink([FromBody] CreateSharedLinkArgs args)
    {
        try
        {
            var dto = mapper.Map<OteSharedLinkDTO>(args);
            dto.Enable = true;

            var result = await oteTicketRepository.CreateSharedLink(dto);
            if(!result.Succeeded || result.Result is null)
            {
                return new JsonResult(new CreateSharedLinkResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new CreateSharedLinkResult { IsSuccess = true, Result = result.Result });
        }
        catch (Exception ex)
        {
            return new JsonResult(new CreateSharedLinkResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("GetSharedLink")]
    [HttpGet]
    [ProducesResponseType(typeof(GetSharedLinkResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSharedLink([FromQuery] GetSharedLinkArgs args)
    {
        try
        {
            List<OteSharedLinkDTO> sharedLinks = new();

            if(!string.IsNullOrEmpty(args.Guid) && !string.IsNullOrEmpty(args.Token))
            {
                var tokenResult = await oteTicketRepository.GetSharedLinks(args.Token, args.Guid);
                if(!tokenResult.Succeeded || tokenResult.Result is null)
                {
                    return new JsonResult(new GetSharedLinkResult { ErrorInfo = new ErrorInfo { Message = tokenResult.Message } });
                }

                sharedLinks.Add(tokenResult.Result);
            }

            if(args.ActivityId is not null && args.OteDateId is not null)
            {
                var idResult = await oteTicketRepository.GetSharedLinks(args.ActivityId.Value, args.OteDateId.Value);
                if(!idResult.Succeeded || idResult.Result is null)
                {
                    return new JsonResult(new GetSharedLinkResult { ErrorInfo = new ErrorInfo { Message = idResult.Message } });
                }
                
                sharedLinks.AddRange(idResult.Result);
            }

            return new JsonResult(new GetSharedLinkResult { IsSuccess = true, Result = sharedLinks });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetSharedLinkResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("UpdateSharedLinkStatus")]
    [HttpPost]
    [ProducesResponseType(typeof(UpdateSharedLinkStatusResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateSharedLinkStatus([FromBody] UpdateSharedLinkStatusArgs args)
    {
        try
        {
            var result = await oteTicketRepository.UpdateSharedLinkStatus(args.Id, args.Status);
            if(!result.Succeeded || result.Result is null)
            {
                return new JsonResult(new UpdateSharedLinkStatusResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new UpdateSharedLinkStatusResult { IsSuccess = true, Result = result.Result });
        }
        catch (Exception ex)
        {
            return new JsonResult(new UpdateSharedLinkStatusResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("CountBookedTickets")]
    [HttpGet]
    [ProducesResponseType(typeof(CountBookedTicketsResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> CountBookedTickets([FromQuery] CountBookedTicketsArgs args)
    {
        try
        {
            var result = await oteTicketRepository.CountBookedTickets(args.ActivityId);
            if(!result.Succeeded)
            {
                return new JsonResult(new CountBookedTicketsResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new CountBookedTicketsResult { IsSuccess = true, Result = result.Result });
        }
        catch (Exception ex)
        {
            return new JsonResult(new CountBookedTicketsResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("BookedCustomers")]
    [HttpGet]
    [ProducesResponseType(typeof(BookedCustomersResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> BookedCustomers([FromQuery] BookedCustomersArgs args)
    {
        try
        {
            var result = await oteTicketRepository.BookedCustomers(args.ActivityId, args.DateId, args.Limit, args.Offset);
            if(!result.Succeeded || result.Result is null)
            {
                return new JsonResult(new BookedCustomersResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new BookedCustomersResult { IsSuccess = true, Result = result.Result });
        }
        catch (Exception ex)
        {
            return new JsonResult(new BookedCustomersResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("GetAllTicketPurchased")]
    [HttpGet]
    [ProducesResponseType(typeof(GetAllTicketPurchasedResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllTicketPurchased([FromQuery] GetAllTicketPurchasedArgs args)
    {
        try
        {
            var result = await oteTicketRepository.GetAllTicketPurchased(args.ActivityId);
            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetAllTicketPurchasedResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new GetAllTicketPurchasedResult { Result = result.Result, IsSuccess = true });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetAllTicketPurchasedResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }
}