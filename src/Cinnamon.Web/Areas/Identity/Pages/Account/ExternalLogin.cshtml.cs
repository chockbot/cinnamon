// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel;

namespace Cinnamon.Web.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ExternalLoginModel : PageModel
    {
        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ProviderDisplayName { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string ErrorMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>

            public int Id { get; set; }
            [Required]
            [EmailAddress]
            [DisplayName("Email")]
            public string Email { get; set; }
            [Required]
            [DisplayName("First Name")]
            public string FirstName { get; set; }
            [Required]
            [DisplayName("Last Name")]
            public string LastName { get; set; }
            [Required]
            [DisplayName("Birthdate")]
            public DateTime? BirthDate { get; set; } = null;
            //public UserType UserType { get; set; }
        }
        
        public IActionResult OnGet() => RedirectToPage("./Login");

        public IActionResult OnPost(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Page("./ExternalLogin", pageHandler: "Callback", values: new { });
            // var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult();
        }

        public async Task<IActionResult> OnGetCallbackAsync(string returnUrl = null, string remoteError = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/Explore");
            if (remoteError != null)
            {
                ErrorMessage = $"Error from external provider: {remoteError}";
                return LocalRedirect("/Error");
            }

            // Sign in the user with this external login provider if the user already has a login.
            // var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
           
           return Redirect("/Creation");

            // if (result.Succeeded)
            // {
            //     //var user = CreateUser();
            //     var user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
            //     var props = new AuthenticationProperties();
            //     props.StoreTokens(info.AuthenticationTokens);
            //     await _signInManager.SignInAsync(user, props, info.LoginProvider);
            //     _logger.LogInformation("{Name} logged in with {LoginProvider} provider.", info.Principal.Identity.Name, info.LoginProvider);
            //     if (userType == UserType.Maker)
            //     {
            //         return Redirect("/Creation");
            //     }
            //     else
            //     {
            //         return Redirect(returnUrl);
            //     }
            // }
            // else
            // {
            //     // If the user does not have an account, then ask the user to create an account.
            //     ReturnUrl = returnUrl;
            //     ProviderDisplayName = info.ProviderDisplayName;
            //     if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
            //     {
            //         Input = new InputModel
            //         {
            //             Email = info.Principal.FindFirstValue(ClaimTypes.Email),
            //             FirstName = info.Principal.FindFirstValue(ClaimTypes.GivenName),
            //             LastName = info.Principal.FindFirstValue(ClaimTypes.Surname),
            //             UserType = userType
            //         };
            //     }
            //     return Page();
            // }
        }

        public async Task<IActionResult> OnPostConfirmationAsync(string returnUrl = null)
        {
            // Get the information about the user from the external login provider
            // var info = await _signInManager.GetExternalLoginInfoAsync();
            // if (info == null)
            // {
            //     ErrorMessage = "Error loading external login information during confirmation.";
            //     return LocalRedirect("/Error");
            // }

            // if (ModelState.IsValid)
            // {
            //     var user = CreateUser();
            //     await _emailStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
            //     await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
            //     await _emailStore.SetEmailConfirmedAsync(user, true, CancellationToken.None);
            //     var result = await _userManager.CreateAsync(user);

            //     if (result.Succeeded)
            //     {
            //         result = await _userManager.AddLoginAsync(user, info);
            //         if (result.Succeeded)
            //         {
            //             var userId = await _userManager.GetUserIdAsync(user);
            //             // Include the access token in the properties
            //             var props = new AuthenticationProperties();
            //             props.StoreTokens(info.AuthenticationTokens);
            //             await CoreDI.DataStore.Customers.SaveDataAsync(new CustomerModel() { UserId = userId, ProfilePath = "/images/Profile/user.png", FirstName = Input.FirstName, LastName = Input.LastName, Birthdate = Input.BirthDate.ToString(), Email = Input.Email, ExternalLogin = true, IsMaker = Input.UserType == UserType.Maker ? true : false, About = "", DateJoined = "" });
            //             await _signInManager.SignInAsync(user, props, authenticationMethod: info.LoginProvider);
            //             _logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);
            //         }
            //     }
            // }
            
            // if(Input.UserType == UserType.Maker)
            // {

            //     return Redirect("/Creation");
            // }
            // else
            // {

            //     return Redirect(returnUrl);
            // }

            return Redirect("/Creation");
        }
    }
}
