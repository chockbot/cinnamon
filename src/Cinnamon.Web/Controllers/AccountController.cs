using Cinnamon.Web.Models.Account;
using Cinnamon.Web.Models.Forms;
using Cinnamon.Web.Modules.ApiAccess.Handlers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Cinnamon.Web.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class AccountController : Controller 
{
    private readonly IAccountApiHandler accountApiHandler;
    private readonly Cinnamon.Web.Config.Config config;

    public AccountController(IAccountApiHandler accountApiHandler, Cinnamon.Web.Config.Config config)
    {
        this.accountApiHandler = accountApiHandler;
        this.config = config;
    }

    [Route("login")]
    [HttpPost]
    public async Task<IActionResult> Login(LoginModel model)
    {
        try
        {
            if(!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Please provide valid email or password" });
            }

            // login to api
            var loginResult = await accountApiHandler.Login(new Framework.ApiCommand.ApiCore.Account.Request.VerifiedLoginArgs {
                Email = model.Email,
                Password = model.Password
            });

            if(!loginResult.Succeeded || loginResult.Result == null)
            {
                return Json(new { success = false, message = "An error occured please try again later" });
            }

            if(loginResult.Succeeded && !loginResult.Result.IsSuccess)
            {
                return Json(new { success = false, message = loginResult.Result.ErrorInfo?.Message });
            }

            var claims = new List<Claim>
            {
                new Claim("Email", loginResult.Result.Result.Email),
                new Claim("Token", loginResult.Result.Result.GeneratedToken),
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties {IsPersistent = true};

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

            return Json(new { success = true, message = "Successfully login" });
        }
        catch
        {
            return Json(new { success = false, message = "An error occured please try again later" });
        }
    }

    [Route("register")]
    [HttpPost]
    public async Task<IActionResult> Register(RegisterModel model)
    {
        try
        {
            if(!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Please provide valid email" });
            }

            var registerWaitlist = await accountApiHandler.RegisterWaitlist(new Framework.ApiCommand.ApiCore.Account.Request.RegisterWaitlistArgs {
                Email = model.Email,
                ValidationRoute = config.BaseUrl + "/confirm-email"
            });

            if(!registerWaitlist.Succeeded || registerWaitlist.Result == null)
            {
                return Json(new { success = false, message = "An error occured please try again later" });
            }

            if(registerWaitlist.Succeeded && !registerWaitlist.Result.IsSuccess)
            {
                switch(registerWaitlist.Result.ErrorInfo?.Code)
                {
                    case "EMAIL-ALREADY-REGISTERED":
                        return Json(new { success = true, message = "Email already verified", code = "VERIFIED" });
                    case "EMAIL-ALREADY-REGISTERED-NOT-VERIFIED":
                        return Json(new { success = true, message = "Email not yet verified", code = "NOTVERIFIED" });
                    case "EMAIL-ALREADY-REGISTERED-VERIFIED":
                        return Json(new { success = true, message = "Email already verified but not yet registered", code = "NOTREGISTERED" });
                    default:
                        return Json(new { success = false, message = "An error occured please try again later", code = "ERRORREGISTER" });
                }
            }

            return Json(new { success = true, message = "Please confirm your email to proceed.", code = "EMAILREGISTERED" });
        }
        catch
        {
            return Json(new { success = false, message = "An error occured please try again later" });
        }
    }

    [Route("registerautologin")]
    [HttpPost]
    public async Task<IActionResult> RegisterAutoLogin(RegisterAutoLogin model)
    {
        try
        {
            if(!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Please provide required fields" });
            }

            var registerResult = await accountApiHandler.Register(new Framework.ApiCommand.ApiCore.Account.Request.SubmitRegisterArgs {
                Birthdate = model.Birthdate,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Password = model.Password,
                ProfilePath = "/images/Profile/user.png",
                IsMaker = false,
            });

            if(!registerResult.Succeeded || registerResult.Result == null)
            {
                return Json(new { success = false, message = "An error occured please try again later" });
            }

            if(registerResult.Succeeded && !registerResult.Result.IsSuccess)
            {
                return Json(new { success = false, message = registerResult.Result.ErrorInfo?.Message });
            }

            // auto login
            var loginResult = await accountApiHandler.Login(new Framework.ApiCommand.ApiCore.Account.Request.VerifiedLoginArgs {
                Email = model.Email,
                Password = model.Password
            });

            if(!loginResult.Succeeded || loginResult.Result == null)
            {
                return Json(new { success = false, message = "An error occured please try again later" });
            }

            if(loginResult.Succeeded && !loginResult.Result.IsSuccess)
            {
                return Json(new { success = false, message = loginResult.Result.ErrorInfo?.Message });
            }

            var claims = new List<Claim>
            {
                new Claim("Email", loginResult.Result.Result.Email),
                new Claim("Token", loginResult.Result.Result.GeneratedToken),
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties {IsPersistent = true};

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

            return Json(new { success = true, message = "Successfully registered" });
        }
        catch 
        {
            return Json(new { success = false, message = "An error occured please try again later" });
        }
    }

    [Route("UploadGovernmentIds")]
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> UploadGovernmentIds([FromForm] UploadGovernmentIds args)
    {
        try
        {
            if(!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Please provide required fields" });
            }

            var token = User.FindFirstValue("Token");
            if(token == null)
            {
                return Json(new { success = false, message = "Unable to identify current user" });
            }

            var result = await accountApiHandler.UploadGovernmentIds(new Framework.ApiCommand.ApiCore.Account.Request.UploadGovernmentIdsArgs {
                BackImageId = args.BackId,
                FrontImageId = args.FrontId
            }, token);

            if(!result.Succeeded || result.Result == null)
            {
                return Json(new { success = false, message = result.Message });
            }

            if(result.Succeeded && !result.Result.IsSuccess)
            {
                return Json(new { success = false, message = result.Result.ErrorInfo?.Message });
            }

            return Json(new { success = true, message = "Successfully uploaded" });
        }
        catch
        {
            return Json(new { success = false, message = "An error occured please try again later" });
        }
    }
}