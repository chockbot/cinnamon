using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Cinnamon.Core.Common;
using Cinnamon.Core.Config;
using Cinnamon.Core.Module.NotificationService.Interactors;
using Cinnamon.Core.Module.NotificationService.Handler;
using Cinnamon.Core.Module.CinnamonMakerService.Interactors;
using Cinnamon.Core.Module.CinnamonMakerService.Interactors.Results;

namespace Cinnamon.Core.Module.CinnamonMakerService.Handler.EmailConfirm;

public class ConfirmEmailMakerHandler : IConfirmEmailMaker
{
    private readonly UserManager<IdentityUser> userManager;
    private readonly IWelcomeNotification welcomeNotification;

    public ConfirmEmailMakerHandler(UserManager<IdentityUser> userManager, IWelcomeNotification welcomeNotification)
    {
        this.userManager = userManager;
        this.welcomeNotification = welcomeNotification;
    }

    public AppResult<ConfirmEmailResult> Execute (ConfirmEmail args)
    {
        throw new NotImplementedException();
    }

    public async Task<AppResult<ConfirmEmailResult>> ExecuteAsync(ConfirmEmail args)
    {
        try
        {
            // user and token checking
            if(string.IsNullOrEmpty(args.UserId) || string.IsNullOrEmpty(args.Token))
            {
                return AppResult<ConfirmEmailResult>.CreateFailed(new ApplicationException("Provide valid userid and token"), "Provide valid userid and token");
            }

            var user = await userManager.FindByIdAsync(args.UserId);
            if(user == null)
            {
                return AppResult<ConfirmEmailResult>.CreateFailed(new ApplicationException("Can't find provided user"), "Can't find provided user", "INVALIDUSER");
            }

            var token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(args.Token));
            var result = await userManager.ConfirmEmailAsync(user, token);

            if(!result.Succeeded)
            {
                return AppResult<ConfirmEmailResult>.CreateFailed(new ApplicationException("Invalid userid and token"), "Invalid userid and token", "INVALIDUSERANDTOKEN");
            }

            var notificationResult = await welcomeNotification.ExecuteAsync(new WelcomeNotification{ Email = user.Email});
            if(!notificationResult.Succeeded)
            {
                return AppResult<ConfirmEmailResult>.CreateFailed(notificationResult.Error.Exception, notificationResult.Message);
            }

            return AppResult<ConfirmEmailResult>.CreateSucceeded(new ConfirmEmailResult{User = user}, "User successfully verified");
        }
        catch (Exception ex)
        {
            return AppResult<ConfirmEmailResult>.CreateFailed(ex, "An error occured in ConfirmEmailMakerHandler");
        }
    }
}