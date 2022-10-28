using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Cinnamon.Core.Common;
using Cinnamon.Core.Config;
using Cinnamon.Core.Module.NotificationService.Interactors;
using Cinnamon.Core.Module.NotificationService.Handler;
using Cinnamon.Core.Module.CinnamonMakerService.Interactors;
using Cinnamon.Core.Module.CinnamonMakerService.Interactors.Results;

namespace Cinnamon.Core.Module.CinnamonMakerService.Handler.Register;

public class RegisterMakerHandler : IRegisterMaker
{
    private readonly CoreConfig coreConfig;
    private readonly IEmailVerification emailVerification;
    private readonly UserManager<IdentityUser> usermanager;
    private readonly IUserStore<IdentityUser> userStore;
    private readonly IUserEmailStore<IdentityUser> emailStore;

    public RegisterMakerHandler(IEmailVerification emailVerification,
        UserManager<IdentityUser> userManager, IUserStore<IdentityUser> userStore,
        IUserEmailStore<IdentityUser> emailStore, CoreConfig coreConfig)
    {
        this.coreConfig = coreConfig;
        this.emailVerification = emailVerification;
        this.usermanager = userManager;
        this.userStore = userStore;
        this.emailStore = emailStore;
    }

    public AppResult<RegisterMakerResult> Execute(RegisterMaker args)
    {
        throw new NotImplementedException();
    }

    public async Task<AppResult<RegisterMakerResult>> ExecuteAsync(RegisterMaker args)
    {
        try
        {
            var user = CreateUser();

            await userStore.SetUserNameAsync(user, args.Username, CancellationToken.None);
            await emailStore.SetEmailAsync(user, args.Email, CancellationToken.None);
            var createUserResult = await usermanager.CreateAsync(user, args.Password);

            if(!createUserResult.Succeeded)
            {
                return AppResult<RegisterMakerResult>
                    .CreateFailed(new ApplicationException("An error occured when trying to create user"), 
                        "An error occured in RegisterMakerHandler");
            }

            var userId = await usermanager.GetUserIdAsync(user);
            var token = await usermanager.GenerateEmailConfirmationTokenAsync(user);

            token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            var verificationLink = $"{coreConfig.BaseUrl}/Account/ConfirmEmail/?userid={userId}&token={token}";
            var emailVerificationResult = await emailVerification.ExecuteAsync(new EmailVerification {Email = args.Email, VerificationLink = verificationLink});

            return AppResult<RegisterMakerResult>
                .CreateSucceeded(new RegisterMakerResult { GeneratedVerificationLink = verificationLink }, "Verification link successfully sent");

        }
        catch(Exception ex)
        {
            return AppResult<RegisterMakerResult>.CreateFailed(ex, "An error occured in RegisterMakerHandler");
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
                $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
        }
    }
}