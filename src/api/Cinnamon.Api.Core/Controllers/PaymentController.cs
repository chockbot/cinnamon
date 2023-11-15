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
    private readonly IGetPaymentChannelsHandler getPaymentChannelsHandler;
    private readonly IVerifyPayoutCallbackHandler verifyPayoutCallbackHandler;

    public PaymentController(IVerifyCallbackHandler verifyCallbackHandler, IGetPaymentChannelsHandler getPaymentChannelsHandler,
        IVerifyPayoutCallbackHandler verifyPayoutCallbackHandler)
    {
        this.verifyCallbackHandler = verifyCallbackHandler;
        this.getPaymentChannelsHandler = getPaymentChannelsHandler;
        this.verifyPayoutCallbackHandler = verifyPayoutCallbackHandler;
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
                TransactionId = args.TransactionId,
                Payload = args.Payload
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

    [Route("GetPaymentChannels")]
    [HttpGet]
    [ProducesResponseType(typeof(GetPaymentChannelsResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPaymentChannels()
    {
        try
        {
            var result = await getPaymentChannelsHandler.ExecuteAsync(new Services.PaymentGatewayService.Interactors.GetPaymentChannelsArgs());
            if(!result.Succeeded || result.Result == null)
            {
                return BadRequest(new GetPaymentChannelsResult {ErrorInfo = new ErrorInfo {Message = result.Message}});
            }

            return new JsonResult(new GetPaymentChannelsResult 
                {
                    IsSuccess = true, 
                    Result = result.Result.PaymentChannels.Select(c => {
                        return new Framework.ApiCommand.ApiCore.DTO.Payment.PaymentChannelDTO {
                            Code = c.Code,
                            Name = c.Name
                        };
                    })
                }
            ); 
        }
        catch (Exception ex)
        {
            return BadRequest(new GetPaymentChannelsResult {ErrorInfo = new ErrorInfo {Message = ex.Message}});
        }
    }

    [Route("VerifyPayoutCallback")]
    [HttpPost]
    [ProducesResponseType(typeof(VerifyPayoutCallbackResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> VerifyPayoutCallback([FromBody] VerifyPayoutCallbackArgs args)
    {
        try
        {
            var result = await verifyPayoutCallbackHandler.ExecuteAsync(new Services.PaymentGatewayService.Interactors.VerifyPayoutCallbackArgs {
                CallbackToken = args.CallbackToken,
                FailureCode = args.FailureCode,
                ReferenceId = args.ReferenceId,
                Status = args.Status
            });
            if(!result.Succeeded || result.Result == null)
            {
                return BadRequest(new VerifyPayoutCallbackResult {ErrorInfo = new ErrorInfo {Message = result.Message}});
            }

            return new JsonResult(new VerifyPayoutCallbackResult 
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
            return BadRequest(new VerifyPayoutCallbackResult {ErrorInfo = new ErrorInfo {Message = ex.Message}});
        }
    }
}