using Microsoft.AspNetCore.Mvc;
using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiData;
using Cinnamon.Framework.ApiCommand.ApiData.PurchaseOrder.Response;
using Cinnamon.Framework.ApiCommand.ApiData.PurchaseOrder.Request;

namespace Cinnamon.Api.Data.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PurchaseOrderController : ControllerBase 
{
    private readonly IPurchaseOrderRepository purchaseOrderRepository;

    public PurchaseOrderController(IPurchaseOrderRepository purchaseOrderRepository)
    {
        this.purchaseOrderRepository = purchaseOrderRepository;
    }

    [Route("GetPurchaseOrderById/{id}")]
    [HttpGet]
    [ProducesResponseType(typeof(GetPurchaseOrderResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPurchaseOrderById(int id)
    {
        try
        {
            var result = await purchaseOrderRepository.GetByIdAsync(id);
            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetPurchaseOrderResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new GetPurchaseOrderResult { Result = result.Result, IsSuccess = true });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetPurchaseOrderResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("GetAllPurchaseOrder")]
    [HttpGet]
    [ProducesResponseType(typeof(GetAllPurchaseOrderResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllPurchaseOrder([FromQuery] GetAllPurchaseOrderArgs args)
    {
        try
        {
            bool isHaveFilter = (args.PageIndex.HasValue && args.CountPerPage.HasValue) || args.CustomerId.HasValue || args.Status.HasValue;

            var result = isHaveFilter?
                await purchaseOrderRepository.GetAllAsync(args.CountPerPage, (args.PageIndex - 1) * args.CountPerPage, 
                    args.IncludeActivity, args.IncludeSchedule, args.CustomerId, args.Status) :
                await purchaseOrderRepository.GetAllAsync();

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetAllPurchaseOrderResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            // get all without pagination to get all rows
            var all = isHaveFilter ?
                await purchaseOrderRepository.GetAllAsync(null, null, null, null, null, null) :
                await purchaseOrderRepository.GetAllAsync();

            if (!all.Succeeded || all.Result == null)
            {
                return new JsonResult(new GetAllPurchaseOrderResult { ErrorInfo = new ErrorInfo { Message = all.Message } });
            }

            var totalRecords = all.Result.Count();
            return new JsonResult(new GetAllPurchaseOrderResult
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
            return new JsonResult(new GetAllPurchaseOrderResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("CreatePurchaseOrder")]
    [HttpPost]
    [ProducesResponseType(typeof(CreatePurchaseOrderResult), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreatePurchaseOrder([FromBody] CreatePurchaseOrderArgs args)
    {
        try
        {
            var result = await purchaseOrderRepository.Create(args.ActivityId, args.ScheduleId, args.CustomerId,
                args.Total, args.ConvinienceFee, args.Coupon, args.CouponAmount, args.OverallTotal, 
                args.Status, args.Payload ?? string.Empty, args.CreditAmount);

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new CreatePurchaseOrderResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new CreatePurchaseOrderResult { IsSuccess = true, Result = result.Result });
        }
        catch (Exception ex)
        {
            return new JsonResult(new CreatePurchaseOrderResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("UpdatePurchaseOrder")]
    [HttpPost]
    [ProducesResponseType(typeof(UpdatePurchaseOrderResult), StatusCodes.Status202Accepted)]
    public async Task<IActionResult> UpdatePurchaseOrder([FromBody] UpdatePurchaseOrderArgs args)
    {
        try
        {
            var result = await purchaseOrderRepository.Update(args.PurchaseOrderId, args.ScheduleId, args.Total,
                args.ConvinienceFee, args.Coupon, args.CouponAmount, args.OverallTotal, args.Status, args.CreditAmount);

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new UpdatePurchaseOrderResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new UpdatePurchaseOrderResult { IsSuccess = true, Result = result.Result });
        }
        catch (Exception ex)
        {
            return new JsonResult(new UpdatePurchaseOrderResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("GetAllPurchaseOrderNeedToPayout")]
    [HttpGet]
    [ProducesResponseType(typeof(GetAllPurchaseOrderResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllPurchaseOrderNeedToPayout()
    {
        try
        {
            var result = await purchaseOrderRepository.GetAllPurchaseOrderNeedToPayout();
            if(!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetAllPurchaseOrderResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new GetAllPurchaseOrderResult
            {
                Result = result.Result,
                IsSuccess = true,
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetAllPurchaseOrderResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }
}