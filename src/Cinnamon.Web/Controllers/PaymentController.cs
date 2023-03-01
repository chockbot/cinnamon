using Cinnamon.Web.Models.Forms;
using Cinnamon.Web.Modules.ApiAccess.Handlers;
using Microsoft.AspNetCore.Mvc;

namespace Cinnamon.Web.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class PaymentController : ControllerBase 
{
    private readonly IPaymentApiHandler paymentApiHandler;

    public PaymentController(IPaymentApiHandler paymentApiHandler)
    {
        this.paymentApiHandler = paymentApiHandler;
    }

    [Route("VerifyCallback")]
    [HttpPost]
    public async Task<IActionResult> VerifyCallback([FromBody] PaymentVerifyCallback args)
    {
        try
        {
            var callbackToken = string.Empty;
            if(Request.Headers.TryGetValue("x-callback-token", out Microsoft.Extensions.Primitives.StringValues value))
            {
                callbackToken = value;
            }
            var result = await paymentApiHandler.VerifyCallback(new Framework.ApiCommand.ApiCore.Payment.Request.VerifyCallbackArgs {
                CallbackToken = callbackToken,
                Status = args.data.status,
                TransactionId = args.data.reference_id
            });

            if(!result.Succeeded || result.Result == null || !result.Result.IsSuccess)
            {
                return BadRequest(new {message = result.Result?.ErrorInfo?.Message ?? result.Message});
            }

            return Ok(new {message = result.Message, success = true});
        }
        catch
        {
            return BadRequest(new {message = "An error occured. Please try again", success = false});
        }
    }
}
