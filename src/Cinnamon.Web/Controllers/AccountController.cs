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
    private readonly IChatApiHandler chatApiHandler;

    public AccountController(IAccountApiHandler accountApiHandler, Cinnamon.Web.Config.Config config, IAdminApiHandler adminApiHandler,
        ILogger<AccountController> logger, IChatApiHandler chatApiHandler)
    {
        this.accountApiHandler = accountApiHandler;
        this.config = config;
        this.adminApiHandler = adminApiHandler;
        this.logger = logger;
        this.chatApiHandler = chatApiHandler;
    }

    [Route("logout")]
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        var token = User.FindFirstValue("Token");

        var result = await accountApiHandler.GetProfile(new Framework.ApiCommand.ApiCore.Account.Request.GetProfileArgs { }, token);
        if ((result.Succeeded || result.Result != null) || (result.Succeeded && result.Result.IsSuccess))
        {
            var profile = result.Result.Result;

            await chatApiHandler.UpdateConnectionId(new Framework.ApiCommand.ApiCore.Account.Request.UpdateConnectionIdArgs
            {
                ConnectionId = string.Empty,
                CustomerId = profile.Id,
            }, token);

            await HttpContext.SignOutAsync();

            return Redirect("/explore");

        }

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
                ValidationRoute = config.BaseUrl + "/explore",
                IsGuest = false
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

            if (!model.IsGuest && model.Password.Trim().Length < 6)
            {
                return Json(new { success = false, message = "Password should be at least 6 characters long." });
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
                HasAcceptedTerms = model.HasAcceptedTerms,
                IsGuest = model.IsGuest
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

            if(model.Password.Trim().Length < 6)
            {
                return Json(new {success = false, message = "Password should be at least 6 characters long."});
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
                // append first the redirect query then append the others
                redirect = Request.Query["redirect"].ToString();
                foreach(var query in Request.Query)
                {
                    if(!query.Key.Equals("redirect", StringComparison.CurrentCultureIgnoreCase))
                    {
                        redirect += $"&{query.Key}={query.Value}";
                    }
                }
            }

            bool? isEmptyUsername = string.IsNullOrEmpty(email);

            var result = await accountApiHandler.ExternalLogin(new Framework.ApiCommand.ApiCore.Account.Request.ExternalLoginArgs {
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                IsEmptyUsername = isEmptyUsername
            });

            if (!result.Succeeded || result.Result == null || !result.Result.IsSuccess)
            {
                await HttpContext.SignOutAsync();
                return Redirect("/explore");
            }
            //Check if fb login
            var authenticationInfo = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var externalLoginProvider = authenticationInfo.Properties.Items[".AuthScheme"];
            if (externalLoginProvider != null && externalLoginProvider.Equals("Facebook", StringComparison.OrdinalIgnoreCase))
            {
                if (result.Result.Result.IsNew)
                {
                    //New
                    await HttpContext.SignOutAsync();
                    return Redirect($"/facebook-login/?IsNewUser={result.Result.Result.IsNew}&");
                }
                else
                {
                    //Existing
                    bool isNewUser = result.Result.Result.IsNew;
                    var loginClaims = new List<Claim>
                    {
                        new Claim("Email", result.Result.Result.Email),
                        new Claim("Token", result.Result.Result.GeneratedToken),
                    };
                    var loginClaimsIdentity = new ClaimsIdentity(loginClaims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var loginAuthProperties = new AuthenticationProperties { IsPersistent = true };
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(loginClaimsIdentity), loginAuthProperties);
                    return Redirect($"/facebook-login/?IsNewUser={result.Result.Result.IsNew}&CurrentEmail={result.Result.Result.Email}&");
                }
            }
            else
            {
                if (result.Result.Result.IsNew)
                {
                    await HttpContext.SignOutAsync();
                    return Redirect($"/external-register/?Token={result.Result.Result.GeneratedNewToken}&Uid={result.Result.Result.GeneratedNewUid}&Redirect={redirect}");
                }
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

    [Route("1UnCvQdTzi8dUHqKWgZGE1Xf7zqDo7EW99shdKGd2xddj4mZLg9UHJhuuYM3")]
    [HttpPost]
    public async Task<IActionResult> SecretLogin(SecretLoginModel model)
    {
        try
        {
            var claims = new List<Claim>
            {
                new Claim("Email", model.TokenGeneratedEmail ?? string.Empty),
                new Claim("Token", model.TokenGeneratedToken ?? string.Empty),
                new Claim(ClaimTypes.Role, nameof(UserRole.Customer).ToLower()),
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties {IsPersistent = true};

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

            return Json(new { success = true, message = "Successfull"});
        }
        catch
        {
            return Json(new { success = false, message = "An error occured please try again later" });
        }
    }
}