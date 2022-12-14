using Microsoft.AspNetCore.Mvc;
using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Framework.ApiCommand.ApiCore.Account.Request;

namespace Cinnamon.Api.Core.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase 
{
    private readonly ISubmitRegisterHandler submitRegisterHandler;

    public AccountController(ISubmitRegisterHandler submitRegisterHandler)
    {
        this.submitRegisterHandler = submitRegisterHandler;
    }

    [Route("Register")]
    [HttpPost]
    public async Task<IActionResult> Register([FromBody] SubmitRegisterArgs args)
    {
        var result = await submitRegisterHandler.ExecuteAsync(args);
        return new JsonResult(result.Result);
    }
}