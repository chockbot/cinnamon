using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Cinnamon.Web.Modules.ApiAccess.Handlers;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace Cinnamon.Web.Areas.Identity.Pages.Account;

public class Onboarding : PageModel
{
    private readonly IAccountApiHandler accountApiHandler;

    public Onboarding(IAccountApiHandler accountApiHandler)
    {
        this.accountApiHandler = accountApiHandler;
    }

    [BindProperty]
    public InputModel Input { get; set;}

    public async Task<IActionResult> OnGetAsync()
    {
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if(ModelState.IsValid)
        {
            // login to api
            var loginResult = await accountApiHandler.Login(new Framework.ApiCommand.ApiCore.Account.Request.VerifiedLoginArgs {
                Email = Input.Email,
                Password = Input.Password
            });

            if(!loginResult.Succeeded || loginResult.Result == null)
            {
                ModelState.AddModelError(string.Empty, "An error occured please try again later");
                return Page();
            }

            if(loginResult.Succeeded && !loginResult.Result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, loginResult.Result.ErrorInfo?.Message);
                return Page();
            }

            var claims = new List<Claim>
            {
                new Claim("Email", loginResult.Result.Result.Email),
                new Claim("Token", loginResult.Result.Result.GeneratedToken),
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties {IsPersistent = true};

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

            return Redirect("/Creation");
        }
        return Page();
    }

    public class InputModel 
    {
        [Required]
        public string Password { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}