using Cinnamon.Api.Core.Services.PaymentGatewayService.Handlers;
using Cinnamon.Framework.ApiCommand.ApiCore;
using Cinnamon.Framework.ApiCommand.ApiCore.Payment.Request;
using Cinnamon.Framework.ApiCommand.ApiCore.Payment.Response;
using Microsoft.AspNetCore.Mvc;

namespace Cinnamon.Api.Core.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PaymentController : ControllerBase 
{
    private readonly IVerifyCallbackHandler verifyCallbackHandler;

    public PaymentController(IVerifyCallbackHandler verifyCallbackHandler)
    {
        this.verifyCallbackHandler = verifyCallbackHandler;
    }

    [Route("VerifyCallback")]
    [HttpPost]
    [ProducesResponseType(typeof(VerifyCallbackResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> VerifyCallback([FromBody] VerifyCallbackArgs args)
    {
        try
        {
            var result = await verifyCallbackHandler.ExecuteAsync(new Services.PaymentGatewayService.Interactors.VerifyCallbackArgs {
                CallbackToken = args.CallbackToken,
                Status = args.Status,
                TransactionId = args.TransactionId
            });
            if(!result.Succeeded || result.Result == null)
            {
                return BadRequest(new VerifyCallbackResult {ErrorInfo = new ErrorInfo {Message = result.Message}});
            }

            return new JsonResult(new VerifyCallbackResult 
                {
                    IsSuccess = true, 
                    Result = new Framework.ApiCommand.ApiCore.DTO.Payment.VerifyCallbackDTO {
                        Message = "Ok"
                    }
                }
            ); 
        }
        catch (Exception ex)
        {
            return BadRequest(new VerifyCallbackResult {ErrorInfo = new ErrorInfo {Message = ex.Message}});
        }
    }
}