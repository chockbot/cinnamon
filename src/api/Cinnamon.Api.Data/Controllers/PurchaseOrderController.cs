using Microsoft.AspNetCore.Mvc;
using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiData;
using Cinnamon.Framework.ApiCommand.ApiData.PurchaseOrder.Response;
using Cinnamon.Framework.ApiCommand.ApiData.PurchaseOrder.Request;
using System.Globalization;

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
                args.Status, args.Payload ?? string.Empty, args.CreditAmount, args.UnitPrice, args.UnitCount, args.IsInclusivePayment,
                args.PerUnitDisburseAmount, args.TotalDisburseAmount);

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
                args.ConvinienceFee, args.Coupon, args.CouponAmount, args.OverallTotal, args.Status, args.CreditAmount,
                args.UnitPrice, args.UnitCount, args.PGPayload);

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

    [Route("UpdatePurchaseOrdersStatus")]
    [HttpPost]
    [ProducesResponseType(typeof(UpdatePurchaseOrdersStatusResult), StatusCodes.Status202Accepted)]
    public async Task<IActionResult> UpdatePurchaseOrdersStatus([FromBody] UpdatePurchaseOrdersStatusArgs args)
    {
        try
        {
            var result = await purchaseOrderRepository.UpdatePurchaseOrdersStatus(args.Ids, args.Status);

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new UpdatePurchaseOrdersStatusResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new UpdatePurchaseOrdersStatusResult { IsSuccess = true, Result = result.Result });
        }
        catch (Exception ex)
        {
            return new JsonResult(new UpdatePurchaseOrdersStatusResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("GetAllInclusiveTransactions")]
    [HttpGet]
    [ProducesResponseType(typeof(GetAllInclusiveTransactionResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllInclusiveTransactions([FromQuery] GetAllInclusiveTransactionArgs args)
    {
        try
        {
            DateTime? purchaseDateFrom = null;
            DateTime? purchaseDateTo = null;
            if(!string.IsNullOrEmpty(args.PurchaseDateFrom) && !string.IsNullOrEmpty(args.PurchaseDateTo))
            {
                purchaseDateFrom = DateTime.ParseExact(args.PurchaseDateFrom, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
                purchaseDateTo = DateTime.ParseExact(args.PurchaseDateTo, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
            }

            var result = await purchaseOrderRepository.GetInclusiveTransactions(args.Name, args.Email, args.Status, purchaseDateFrom, purchaseDateTo);
            if(!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetAllInclusiveTransactionResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new GetAllInclusiveTransactionResult
            {
                Result = result.Result,
                IsSuccess = true,
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetAllInclusiveTransactionResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("GetGrossSalesByProvider")]
    [HttpGet]
    [ProducesResponseType(typeof(GetGrossSalesByProviderResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetGrossSalesByProvider([FromQuery] GetGrossSalesByProviderArgs args)
    {
        try
        {
            DateTime From = DateTime.ParseExact(args.DateFrom, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);

            var result = await purchaseOrderRepository.GetGrossSalesByProvider(args.Id, From);
            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetGrossSalesByProviderResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new GetGrossSalesByProviderResult
            {
                Result = result.Result,
                IsSuccess = true,
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetGrossSalesByProviderResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("GetOteNeedToDisburse")]
    [HttpGet]
    [ProducesResponseType(typeof(GetOteNeedToDisburseResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOteNeedToDisburse()
    {
        try
        {
            var result = await purchaseOrderRepository.GetAllOteNeedToDisburse();

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetOteNeedToDisburseResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetOteNeedToDisburseResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new GetOteNeedToDisburseResult
            {
                Result = result.Result,
                IsSuccess = true,
                Pagination = new()
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetOteNeedToDisburseResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }
}