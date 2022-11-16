using System.Text;
using Microsoft.AspNetCore.Identity;
using Cinnamon.Core.Models;
using Cinnamon.Core.Common;
using Cinnamon.Core.Config;
using Cinnamon.Core.Module.CinnamonMakerService.Interactors;
using Cinnamon.Core.Module.CinnamonMakerService.Interactors.Results;

namespace Cinnamon.Core.Module.CinnamonMakerService.Handler.Register;

public class RegisterMakerHandler : IRegisterMaker
{
    private readonly CoreConfig coreConfig;
    private readonly UserManager<CustomerModel> usermanager;
    private readonly IUserStore<CustomerModel> userStore;

    public RegisterMakerHandler(UserManager<CustomerModel> userManager, IUserStore<CustomerModel> userStore,CoreConfig coreConfig)
    {
        this.coreConfig = coreConfig;
        this.usermanager = userManager;
        this.userStore = userStore;
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
            user.AcceptFlag = args.AcceptFlag;
            user.Birthdate = args.Birthdate;
            user.Email = args.Email;
            user.FirstName = args.FirstName;
            user.LastName = args.LastName;
            user.IsMaker = false;

            // register user using identity framework
            await userStore.SetUserNameAsync(user, args.Email, CancellationToken.None);

            var createUserResult = await usermanager.CreateAsync(user, args.Password);

            if(!createUserResult.Succeeded)
            {
                return AppResult<RegisterMakerResult>
                    .CreateFailed(new ApplicationException("An error occured when trying to create user"), 
                        "An error occured in RegisterMakerHandler");
            }

            var userId = await usermanager.GetUserIdAsync(user);

            return AppResult<RegisterMakerResult>
                .CreateSucceeded(new RegisterMakerResult { User = user }, "Customer successfully registered");

        }
        catch(Exception ex)
        {
            return AppResult<RegisterMakerResult>.CreateFailed(ex, "An error occured in RegisterMakerHandler");
        }
    }

    private CustomerModel CreateUser()
    {
        try
        {
            return Activator.CreateInstance<CustomerModel>();
        }
        catch
        {
            throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
                $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
        }
    }
}