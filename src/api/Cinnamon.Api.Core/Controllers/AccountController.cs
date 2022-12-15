using Microsoft.AspNetCore.Mvc;
using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Framework.ApiCommand.ApiCore.Account.Request;
using Cinnamon.Framework.ApiCommand.ApiCore.Account.Response;
using Cinnamon.Framework.ApiCommand.ApiCore;
using Cinnamon.Framework.ApiCommand.ApiCore.DTO.Customer;
using Cinnamon.Framework.ApiCommand.ApiCore.DTO.Waitlist;
using Microsoft.AspNetCore.Authorization;

namespace Cinnamon.Api.Core.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AccountController : ControllerBase 
{
    private readonly ISubmitRegisterHandler submitRegisterHandler;
    private readonly ISubmitWaitlistHandler submitWaitlistHandler;

    public AccountController(ISubmitRegisterHandler submitRegisterHandler, ISubmitWaitlistHandler submitWaitlistHandler)
    {
        this.submitRegisterHandler = submitRegisterHandler;
        this.submitWaitlistHandler = submitWaitlistHandler;
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

    [Route("RegisterWaitlist")]
    [HttpPost]
    [ProducesResponseType(typeof(RegisterWaitlistResult), StatusCodes.Status201Created)]
    [AllowAnonymous]
    public async Task<IActionResult> RegisterWaitlist(RegisterWaitlistArgs args)
    {
        try
        {
            var result = await submitWaitlistHandler.ExecuteAsync(new Services.AccountService.Interactors.SubmitWaitlistArgs{
                Email = args.Email
            });

            if(!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new RegisterWaitlistResult {ErrorInfo = new ErrorInfo {Message = result.Message}});
            }
            var created = result.Result;

            return new JsonResult(new RegisterWaitlistResult {
                Result = new WaitlistDTO {
                    Email = created.Email,
                    Guid = created.Guid,
                    Token = created.Token,
                    Id = created.Id
                },
                IsSuccess = true
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new RegisterWaitlistResult {ErrorInfo = new ErrorInfo {Message = ex.Message}});
        }
    }
}