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
        private readonly IGetDisbursementDetails getDisbursementDetails;
        private readonly IManualDisbursement manualDisbursement;
        private readonly ICreateAnnouncementHandler createAnnouncementHandler;
        private readonly IUpdateAnnouncementHandler updateAnnouncementHandler;
        private readonly IGetAnnouncementsHandler getAnnouncementsHandler;
        private readonly IDeleteAnnouncementHandler deleteAnnouncementHandler;
        private readonly IUpdateDynamicContentHandler updateDynamicContentHandler;
        private readonly ILogger _logger;

        public AdminController(IGetAdminUserByEmailHandler getAdminUserByEmailHandler, ILogger<AdminController> logger,
            IUpdateCustomerPricingHandler updateCustomerPricingHandler, IGetAllInclusiveTransactionHandler getAllInclusiveTransactionHandler, 
            ICreateCouponHandler createCouponHandler, IGetDisbursements getDisbursements, IGetDisbursementDetails getDisbursementDetails,
            IManualDisbursement manualDisbursement, ICreateAnnouncementHandler createAnnouncementHandler,
            IDeleteAnnouncementHandler deleteAnnouncementHandler, IUpdateDynamicContentHandler updateDynamicContentHandler)
        {
            _logger = logger;

            this.getAdminUserByEmailHandler = getAdminUserByEmailHandler;
            this.updateCustomerPricingHandler = updateCustomerPricingHandler;
            this.getAllInclusiveTransactionHandler = getAllInclusiveTransactionHandler;
            this.createCouponHandler = createCouponHandler;
            this.getDisbursements = getDisbursements;
            this.getDisbursementDetails = getDisbursementDetails;
            this.manualDisbursement = manualDisbursement;
            this.createAnnouncementHandler = createAnnouncementHandler;
            this.updateAnnouncementHandler = updateAnnouncementHandler;
            this.getAnnouncementsHandler = getAnnouncementsHandler;
            this.deleteAnnouncementHandler = deleteAnnouncementHandler;
            this.updateDynamicContentHandler = updateDynamicContentHandler;
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

        [Route("GetDisbursements/{disbursementId}")]
        [HttpGet]
        [ProducesResponseType(typeof(GetDisbursementDetailsResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDisbursementDetails(int disbursementId)
        {
            try
            {
                var result = await getDisbursementDetails.ExecuteAsync(new Services.Disbursement.Interactors.GetDisbursementDetailsArgs {
                    DisbursementId = disbursementId
                });
                if (!result.Succeeded || result.Result == null)
                {
                    return new JsonResult(new GetDisbursementDetailsResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
                }
                var disbursement = result.Result.DisbursementDetailed;

                return new JsonResult(new GetDisbursementDetailsResult
                {
                    IsSuccess = true,
                    Result = new Framework.ApiCommand.ApiCore.DTO.Disbursement.DisbursementDetailedDTO {
                        Disbursement = new Framework.ApiCommand.ApiCore.DTO.Disbursement.DisbursementsInformationDTO {
                            Amount = disbursement.Amount,
                            Id = disbursement.Id,
                            InclusivePayment = disbursement.InclusivePayment,
                            Label = disbursement.Label,
                            ProviderEmail = disbursement.ProviderEmail,
                            ProviderFirstName = disbursement.ProviderFirstName,
                            ProviderLastName = disbursement.ProviderLastName,
                            Remarks = disbursement.Remarks,
                            Status = disbursement.Status,
                        },
                        DisbursementItems = disbursement.DisbursementDetails.Select(d => new Framework.ApiCommand.ApiCore.DTO.Disbursement.DisbursementItemDTO {
                            Amount = d.Amount,
                            DisbursementId = disbursement.Id,
                            Id = d.Id,
                            Label = d.Label
                        })
                    }
                });

            }
            catch (Exception ex)
            {
                return new JsonResult(new GetDisbursementDetailsResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
            }
        }

        [Route("ManualDisbursement")]
        [HttpPost]
        [ProducesResponseType(typeof(ManaulDisbursementResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> ManualDisbursement([FromBody] ManualDisbursementArgs args)
        {
            try
            {
                var result = await manualDisbursement.ExecuteAsync(new Services.Disbursement.Interactors.ManualDisbursementArgs {
                    DisbursementId = args.DisbursementId,
                    Remarks = args.Remarks
                });
                if (!result.Succeeded || result.Result == null)
                {
                    return new JsonResult(new ManaulDisbursementResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
                }

                return new JsonResult(new ManaulDisbursementResult
                {
                    IsSuccess = true,
                    Result = true
                });

            }
            catch (Exception ex)
            {
                return new JsonResult(new ManaulDisbursementResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
            }
        }

        [Route("CreateAnnouncement")]
        [HttpPost]
        [ProducesResponseType(typeof(CreateAnnouncementResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateAnnouncement([FromBody] CreateAnnouncementArgs args)
        {
            try
            {
                var result = await createAnnouncementHandler.ExecuteAsync(new Services.AdminService.Interactors.CreateAnnouncementArgs {
                    ButtonLabel = args.ButtonLabel ?? string.Empty,
                    Description = args.Description,
                    Link = args.Link ?? string.Empty,
                    Status = args.Status,
                    Title = args.Title,
                });

                if (!result.Succeeded || result.Result == null)
                {
                    return new JsonResult(new CreateAnnouncementResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
                }
                var created = result.Result;

                return new JsonResult(new CreateAnnouncementResult {
                    IsSuccess = true,
                    Result = new Framework.ApiCommand.ApiCore.DTO.Announcement.AnnouncementDTO {
                        ButtonLabel = created.ButtonLabel,
                        Description = created.Description,
                        Id = created.Id,
                        Link = created.Link,
                        Status = created.Status,
                        Title = created.Title
                    }
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new CreateAnnouncementResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
            }
        }

        [Route("UpdateAnnouncement")]
        [HttpPost]
        [ProducesResponseType(typeof(UpdateAnnouncementResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateAnnouncement([FromBody] UpdateAnnouncementArgs args)
        {
            try
            {
                var result = await updateAnnouncementHandler.ExecuteAsync(new Services.AdminService.Interactors.UpdateAnnouncementArgs {
                    ButtonLabel = args.ButtonLabel,
                    Description = args.Description,
                    Id = args.Id,
                    Link = args.Link,
                    Status = args.Status,
                    Title = args.Title
                });

                if (!result.Succeeded || result.Result == null)
                {
                    return new JsonResult(new UpdateAnnouncementResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
                }
                var created = result.Result;

                return new JsonResult(new UpdateAnnouncementResult {
                    IsSuccess = true,
                    Result = new Framework.ApiCommand.ApiCore.DTO.Announcement.AnnouncementDTO {
                        ButtonLabel = created.ButtonLabel,
                        Description = created.Description,
                        Id = created.Id,
                        Link = created.Link,
                        Status = created.Status,
                        Title = created.Title
                    }
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new UpdateAnnouncementResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
            }
        }

        [Route("GetAllAnnouncements")]
        [HttpGet]
        [ProducesResponseType(typeof(GetAllAnnouncementsResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllAnnouncements()
        {
            try
            {
                var result = await getAnnouncementsHandler.ExecuteAsync(new Services.AdminService.Interactors.GetAnnouncementsArgs {});

                if (!result.Succeeded || result.Result == null)
                {
                    return new JsonResult(new GetAllAnnouncementsResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
                }
                var announcements = result.Result.Announcements;

                return new JsonResult(new GetAllAnnouncementsResult {
                    IsSuccess = true,
                    Result = announcements.Select(a => new Framework.ApiCommand.ApiCore.DTO.Announcement.AnnouncementDTO {
                        ButtonLabel = a.ButtonLabel,
                        Description = a.Description,
                        Id = a.Id,
                        Link = a.Link,
                        Status = a.Status,
                        Title = a.Title
                    })
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new GetAllAnnouncementsResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
            }
        }

        [Route("DeleteAnnouncement")]
        [HttpPost]
        [ProducesResponseType(typeof(DeleteAnnouncementResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteAnnouncement([FromBody] DeleteAnnouncementArgs args)
        {
            try
            {
                var result = await deleteAnnouncementHandler.ExecuteAsync(new Services.AdminService.Interactors.DeleteAnnouncementArgs {
                    AnnouncementId = args.Id
                });

                if (!result.Succeeded || result.Result == null)
                {
                    return new JsonResult(new DeleteAnnouncementResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
                }
                var deleted = result.Result;

                return new JsonResult(new DeleteAnnouncementResult {
                    IsSuccess = true,
                    Result = deleted.AnnouncementId
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new DeleteAnnouncementResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
            }
        }

        [Route("EventPolicies")]
        [HttpPost]
        [ProducesResponseType(typeof(EventPoliciesResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> EventPolicies([FromBody] EventPoliciesArgs args)
        {
            try
            {
                var result = await updateDynamicContentHandler.ExecuteAsync(new Services.AdminService.Interactors.UpdateDynamicContentArgs {
                    Content = args.Content,
                    DateLastUpdated = DateTime.Now,
                    Description = "event policies description",
                    Identifier = "event-policies",
                    Title = args.Title
                });
                if (!result.Succeeded || result.Result == null)
                {
                    return new JsonResult(new EventPoliciesResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
                }

                return new JsonResult(new EventPoliciesResult
                {
                    IsSuccess = true,
                    Result = true
                });

            }
            catch (Exception ex)
            {
                return new JsonResult(new EventPoliciesResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
            }
        }

        [Route("EventBuyerPolicies")]
        [HttpPost]
        [ProducesResponseType(typeof(EventPoliciesResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> EventBuyerPolicies([FromBody] EventBuyerPoliciesArgs args)
        {
            try
            {
                var result = await updateDynamicContentHandler.ExecuteAsync(new Services.AdminService.Interactors.UpdateDynamicContentArgs {
                    Content = args.Content,
                    DateLastUpdated = DateTime.Now,
                    Description = "event policies description",
                    Identifier = "event-buyer-policies",
                    Title = "Event buyer policies"
                });
                if (!result.Succeeded || result.Result == null)
                {
                    return new JsonResult(new EventPoliciesResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
                }

                return new JsonResult(new EventPoliciesResult
                {
                    IsSuccess = true,
                    Result = true
                });

            }
            catch (Exception ex)
            {
                return new JsonResult(new EventPoliciesResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
            }
        }

        [Route("EventSellerPolicies")]
        [HttpPost]
        [ProducesResponseType(typeof(EventPoliciesResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> EventSellerPolicies([FromBody] EventSellerPoliciesArgs args)
        {
            try
            {
                var result = await updateDynamicContentHandler.ExecuteAsync(new Services.AdminService.Interactors.UpdateDynamicContentArgs {
                    Content = args.Content,
                    DateLastUpdated = DateTime.Now,
                    Description = "event policies description",
                    Identifier = "event-seller-policies",
                    Title = "Event seller policies"
                });
                if (!result.Succeeded || result.Result == null)
                {
                    return new JsonResult(new EventPoliciesResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
                }

                return new JsonResult(new EventPoliciesResult
                {
                    IsSuccess = true,
                    Result = true
                });

            }
            catch (Exception ex)
            {
                return new JsonResult(new EventPoliciesResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
            }
        }

        [Route("PrivacyPolicies")]
        [HttpPost]
        [ProducesResponseType(typeof(PrivacyPoliciesResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> PrivacyPolicies([FromBody] PrivacyPoliciesArgs args)
        {
            try
            {
                var result = await updateDynamicContentHandler.ExecuteAsync(new Services.AdminService.Interactors.UpdateDynamicContentArgs {
                    Content = args.Content,
                    DateLastUpdated = DateTime.Now,
                    Description = "privacy policies description",
                    Identifier = "privacy-policies",
                    Title = args.Title
                });
                if (!result.Succeeded || result.Result == null)
                {
                    return new JsonResult(new PrivacyPoliciesResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
                }

                return new JsonResult(new PrivacyPoliciesResult
                {
                    IsSuccess = true,
                    Result = true
                });

            }
            catch (Exception ex)
            {
                return new JsonResult(new PrivacyPoliciesResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
            }
        }
    }
}
