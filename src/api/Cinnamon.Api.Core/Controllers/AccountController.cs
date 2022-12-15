using Microsoft.AspNetCore.Mvc;
using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Framework.ApiCommand.ApiCore.Account.Request;
using Cinnamon.Framework.ApiCommand.ApiCore.Account.Response;
using Cinnamon.Framework.ApiCommand.ApiCore;
using Cinnamon.Framework.ApiCommand.ApiCore.DTO.Customer;
using Microsoft.AspNetCore.Authorization;

namespace Cinnamon.Api.Core.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AccountController : ControllerBase 
{
    private readonly ISubmitRegisterHandler submitRegisterHandler;

    public AccountController(ISubmitRegisterHandler submitRegisterHandler)
    {
        this.submitRegisterHandler = submitRegisterHandler;
    }

    [Route("Register")]
    [HttpPost]
    [ProducesResponseType(typeof(SubmitRegisterResult), StatusCodes.Status201Created)]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] SubmitRegisterArgs args)
    {
        try
        {
            var result = await submitRegisterHandler.ExecuteAsync(new Services.AccountService.Interactors.SubmitRegisterArgs {
                Birthdate = args.Birthdate,
                Email = args.Email,
                ExternalLogin = args.ExternalLogin,
                FirstName = args.FirstName,
                LastName = args.LastName,
                Password = args.Password,
                ProfilePath = args.ProfilePath
            });

            if(!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new SubmitRegisterResult {ErrorInfo = new ErrorInfo {Message = result.Message}});
            }
            var objResult = result.Result;

            return new JsonResult(new SubmitRegisterResult {
                Result = new CustomerDTO {
                    Birthdate = objResult.Birthdate,
                    Email = objResult.Email,
                    ExternalLogin = objResult.ExternalLogin,
                    FirstName = objResult.FirstName,
                    LastName = objResult.LastName,
                    ProfileImg = objResult.ProfileImg,
                    Id = objResult.Id
                },
                IsSuccess = true,
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new SubmitRegisterResult {ErrorInfo = new ErrorInfo {Message = ex.Message}});
        }
    }
}