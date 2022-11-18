// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System.ComponentModel;
using Cinnamon.Core;
using Cinnamon.Core.Enums;

namespace Cinnamon.Web.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ExternalLoginModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserStore<IdentityUser> _userStore;
        private readonly IUserEmailStore<IdentityUser> _emailStore;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<ExternalLoginModel> _logger;

        public ExternalLoginModel(
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            IUserStore<IdentityUser> userStore,
            ILogger<ExternalLoginModel> logger,
            IEmailSender emailSender)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _logger = logger;
            _emailSender = emailSender;
        }

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
            public UserType UserType { get; set; }
        }
        
        public IActionResult OnGet() => RedirectToPage("./Login");

        public IActionResult OnPost(string provider, UserType userType, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Page("./ExternalLogin", pageHandler: "Callback", values: new { userType, returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }

        public async Task<IActionResult> OnGetCallbackAsync(UserType userType,string returnUrl = null, string remoteError = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/Explore");
            if (remoteError != null)
            {
                ErrorMessage = $"Error from external provider: {remoteError}";
                return LocalRedirect("/Error");
            }
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ErrorMessage = "Error loading external login information.";
                return LocalRedirect("/Error");
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await CoreDI.DataStore.User.GetAllAsync();
            var userData = result.Where(x => x.Email.Equals(info.Principal.FindFirstValue(ClaimTypes.Email).ToString())).FirstOrDefault();
            if (userData != null)
            {
                var user = CreateUser();

                await _emailStore.SetUserNameAsync(user, userData.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, userData.Email, CancellationToken.None);

                await _signInManager.SignInAsync(user, isPersistent: false, info.LoginProvider);
                _logger.LogInformation("{Name} logged in with {LoginProvider} provider.", info.Principal.Identity.Name, info.LoginProvider);
                if (userType == UserType.Maker)
                {
                    return Redirect("/Creation");
                }
                else
                {
                    return Redirect("/Explore");
                }
            }
            else
            {
                // If the user does not have an account, then ask the user to create an account.
                ReturnUrl = returnUrl;
                ProviderDisplayName = info.ProviderDisplayName;
                if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
                {
                    Input = new InputModel
                    {
                        Email = info.Principal.FindFirstValue(ClaimTypes.Email),
                        FirstName = info.Principal.FindFirstValue(ClaimTypes.GivenName),
                        LastName = info.Principal.FindFirstValue(ClaimTypes.Surname),
                        UserType = userType
                    };
                }
                return Page();
            }
        }

        public async Task<IActionResult> OnPostConfirmationAsync(string returnUrl = null)
        {
            // Get the information about the user from the external login provider
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ErrorMessage = "Error loading external login information during confirmation.";
                return LocalRedirect("/Error");
            }

            if (ModelState.IsValid)
            {
           

                try
                {
                    var result = await CoreDI.DataStore.User.SaveDataAsync(new UserListModel() { FirstName = Input.FirstName, LastName = Input.LastName, Birthdate = Input.BirthDate.ToString(), Email = Input.Email, Type = Core.Enums.UserType.Customer, Password = info.LoginProvider, ConfirmPassword = info.LoginProvider, ExternalLogin = true, isMaker = Input.UserType == UserType.Maker ? true:false });
                    var user = CreateUser();

                    await _emailStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                    await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                    await _signInManager.SignInAsync(user, isPersistent: false, info.LoginProvider);
                }
                catch(Exception ex)
                {
                    return LocalRedirect("/Error");
                }
            }
            
            if(Input.UserType == UserType.Maker)
            {

                return Redirect("/Creation");
            }
            else
            {

                return Redirect("/Explore");
            }
        }

        private IdentityUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<IdentityUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
                    $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the external login page in /Areas/Identity/Pages/Account/ExternalLogin.cshtml");
            }
        }

        private IUserEmailStore<IdentityUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<IdentityUser>)_userStore;
        }
    }
}
