using System.Text;
using Microsoft.AspNetCore.Identity;
using Cinnamon.Core.Models;
using Cinnamon.Core.Common;
using Cinnamon.Core.Config;
using Cinnamon.Core.Module.CinnamonMakerService.Interactors;
using Cinnamon.Core.Module.CinnamonMakerService.Interactors.Results;
using Cinnamon.Core.Module.NotificationService.Handler;

namespace Cinnamon.Core.Module.CinnamonMakerService.Handler.Register;

public class RegisterMakerHandler : IRegisterMaker
{
    private readonly CoreConfig coreConfig;
    private readonly UserManager<IdentityUser> usermanager;
    private readonly IUserStore<IdentityUser> userStore;
    private readonly IUserEmailStore<IdentityUser> emailStore;
    private readonly IWelcomeNotification welcomeNotification;

    public RegisterMakerHandler(UserManager<IdentityUser> userManager, 
        IUserStore<IdentityUser> userStore,CoreConfig coreConfig, IWelcomeNotification welcomeNotification)
    {
        this.coreConfig = coreConfig;
        this.usermanager = userManager;
        this.userStore = userStore;
        this.emailStore = GetEmailStore();
        this.welcomeNotification = welcomeNotification;
    }

    public AppResult<RegisterMakerResult> Execute(RegisterMaker args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex) 
        {
            return AppResult<RegisterMakerResult>.CreateFailed(ex, "An error occured in RegisterMakerHandler");
        }
    }

    public async Task<AppResult<RegisterMakerResult>> ExecuteAsync(RegisterMaker args)
    {
        try
        {
            var user = CreateUser();

            // register user using identity framework
            await userStore.SetUserNameAsync(user, args.Email, CancellationToken.None);
            await emailStore.SetEmailAsync(user, args.Email, CancellationToken.None);
            var createUserResult = await usermanager.CreateAsync(user, args.Password);

            if(!createUserResult.Succeeded)
            {
                string errors = string.Empty;
                foreach(var error in createUserResult.Errors)
                {
                    errors += error.Description + ". ";
                }
                return AppResult<RegisterMakerResult>
                    .CreateFailed(new ApplicationException(errors),errors);
            }

            var userId = await usermanager.GetUserIdAsync(user);

            var customer = new CustomerModel
            {
                AcceptFlag      = args.AcceptFlag,
                Birthdate       = args.Birthdate,
                Email           = args.Email,
                FirstName       = args.FirstName,
                LastName        = args.LastName,
                IsMaker         = false,
                UserId          = userId,
                ProfilePath     = args.ProfilePath
                
            };

            var customerRes = await CoreDI.DataStore.Customers.SaveDataAsync(customer);
            if(!customerRes.Message.ToLower().Contains("saved"))
            {
                return AppResult<RegisterMakerResult>.CreateFailed(new ApplicationException("An error occured when saving customer information"), "An error occured in RegisterMakerHandler");
            }

            var welcomeNotify = await welcomeNotification.ExecuteAsync(new NotificationService.Interactors.WelcomeNotification { Email = args.Email });
            if(!welcomeNotify.Succeeded)
            {
                return AppResult<RegisterMakerResult>.CreateFailed(welcomeNotify.Error.Exception, welcomeNotify.Message);
            }

            return AppResult<RegisterMakerResult>
                .CreateSucceeded(new RegisterMakerResult { User = user }, "Customer successfully registered");

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

    private IUserEmailStore<IdentityUser> GetEmailStore()
    {
        if (!usermanager.SupportsUserEmail)
        {
            throw new NotSupportedException("The default UI requires a user store with email support.");
        }
        return (IUserEmailStore<IdentityUser>)userStore;
    }
}