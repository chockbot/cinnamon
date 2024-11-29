using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Framework.Common;
using Cinnamon.Framework.ApiCommand.ApiData.Customer.Request;
using Cinnamon.Api.Core.Services.AccountService.Interactors.Results;
using Cinnamon.Api.Core.Services.AccountService.Interactors;

namespace Cinnamon.Api.Core.Services.AccountService;

public class SubmitRegisterHandler : ISubmitRegisterHandler
{
    private readonly ICustomerData customerData;
    private readonly IWaitListData waitListData;
    private readonly IGenerateCustomerHandler generateCustomerHandler;

    public SubmitRegisterHandler(ICustomerData customerData, IWaitListData waitListData,
        IGenerateCustomerHandler generateCustomerHandler)
    {
        this.customerData = customerData;
        this.waitListData = waitListData;
        this.generateCustomerHandler = generateCustomerHandler;
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
            // check if email already in used
            var checkCustomer = await customerData.GetCustomerByEmail(args.Email);
            if(!checkCustomer.Succeeded || checkCustomer.Result == null)
            {
                return AppResult<SubmitRegisterResult>.CreateFailed(
                    new ApplicationException("An error occured in SubmitRegisterHandler"), "An error occured in SubmitRegisterHandler");
            }

            if(checkCustomer.Succeeded && checkCustomer.Result.IsSuccess)
            {
                return AppResult<SubmitRegisterResult>.CreateFailed(
                    new ApplicationException("Email address already registered by other user"), "Email address already registered by other user");
            }

            // check first if already in wait list
            var waitlistRes = await waitListData.GetWaitListByEmail(args.Email);
            if(!waitlistRes.Succeeded)
            {
                return AppResult<SubmitRegisterResult>.CreateFailed(waitlistRes.Error.Exception, waitlistRes.Message);
            }
            
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

            // generate unique customer handler
            var generateHandlerRes = await generateCustomerHandler.ExecuteAsync(new GenerateCustomerHandlerArgs {Handler = $"{args.FirstName} {args.LastName}"});
            if(!generateHandlerRes.Succeeded || generateHandlerRes.Result == null)
            {
                return AppResult<SubmitRegisterResult>.CreateFailed(new ApplicationException(generateHandlerRes.Message), generateHandlerRes.Message);
            }
            var handlerName = generateHandlerRes.Result.GeneratedHandler;

            // validate birthdate, age between 18 to 120
            var age = DateTime.Today.Year - args.Birthdate.Year;
            if(age < 13 || age > 120)
            {
                return AppResult<SubmitRegisterResult>.CreateFailed(
                    new ApplicationException("Please provide valid birth year. Age between 13 and 120"), "Please provide valid birth year. Age between 13 and 120");
            }
            
            var createCustomer = await customerData.CreateCustomerWithPassword(new CreateCustomerWithPasswordArgs {
                Birthdate = args.Birthdate,
                Email = args.Email,
                ExternalLogin = args.ExternalLogin,
                FirstName = args.FirstName,
                LastName = args.LastName,
                PhoneNumber = args.PhoneNumber,
                ProfilePath = args.ProfilePath, 
                Password = args.Password,
                Handler = handlerName,
                HasAcceptedTerms = args.HasAcceptedTerms,
                IsGuest = args.IsGuest
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
                Birthdate = created.Birthdate,
                Email = created.Email,
                FirstName = created.FirstName,
                Id = created.Id,
                PhoneNumber = created.PhoneNumber,
                LastName = created.LastName,
                ProfileImg = created.ProfileImg,
                Handler = created.Handler
            }, "Successfully registered");
        }
        catch (Exception ex)
        {
            return AppResult<SubmitRegisterResult>.CreateFailed(ex, "An error occured in SubmitRegisterHandler");
        }
    }
}