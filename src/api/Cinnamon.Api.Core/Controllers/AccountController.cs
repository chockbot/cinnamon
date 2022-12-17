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
    private readonly ISubmitVerifyEmailHandler submitVerifyEmailHandler;
    private readonly ISubmitLoginHandler submitLoginHandler;

    public AccountController(ISubmitRegisterHandler submitRegisterHandler, ISubmitWaitlistHandler submitWaitlistHandler,
        ISubmitVerifyEmailHandler submitVerifyEmailHandler, ISubmitLoginHandler submitLoginHandler)
    {
        this.submitRegisterHandler = submitRegisterHandler;
        this.submitWaitlistHandler = submitWaitlistHandler;
        this.submitVerifyEmailHandler = submitVerifyEmailHandler;
        this.submitLoginHandler = submitLoginHandler;
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
                Email = args.Email,
                ValidationRoute = args.ValidationRoute
            });

            if(!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new RegisterWaitlistResult {ErrorInfo = new ErrorInfo {Message = result.Message}});
            }
            var created = result.Result;

            return new JsonResult(new RegisterWaitlistResult {
                Result = new VerificationLinkDTO {
                    Email = created.Email,
                    Guid = created.Guid,
                    Token = created.Token,
                    Id = created.Id,
                    VerificationLink = created.VerificationLink
                },
                IsSuccess = true
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new RegisterWaitlistResult {ErrorInfo = new ErrorInfo {Message = ex.Message}});
        }
    }

    [Route("VerifyRegisteredEmail")]
    [HttpPost]
    [ProducesResponseType(typeof(VerifyRegisteredEmailResult), StatusCodes.Status201Created)]
    [AllowAnonymous]
    public async Task<IActionResult> VerifyRegisteredEmail(VerifyRegisteredEmailArgs args)
    {
        try
        {
            var result = await submitVerifyEmailHandler.ExecuteAsync(new Services.AccountService.Interactors.SubmitVerifyEmailArgs {
                Token = args.Token,
                UserId = args.UserId
            });

            if(!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new VerifyRegisteredEmailResult {ErrorInfo = new ErrorInfo {Message = result.Message}});
            }
            var verified = result.Result;

            return new JsonResult(new VerifyRegisteredEmailResult {
                Result = new WaitlistDTO {
                    Email = verified.Email,
                    Guid = verified.Guid,
                    Token = verified.Token,
                    Id = verified.Id,
                    IsVerified = verified.IsVerified
                },
                IsSuccess = true
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new VerifyRegisteredEmailResult {ErrorInfo = new ErrorInfo {Message = ex.Message}});
        }
    }

    [Route("Login")]
    [HttpPost]
    [ProducesResponseType(typeof(VerifiedLoginResult), StatusCodes.Status201Created)]
    [AllowAnonymous]
    public async Task<IActionResult> Login(VerifiedLoginArgs args)
    {
        try
        {
            var result = await submitLoginHandler.ExecuteAsync(new Services.AccountService.Interactors.SubmitLoginArgs {
                Email = args.Email,
                Password = args.Password
            });

            if(!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new VerifiedLoginResult {ErrorInfo = new ErrorInfo {Message = result.Message}});
            }
            var verified = result.Result;

            return new JsonResult(new VerifiedLoginResult {
                Result = new VerifiedLoginDTO {
                    Email = verified.Email,
                    ExternalLogin = verified.ExternalLogin,
                    FirstName = verified.FirstName,
                    LastName = verified.LastName,
                    IsMaker = verified.IsMaker,
                    Id = verified.Id,
                    GeneratedToken = verified.GeneratedToken
                },
                IsSuccess = true
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new VerifiedLoginResult {ErrorInfo = new ErrorInfo {Message = ex.Message}});
        }
    }
}