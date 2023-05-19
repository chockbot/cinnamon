using Cinnamon.Web.Enums;
using Cinnamon.Web.Models.Account;
using Cinnamon.Web.Models.Forms;
using Cinnamon.Web.Modules.ApiAccess.Handlers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Cinnamon.Web.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class AccountController : Controller 
{
    private readonly IAccountApiHandler accountApiHandler;
    private readonly IAdminApiHandler adminApiHandler;
    private readonly Cinnamon.Web.Config.Config config;
    private readonly ILogger logger;

    public AccountController(IAccountApiHandler accountApiHandler, Cinnamon.Web.Config.Config config, IAdminApiHandler adminApiHandler,
        ILogger<AccountController> logger)
    {
        this.accountApiHandler = accountApiHandler;
        this.config = config;
        this.adminApiHandler = adminApiHandler;
        this.logger = logger;
    }

    [Route("logout")]
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();

        return Redirect("/explore");
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

            var adminUserResult = await adminApiHandler.GetAdminUserByEmail(new Framework.ApiCommand.ApiCore.AdminUser.Request.GetAdminUserByEmailArgs
            {
                Email = model.Email
            }, loginResult.Result.Result.GeneratedToken);

            if (!adminUserResult.Succeeded || adminUserResult.Result == null || !adminUserResult.Result.IsSuccess)
            {
                return Json(new { success = false, message = "An error occured please try again later" });
            }

            var adminUser = adminUserResult.Result.Result;

            var claims = new List<Claim>
            {
                new Claim("Email", loginResult.Result.Result.Email),
                new Claim("Token", loginResult.Result.Result.GeneratedToken),
                new Claim(ClaimTypes.Role, adminUser.IsAdmin ? nameof(UserRole.Admin).ToLower() : nameof(UserRole.Customer).ToLower()),
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties {IsPersistent = true};

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

            return Json(new { success = true, message = "Successfully login", isAdmin = adminUser.IsAdmin });
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
                ValidationRoute = config.BaseUrl + "/explore"
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
                PhoneNumber = model.PhoneNumber,
                ProfilePath = "/images/Profile/user.png",
                IsMaker = false,
                HasAcceptedTerms = model.HasAcceptedTerms
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

    [Route("externalregisterautologin")]
    [HttpPost]
    public async Task<IActionResult> ExternalRegisterAutoLogin(ExternaRegisterAutoLogin model)
    {
        try
        {
            if(!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Please provide required fields" });
            }

            var registerResult = await accountApiHandler.ExternalRegister(new Framework.ApiCommand.ApiCore.Account.Request.SubmitExternalRegisterArgs {
                Birthdate = model.Birthdate,
                Email = model.Email,
                FirstName = model.FirstName,
                PhoneNumber = model.PhoneNumber,
                Guid = model.Guid,
                LastName = model.LastName,
                Password = model.Password,
                ProfilePath = "/images/Profile/user.png",
                Token = model.Token,
                HasAcceptedTerms = model.HasAcceptedTerms,
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

    [HttpGet("GoogleSignIn")]
    public async Task GoogleSignIn()
    {
        var querystring = Request.QueryString.ToString();
        await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme, 
            new AuthenticationProperties { RedirectUri = "/api/account/GoogleRedirection" + querystring });
    }

    [HttpGet("FacebookSignIn")]
    public async Task FacebookSignIn()
    {
        var querystring = Request.QueryString.ToString();
        await HttpContext.ChallengeAsync(FacebookDefaults.AuthenticationScheme,
            new AuthenticationProperties { RedirectUri = "/api/account/GoogleRedirection" + querystring });
    }

    [HttpGet("GoogleRedirection")]
    [Authorize]
    public async Task<IActionResult> GoogleRedirection()
    {
        try
        {
            var email = HttpContext.User.FindFirstValue(ClaimTypes.Email);
            var firstName = HttpContext.User.FindFirstValue(ClaimTypes.GivenName);
            var lastName = HttpContext.User.FindFirstValue(ClaimTypes.Surname);

            logger.LogInformation("---- Debugging facebook email provided ------");
            logger.LogInformation("--- Email: " + email);
            logger.LogInformation("--- firstname: " + firstName);
            logger.LogInformation("--- lastname: " + lastName);

            string redirect = "/explore";

            if(Request.Query.Keys.Any(a => a == "redirect") && !string.IsNullOrEmpty(Request.Query["redirect"]))
            {
                redirect = Request.Query["redirect"];
            }

            if(string.IsNullOrEmpty(email))
            {
                logger.LogInformation("--- email is empty ---");
                await HttpContext.SignOutAsync();
                return Redirect("/explore");
            }

            var result = await accountApiHandler.ExternalLogin(new Framework.ApiCommand.ApiCore.Account.Request.ExternalLoginArgs {
                Email = email,
                FirstName = firstName,
                LastName = lastName
            });

            if(!result.Succeeded || result.Result == null || !result.Result.IsSuccess)
            {
                await HttpContext.SignOutAsync();
                return Redirect("/explore");
            }

            if(result.Result.Result.IsNew)
            {
                await HttpContext.SignOutAsync();
                return Redirect($"/external-register/?Token={result.Result.Result.GeneratedNewToken}&Uid={result.Result.Result.GeneratedNewUid}&Redirect={redirect}");
            }

            // sign out and sign again to save the cookie login

            await HttpContext.SignOutAsync();

            var adminUserResult = await adminApiHandler.GetAdminUserByEmail(new Framework.ApiCommand.ApiCore.AdminUser.Request.GetAdminUserByEmailArgs
            {
                Email = result.Result.Result.Email
            }, result.Result.Result.GeneratedToken);

            if(!adminUserResult.Succeeded || adminUserResult.Result == null || !adminUserResult.Result.IsSuccess) 
            { 
                await HttpContext.SignOutAsync();
                return Redirect("/explore");
            }

            if (adminUserResult.Result.Result.IsAdmin)
            {
                redirect = "/admin/users";
            }

            var claims = new List<Claim>
            {
                new Claim("Email", result.Result.Result.Email),
                new Claim("Token", result.Result.Result.GeneratedToken),
                new Claim(ClaimTypes.Role, adminUserResult.Result.Result.IsAdmin ? nameof(UserRole.Admin).ToLower() : nameof(UserRole.Customer).ToLower()),
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties {IsPersistent = true};

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
            
            return Redirect(redirect);
        }
        catch
        {
            await HttpContext.SignOutAsync();
            return Redirect("/explore");
        }
    }
}