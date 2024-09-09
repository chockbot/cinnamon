using AutoMapper;
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
    private readonly IOtePurchaseOrderHandler otePurchaseOrderHandler;
    private readonly IOtePurchaseOrderDetailsHandler otePurchaseOrderDetailsHandler;
    private readonly IMapper mapper;
    private readonly ITransactionRedirectionHandler transactionRedirectionHandler;
    private readonly IGetDirectStudentSalesHandler getDirectStudentSalesHandler;
    private readonly IOteRequestPaymentHandler oteRequestPaymentHandler;
    private readonly IGetOteRequestPaymentHandler getOteRequestPayment;

    public TransactionController(IPurchaseOrderHandler purchaseOrderHandler, IGetPurchaseOrderHandler getPurchaseOrderHandler, 
        IGetGrossSalesByProviderHandler getGrossSalesByProviderHandler, IGetPayoutsByProviderHandler getPayoutsByProviderHandler, 
        IOtePurchaseOrderHandler otePurchaseOrderHandler, IOtePurchaseOrderDetailsHandler otePurchaseOrderDetailsHandler, 
        IMapper mapper, ITransactionRedirectionHandler transactionRedirectionHandler, 
        IGetDirectStudentSalesHandler getDirectStudentSalesHandler, IOteRequestPaymentHandler oteRequestPaymentHandler,
        IGetOteRequestPaymentHandler getOteRequestPayment)
    {
        this.purchaseOrderHandler = purchaseOrderHandler;
        this.getPurchaseOrderHandler = getPurchaseOrderHandler;
        this.getGrossSalesByProviderHandler = getGrossSalesByProviderHandler;
        this.getPayoutsByProviderHandler = getPayoutsByProviderHandler;
        this.otePurchaseOrderHandler = otePurchaseOrderHandler;
        this.otePurchaseOrderDetailsHandler = otePurchaseOrderDetailsHandler;
        this.mapper = mapper;
        this.transactionRedirectionHandler = transactionRedirectionHandler;
        this.getDirectStudentSalesHandler = getDirectStudentSalesHandler;
        this.oteRequestPaymentHandler = oteRequestPaymentHandler;
        this.getOteRequestPayment = getOteRequestPayment;
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
                SelectedPeriod = args.SelectedPeriod ?? string.Empty,
                AddOnsAmount = args.AddOns,
                AddOnsDetails = args.AddOnsDetails.Select(s =>
                {
                    return new Services.TransactionService.Interactors.PurchaseOrderArgs.AddOn {
                        AddOnId = s.AddOnId,
                        AddOnName = s.Name,
                        AddOnCount = s.AddOnCount
                    };
                })
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
                    Id                 = purchseOrder.Id,
                    ActivityId         = purchseOrder.ActivityId,
                    ConvinienceFee     = purchseOrder.ConvinienceFee,
                    Coupon             = purchseOrder.Coupon,
                    CouponAmount       = purchseOrder.CouponAmount,
                    CustomerId         = purchseOrder.CustomerId,
                    OverallTotal       = purchseOrder.OverallTotal,
                    ScheduleId         = purchseOrder.ScheduleId,
                    Total              = purchseOrder.Total,
                    EnrolleeCount      = purchseOrder.EnrolleeCount,
                    PaymentMethod      = purchseOrder.PaymentMethod,
                    ServiceFee         = purchseOrder.ServiceFee,
                    PaymentProviderFee = purchseOrder.PaymentProviderFee,
                    AppliedCredit      = purchseOrder.AppliedCredits,
                    IsInclusivePayment = purchseOrder.IsInclusivePayment,
                    AddOnsAmount       = purchseOrder.AddOnsAmount,
                    AddOnsDetails      = purchseOrder.AddOnsDetails.Select(s =>
                    {
                        return new Framework.ApiCommand.ApiCore.DTO.PurchaseOrder.PurchaseOrderDTO.AddOnDetail
                        {
                            Id         = s.AddOnId,
                            Name       = s.AddOnName,
                            AddOnCount = s.AddOnCount
                        };
                    }).ToList(),
                }
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetPurchaseOrderResult {ErrorInfo = new ErrorInfo {Message = ex.Message}});
        }
    }

    [Route("GetDirectStudentSales")]
    [HttpGet]
    [ProducesResponseType(typeof(GetDirectStudentSalesResult), StatusCodes.Status200OK)]
    [AllowAnonymous]
    public async Task<IActionResult> GetDirectStudentSales([FromQuery] GetDirectStudentSalesArgs args)
    {
        try
        {
            var date = DateTime.ParseExact(args.DateFrom, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
            var result = await getDirectStudentSalesHandler.ExecuteAsync(new Services.TransactionService.Interactors.GetDirectStudentSalesArgs
            {
                Id       = args.ProviderId,
                DateFrom = date
            });
            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetDirectStudentSalesResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new GetDirectStudentSalesResult
            {
                IsSuccess = true,
                Result = result.Result.DirectStudentSales.Select(s =>
                {
                    return new Framework.ApiCommand.ApiCore.DTO.DirectStudents.DirectStudentPaymentDTO
                    {
                        Amount    = s.Amount,
                        CreatedOn = s.PurchaseDate,
                    };
                })
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetDirectStudentSalesResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
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
                Id       = args.Id,
                DateFrom = date,
                Status   = args.Status
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

    [Route("SubmitOtePurchaseOrder")]
    [HttpPost]
    [ProducesResponseType(typeof(SubmitOtePurchaseOrderResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> SubmitOtePurchaseOrder([FromBody] SubmitOtePurchaseOrderArgs args)
    {
        try
        {
            var result = await otePurchaseOrderHandler.ExecuteAsync(new Services.TransactionService.Interactors.OtePurchaseOrderArgs {
                ActivityId = args.ActivityId,
                CardInformation = args.CardInformation != null ? new Services.TransactionService.Interactors.OtePurchaseOrderArgs.CardDetails {
                    AccountHolder = args.CardInformation.AccountHolder,
                    CardNumber = args.CardInformation.CardNumber,
                    CVV = args.CardInformation.CVV,
                    ExpireMonthYear = args.CardInformation.ExpireMonthYear
                } : null,
                CouponCode = args.CouponCode,
                IsCreditsApplied = args.IsCreditsApplied,
                PaymentChannel = args.PaymentChannel,
                PaymentMethod = args.PaymentMethod,
                Tickets = args.Tickets.Select(t => {
                    return new Services.TransactionService.Interactors.OtePurchaseOrderArgs.Ticket {
                        Count = t.Count,
                        Id = t.Id,
                        SeatNumber = t.SeatNumber
                    };
                }),
                Questions = args.Questions is not null ? args.Questions.Select(q => new Services.TransactionService.Interactors.OtePurchaseOrderArgs.ActivityQuestion {
                    Answer = q.Answer ?? string.Empty,
                    Id = q.Id,
                    Question = q.Question
                }) : null,
                Guid = args.Guid,
                Token = args.Token
            });

            if(!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new SubmitOtePurchaseOrderResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new SubmitOtePurchaseOrderResult 
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
            return new JsonResult(new SubmitOtePurchaseOrderResult {ErrorInfo = new ErrorInfo {Message = ex.Message}});
        }
    }

    [Route("GetOtePurchaseOrder/{id}")]
    [HttpGet]
    [ProducesResponseType(typeof(OteGetPurchaseOrderResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOtePurchaseOrder(int id)
    {
        try
        {
            var result = await otePurchaseOrderDetailsHandler.ExecuteAsync(new Services.TransactionService.Interactors.OtePurchaseOrderDetailsArgs {
                PurchaseOrderId = id
            });

            if(!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new OteGetPurchaseOrderResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            var resultData = mapper.Map<OtePurchaseOrderDTO>(result.Result);

            return new JsonResult(new OteGetPurchaseOrderResult 
            {
                IsSuccess = true, 
                Result = resultData
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new OteGetPurchaseOrderResult {ErrorInfo = new ErrorInfo {Message = ex.Message}});
        }
    }

    [AllowAnonymous]
    [Route("TransactionRedirection")]
    [HttpGet]
    [ProducesResponseType(typeof(TransactionRedirectionResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> TransactionRedirection([FromQuery] TransactionRedirectionArgs args)
    {
        try
        {
            var result = await transactionRedirectionHandler.ExecuteAsync(new Services.TransactionService.Interactors.TransactionRedirectionArgs {
                Guid = args.Guid,
                Token = args.Token
            });

            if(!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new TransactionRedirectionResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            return new JsonResult(new TransactionRedirectionResult 
            {
                IsSuccess = true, 
                Result = result.Result.RedirectUrl
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new TransactionRedirectionResult {ErrorInfo = new ErrorInfo {Message = ex.Message}});
        }
    }

    [Route("PaymentRequest")]
    [HttpPost]
    [ProducesResponseType(typeof(PaymentRequestResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> PaymentRequest([FromBody] PaymentRequestArgs args)
    {
        try
        {
            var result = await oteRequestPaymentHandler.ExecuteAsync(new Services.TransactionService.Interactors.OteRequestPaymentArgs
            {
                ActivityId = args.ActivityId,
                Guid = args.Guid,
                SelectedDate = args.SelectedDate,
                Token = args.Token,
                SelectedTickets = args.SelectedTickets.Select(t => new Services.TransactionService.Interactors.OteRequestPaymentArgs.RequestPaymentTicket
                {
                    TicketCount = t.TicketCount,
                    TicketId = t.TicketId,
                    SeatNumber = t.SeatNumber,
                    CategoryUUID = t.CategoryUUID,
                    RowUUID = t.RowUUID,
                    SeatUUID = t.SeatUUID
                }),
                Questions = args.Questions?.Select(q => new Services.TransactionService.Interactors.OteRequestPaymentArgs.ProviderQuestion
                {
                    Answer = q.Answer,
                    Id = q.Id,
                    Question = q.Question
                })
            });

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new PaymentRequestResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            var dto = mapper.Map<PaymentRequestDTO>(result.Result);

            return new JsonResult(new PaymentRequestResult
            {
                IsSuccess = true,
                Result = dto
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new PaymentRequestResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("PaymentRequest")]
    [HttpGet]
    [ProducesResponseType(typeof(GetRequestPaymentResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRequestPayment([FromQuery] GetRequestPaymentArgs args)
    {
        try
        {
            var result = await getOteRequestPayment.ExecuteAsync(new Services.TransactionService.Interactors.OteGetRequestPaymentArgs
            {
                Guid = args.Guid,
                Token = args.Token
            });

            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetRequestPaymentResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            var dto = mapper.Map<PaymentRequestDTO>(result.Result);

            return new JsonResult(new GetRequestPaymentResult
            {
                IsSuccess = true,
                Result = dto
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetRequestPaymentResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }
}