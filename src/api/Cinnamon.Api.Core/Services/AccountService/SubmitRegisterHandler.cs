using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Framework.ApiCommand.ApiCore.Account.Request;
using Cinnamon.Framework.ApiCommand.ApiCore.Customer.Response;
using Cinnamon.Framework.Common;
using Cinnamon.Framework.ApiCommand.ApiData.Customer.Request;

namespace Cinnamon.Api.Core.Services.AccountService;

public class SubmitRegisterHandler : ISubmitRegisterHandler
{
    private readonly ICustomerData customerData;
    private readonly IWaitListData waitListData;

    public SubmitRegisterHandler(ICustomerData customerData, IWaitListData waitListData)
    {
        this.customerData = customerData;
        this.waitListData = waitListData;
    }

    public AppResult<SubmitRegisterResult> Execute(SubmitRegisterArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<SubmitRegisterResult>.CreateFailed(ex, "An error occured in SubmitRegisterHandler");
        }
    }

    public async Task<AppResult<SubmitRegisterResult>> ExecuteAsync(SubmitRegisterArgs args)
    {
        try
        {
            // check first if already in wait list
            var waitlistRes = await waitListData.GetWaitListByEmail(args.Email);
            if(!waitlistRes.Succeeded)
                return AppResult<SubmitRegisterResult>.CreateFailed(waitlistRes.Error.Exception, waitlistRes.Message);
            
            if(waitlistRes.Result != null && !waitlistRes.Result.IsSuccess)
            {
                // don't have yet resgirestered email in waitlist
                return AppResult<SubmitRegisterResult>.CreateFailed(
                    new ApplicationException("Don't have yet registered email"),"Don't have yet registered email");
            }

            if(waitlistRes.Result != null && !waitlistRes.Result.Result.IsVerified)
            {
                // email not yet verified
                return AppResult<SubmitRegisterResult>.CreateFailed(
                    new ApplicationException("Email not yet verified"),"Email not yet verified");
            }

            var createCustomer = await customerData.CreateCustomerWithPassword(new CreateCustomerWithPasswordArgs {
                Birthdate = args.Birthdate,
                Email = args.Email,
                ExternalLogin = args.ExternalLogin,
                FirstName = args.FirstName,
                LastName = args.LastName,
                IsMaker = args.IsMaker,
                ProfilePath = args.ProfilePath,
                Password = args.Password
            });

            if(!createCustomer.Succeeded)
            {
                return AppResult<SubmitRegisterResult>.CreateFailed(createCustomer.Error.Exception,createCustomer.Message);
            }

            if(createCustomer.Result == null)
            {
                return AppResult<SubmitRegisterResult>.CreateFailed(
                    new ApplicationException("An error occured in SubmitRegisterHandler"), "An error occured in SubmitRegisterHandler");
            }

            var created = createCustomer.Result.Result;

            return AppResult<SubmitRegisterResult>.CreateSucceeded(new SubmitRegisterResult {
                Result = new Framework.ApiCommand.ApiCore.DTO.Customer.CustomerDTO {
                    About = created.About,
                    Birthdate = created.Birthdate,
                    Email = created.Email,
                    ExternalLogin = created.ExternalLogin,
                    FirstName = created.FirstName,
                    Id = created.Id,
                    IsMaker = created.IsMaker,
                    IsVerified = created.IsVerified,
                    LastName = created.LastName,
                    ProfileImg = created.ProfileImg
                }
            }, "Successfully registered");
        }
        catch (Exception ex)
        {
            return AppResult<SubmitRegisterResult>.CreateFailed(ex, "An error occured in SubmitRegisterHandler");
        }
    }
}