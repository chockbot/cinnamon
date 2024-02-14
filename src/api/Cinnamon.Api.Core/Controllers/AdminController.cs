using Cinnamon.Api.Core.Services.AdminService.Handlers;
using Cinnamon.Api.Core.Services.Disbursement.Handlers;
using Cinnamon.Framework.ApiCommand.ApiCore;
using Cinnamon.Framework.ApiCommand.ApiCore.AdminUser.Request;
using Cinnamon.Framework.ApiCommand.ApiCore.AdminUser.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cinnamon.Api.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AdminController : ControllerBase
    {
        private readonly IGetAdminUserByEmailHandler getAdminUserByEmailHandler;
        private readonly IUpdateCustomerPricingHandler updateCustomerPricingHandler;
        private readonly IGetAllInclusiveTransactionHandler getAllInclusiveTransactionHandler;
        private readonly ICreateCouponHandler createCouponHandler;
        private readonly IGetDisbursements getDisbursements;
        private readonly ILogger _logger;

        public AdminController(IGetAdminUserByEmailHandler getAdminUserByEmailHandler, ILogger<AdminController> logger,
            IUpdateCustomerPricingHandler updateCustomerPricingHandler, IGetAllInclusiveTransactionHandler getAllInclusiveTransactionHandler, 
            ICreateCouponHandler createCouponHandler, IGetDisbursements getDisbursements)
        {
            _logger = logger;

            this.getAdminUserByEmailHandler = getAdminUserByEmailHandler;
            this.updateCustomerPricingHandler = updateCustomerPricingHandler;
            this.getAllInclusiveTransactionHandler = getAllInclusiveTransactionHandler;
            this.createCouponHandler = createCouponHandler;
            this.getDisbursements = getDisbursements;
        }

        [Route("User")]
        [HttpGet]
        [ProducesResponseType(typeof(GetAdminUserByEmailResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAdminUserByEmail([FromQuery] GetAdminUserByEmailArgs args)
        {
            try
            {
                var result = await getAdminUserByEmailHandler.ExecuteAsync(new Services.AdminService.Interactors.GetAdminUserByEmailArgs
                {
                    Email = args.Email
                });

                if (!result.Succeeded || result.Result == null)
                {
                    return new JsonResult(new GetAdminUserByEmailResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
                }
                var adminUserResult = result.Result.AdminUserDetail;

                return new JsonResult(new GetAdminUserByEmailResult
                {
                    IsSuccess = true,
                    Result = new Framework.ApiCommand.ApiCore.DTO.AdminUserDTO.AdminUserDTO
                    {
                        EmailAddress = adminUserResult.EmailAddress,
                        FirstName    = adminUserResult.FirstName,
                        LastName     = adminUserResult.LastName,
                        Id           = adminUserResult.Id,
                        IsAdmin      = adminUserResult.IsAdmin
                    }
                });

            }
            catch (Exception ex)
            {
                return new JsonResult(new GetAdminUserByEmailResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
            }
        }

        [Route("CustomerPricing")]
        [HttpPost]
        [ProducesResponseType(typeof(UpdateCustomerPricingResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> CustomerPricing([FromBody] UpdateCustomerPricingArgs args)
        {
            try
            {
                var result = await updateCustomerPricingHandler.ExecuteAsync(new Services.AdminService.Interactors.UpdateCustomerPricingArgs {
                    CustomerId = args.CustomerId,
                    Rate = args.Rate,
                    IsManualPayment = args.IsManualPayment,
                    InclusivePricing = args.InclusivePricing
                });
                if (!result.Succeeded || result.Result == null)
                {
                    return new JsonResult(new GetAdminUserByEmailResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
                }

                return new JsonResult(new UpdateCustomerPricingResult
                {
                    IsSuccess = true,
                    Result = true
                });

            }
            catch (Exception ex)
            {
                return new JsonResult(new UpdateCustomerPricingResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
            }
        }

        [Route("GetAllInclusiveTransactions")]
        [HttpGet]
        [ProducesResponseType(typeof(GetAllInclusiveTransactionResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllInclusiveTransactions([FromQuery] GetAllInclusiveTransactionArgs args)
        {
            try
            {
                var result = await getAllInclusiveTransactionHandler.ExecuteAsync(new Services.AdminService.Interactors.GetAllInclusiveTransactionArgs {
                    Email = args.Email,
                    Name = args.Name,
                    Status = args.Status,
                    PurchaseDateFrom = args.PurchaseDateFrom,
                    PurchaseDateTo = args.PurchaseDateTo
                });
                if (!result.Succeeded || result.Result == null)
                {
                    return new JsonResult(new GetAdminUserByEmailResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
                }

                return new JsonResult(new GetAllInclusiveTransactionResult
                {
                    IsSuccess = true,
                    Result = result.Result.Transactions.Select(t => new Framework.ApiCommand.ApiCore.DTO.PurchaseOrder.InclusiveTransactionDTO {
                        ConvinienceFee = t.ConvinienceFee,
                        CreditAmount = t.CreditAmount,
                        OverallTotal = t.OverallTotal,
                        PurchaseDate = t.PurchaseDate,
                        PurchaseOrderId = t.PurchaseOrderId,
                        Status = t.Status,
                        Total = t.Total,
                        UnitCount = t.UnitCount,
                        UnitPrice = t.UnitPrice,
                        Provider = new Framework.ApiCommand.ApiCore.DTO.PurchaseOrder.InclusiveTransactionDTO.ProviderDTO {
                            Email = t.Provider.Email,
                            FirstName = t.Provider.FirstName,
                            LastName = t.Provider.LastName
                        }
                    })
                });

            }
            catch (Exception ex)
            {
                return new JsonResult(new GetAllInclusiveTransactionResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
            }
        }

        [Route("CreateCoupon")]
        [HttpPost]
        [ProducesResponseType(typeof(AdminCreateCouponResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateCoupon([FromBody] AdminCreateCouponArgs args)
        {
            try
            {
                var result = await createCouponHandler.ExecuteAsync(new Services.AdminService.Interactors.CreateCouponArgs {
                    ActivityId = args.ActivityId,
                    Amount = args.Amount,
                    Code = args.Code,
                    DiscountType = args.DiscountType,
                    FromDate = args.FromDate,
                    MaximumSpend = args.MaximumSpend,
                    Name = args.Name,
                    ToDate = args.ToDate
                });

                if (!result.Succeeded || result.Result == null)
                {
                    return new JsonResult(new AdminCreateCouponResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
                }
                var created = result.Result;

                return new JsonResult(new AdminCreateCouponResult {
                    IsSuccess = true,
                    Result = new Framework.ApiCommand.ApiCore.DTO.Coupon.CouponDTO {
                        ActivityId = created.ActivityId,
                        Amount = created.Amount,
                        Code = created.Code,
                        CustomerId = created.CustomerId,
                        DiscountType = created.DiscountType,
                        FromDate = created.FromDate,
                        Id = created.Id,
                        IsAdmin = created.IsAdmin,
                        MaximumSpend = created.MaximumSpend,
                        Name = created.Name,
                        Status = created.Status,
                        ToDate = created.ToDate,
                        DateCreated = created.DateCreated
                    }
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new AdminCreateCouponResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
            }
        }

        [Route("GetDisbursements")]
        [HttpGet]
        [ProducesResponseType(typeof(GetDisbursementResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDisbursements([FromQuery] GetDisbursementArgs args)
        {
            try
            {
                var result = await getDisbursements.ExecuteAsync(new Services.Disbursement.Interactors.GetDisbursementsArgs {
                    FilterBy = args.FilterBy ?? string.Empty,
                    FilterValue = args.FilterValue ?? string.Empty
                });
                if (!result.Succeeded || result.Result == null)
                {
                    return new JsonResult(new GetDisbursementResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
                }

                return new JsonResult(new GetDisbursementResult
                {
                    IsSuccess = true,
                    Result = result.Result.Disbursements.Select(d => new Framework.ApiCommand.ApiCore.DTO.Disbursement.DisbursementsInformationDTO {
                        Amount = d.Amount,
                        Id = d.Id,
                        InclusivePayment = d.InclusivePayment,
                        Label = d.Label,
                        ProviderEmail = d.ProviderEmail,
                        ProviderFirstName = d.ProviderFirstName,
                        ProviderLastName = d.ProviderLastName,
                        Remarks = d.Remarks,
                        Status = d.Status
                    })
                });

            }
            catch (Exception ex)
            {
                return new JsonResult(new GetDisbursementResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
            }
        }
    }
}
