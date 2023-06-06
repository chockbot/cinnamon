using Cinnamon.Api.Core.Services.ActivityService;
using Cinnamon.Api.Core.Services.AdminService.Handlers;
using Cinnamon.Framework.ApiCommand.ApiCore;
using Cinnamon.Framework.ApiCommand.ApiCore.AdminUser.Request;
using Cinnamon.Framework.ApiCommand.ApiCore.AdminUser.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        private readonly ILogger _logger;

        public AdminController(IGetAdminUserByEmailHandler getAdminUserByEmailHandler, ILogger<AdminController> logger,
            IUpdateCustomerPricingHandler updateCustomerPricingHandler)
        {
            _logger = logger;

            this.getAdminUserByEmailHandler = getAdminUserByEmailHandler;
            this.updateCustomerPricingHandler = updateCustomerPricingHandler;
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
                    IsManualPayment = args.IsManualPayment
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
    }
}
