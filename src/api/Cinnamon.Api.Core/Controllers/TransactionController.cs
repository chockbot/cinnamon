using Cinnamon.Api.Core.Services.TransactionService.Handlers;
using Cinnamon.Framework.ApiCommand.ApiCore;
using Cinnamon.Framework.ApiCommand.ApiCore.DTO.PurchaseOrder;
using Cinnamon.Framework.ApiCommand.ApiCore.Transaction.Request;
using Cinnamon.Framework.ApiCommand.ApiCore.Transaction.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace Cinnamon.Api.Core.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TransactionController : ControllerBase
{
    private readonly IPurchaseOrderHandler purchaseOrderHandler;
    private readonly IGetPurchaseOrderHandler getPurchaseOrderHandler;
    private readonly IGetGrossSalesByProviderHandler getGrossSalesByProviderHandler;
    private readonly IGetPayoutsByProviderHandler getPayoutsByProviderHandler;

    public TransactionController(IPurchaseOrderHandler purchaseOrderHandler, IGetPurchaseOrderHandler getPurchaseOrderHandler, IGetGrossSalesByProviderHandler getGrossSalesByProviderHandler,
        IGetPayoutsByProviderHandler getPayoutsByProviderHandler)
    {
        this.purchaseOrderHandler = purchaseOrderHandler;
        this.getPurchaseOrderHandler = getPurchaseOrderHandler;
        this.getGrossSalesByProviderHandler = getGrossSalesByProviderHandler;
        this.getPayoutsByProviderHandler = getPayoutsByProviderHandler;
    }

    [Route("SubmitPurchaseOrder")]
    [HttpPost]
    [ProducesResponseType(typeof(SubmitPurchaseOrderResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> SubmitPurchaseOrder([FromBody] SubmitPurchaseOrderArgs args)
    {
        try
        {
            var result = await purchaseOrderHandler.ExecuteAsync(new Services.TransactionService.Interactors.PurchaseOrderArgs {
                ActivityId = args.ActivityId,
                CouponCode = args.CouponCode,
                NumberOfHeads = args.NumberOfHeads,
                ScheduleId = args.ScheduleId,
                Students = args.Students.Select(s => {
                    return new Services.TransactionService.Interactors.PurchaseOrderArgs.Enrollee {
                        FamilyMemberId = s.FamilyMemberId,
                        Name = s.Name
                    };
                }),
                PaymentChannel = args.PaymentChannel,
                PaymentMethod = args.PaymentMethod,
                CardInformation = args.CardInformation != null ? new Services.TransactionService.Interactors.PurchaseOrderArgs.CardDetails {
                    AccountHolder = args.CardInformation.AccountHolder,
                    CardNumber = args.CardInformation.CardNumber,
                    CVV = args.CardInformation.CVV,
                    ExpireMonthYear = args.CardInformation.ExpireMonthYear
                } : null,
                IsCreditsApplied = args.IsCreditsApplied,
                SelectedPeriod = args.SelectedPeriod ?? string.Empty
            });

            if(!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new SubmitPurchaseOrderResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new SubmitPurchaseOrderResult 
                {
                    IsSuccess = true, 
                    Result = new PaymentOrderDTO {
                        Action = result.Result.Action,
                        Id = result.Result.Id,
                        Url = result.Result.Url
                    }
                }
            );
        }
        catch (Exception ex)
        {
            return new JsonResult(new SubmitPurchaseOrderResult {ErrorInfo = new ErrorInfo {Message = ex.Message}});
        }
    }

    [Route("GetPurchaseOrder/{id}")]
    [HttpGet]
    [ProducesResponseType(typeof(GetPurchaseOrderResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPurchaseOrder(int id)
    {
        try
        {
            var result = await getPurchaseOrderHandler.ExecuteAsync(new Services.TransactionService.Interactors.GetPurchaseOrderArgs {
                PurchaseOrderId = id
            });

            if(!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetPurchaseOrderResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }
            var purchseOrder = result.Result;

            return new JsonResult(new GetPurchaseOrderResult 
            {
                IsSuccess = true, 
                Result = new PurchaseOrderDTO 
                {
                    Id = purchseOrder.Id,
                    ActivityId = purchseOrder.ActivityId,
                    ConvinienceFee = purchseOrder.ConvinienceFee,
                    Coupon = purchseOrder.Coupon,
                    CouponAmount = purchseOrder.CouponAmount,
                    CustomerId = purchseOrder.CustomerId,
                    OverallTotal = purchseOrder.OverallTotal,
                    ScheduleId = purchseOrder.ScheduleId,
                    Total = purchseOrder.Total,
                    EnrolleeCount = purchseOrder.EnrolleeCount,
                    PaymentMethod = purchseOrder.PaymentMethod,
                    ServiceFee = purchseOrder.ServiceFee,
                    PaymentProviderFee = purchseOrder.PaymentProviderFee,
                    AppliedCredit = purchseOrder.AppliedCredits,
                    IsInclusivePayment = purchseOrder.IsInclusivePayment
                }
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetPurchaseOrderResult {ErrorInfo = new ErrorInfo {Message = ex.Message}});
        }
    }

    [Route("GetGrossSalesByProvider")]
    [HttpGet]
    [ProducesResponseType(typeof(GetGrossSalesByProviderResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetGrossSalesByProvider([FromQuery] GetGrossSalesByProviderArgs args)
    {
        try
        {
            var date = DateTime.ParseExact(args.DateFrom, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
            var result = await getGrossSalesByProviderHandler.ExecuteAsync(new Services.TransactionService.Interactors.GetGrossSalesByProviderArgs
            {
                Id = args.Id,   
                DateFrom = date
            });
            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetGrossSalesByProviderResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new GetGrossSalesByProviderResult
            {
                IsSuccess = true,
                Result = result.Result.GrossSales.Select(s =>
                {
                    return new Framework.ApiCommand.ApiCore.DTO.PurchaseOrder.GrossSalesDTO
                    {
                        Id           = s.Id,
                        ActivityId   = s.ActivityId,
                        CustomerId   = s.CustomerId,
                        ScheduleId   = s.ScheduleId,
                        Payload      = s.Payload,
                        PurchaseDate = s.PurchaseDate,
                        Status       = s.Status,
                        Total        = s.Total,
                        UnitCount    = s.UnitCount
                    };
                })
            }); 
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetGrossSalesByProviderResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("GetPayoutsByProvider")]
    [HttpGet]
    [ProducesResponseType(typeof(GetPayoutsByProviderResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPayoutsByProvider([FromQuery] GetPayoutsByProviderArgs args)
    {
        try
        {
            var date = DateTime.ParseExact(args.DateFrom, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
            var result = await getPayoutsByProviderHandler.ExecuteAsync(new Services.TransactionService.Interactors.GetPayoutsByProviderArgs
            {
                Id = args.Id,
                DateFrom = date
            });
            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetPayoutsByProviderResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new GetPayoutsByProviderResult
            {
                IsSuccess = true,
                Result = result.Result.PayoutsLog.Select(s =>
                {
                    return new Framework.ApiCommand.ApiCore.DTO.PayoutLog.PayoutDTO
                    {
                        Id              = s.Id,
                        PurchaseOrderId = s.PurchaseOrderId,
                        CustomerId      = s.CustomerId,
                        Amount          = s.Amount,
                        Status          = s.Status,
                        PayoutDate      = s.PayoutDate
                    };
                })
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetPayoutsByProviderResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }
}