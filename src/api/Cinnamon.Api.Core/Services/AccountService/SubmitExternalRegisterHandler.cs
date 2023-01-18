using System.Text;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Interactors;
using Cinnamon.Api.Core.Services.AccountService.Interactors.Results;
using Cinnamon.Framework.Common;
using Microsoft.AspNetCore.WebUtilities;

namespace Cinnamon.Api.Core.Services.AccountService;

public class SubmitExternalRegisterHandler : IExternalRegisterHandler
{
    private readonly IExternalLoginTokenData externalLoginTokenData;
    private readonly ICustomerData customerData;

    public SubmitExternalRegisterHandler(IExternalLoginTokenData externalLoginTokenData, ICustomerData customerData)
    {
        this.externalLoginTokenData = externalLoginTokenData;
        this.customerData = customerData;
    }

    public AppResult<ExternalRegisterResult> Execute(ExternalRegisterArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<ExternalRegisterResult>.CreateFailed(ex, "An error occured in SubmitExternalRegisterHandler");
        }
    }

    public async Task<AppResult<ExternalRegisterResult>> ExecuteAsync(ExternalRegisterArgs args)
    {
        try
        {
            // check if email already in used
            var checkCustomer = await customerData.GetCustomerByEmail(args.Email);
            if(!checkCustomer.Succeeded || checkCustomer.Result == null)
            {
                return AppResult<ExternalRegisterResult>.CreateFailed(
                    new ApplicationException("An error occured in SubmitExternalRegisterHandler"), "An error occured in SubmitExternalRegisterHandler");
            }

            if(checkCustomer.Succeeded && checkCustomer.Result.IsSuccess)
            {
                return AppResult<ExternalRegisterResult>.CreateFailed(
                    new ApplicationException("Email address already registered by other user"), "Email address already registered by other user");
            }

            // check token and guid
            var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(args.Token));
            var chkToken = await externalLoginTokenData.GetLoginToken(decodedToken, args.Guid);
            if(!chkToken.Succeeded || chkToken.Result == null)
            {
                return AppResult<ExternalRegisterResult>.CreateFailed(
                    new ApplicationException("An error occured in SubmitExternalRegisterHandler"), "An error occured in SubmitExternalRegisterHandler");
            }
            if(chkToken.Succeeded && !chkToken.Result.IsSuccess)
            {
                return AppResult<ExternalRegisterResult>.CreateFailed(
                    new ApplicationException("Invalid guid and token"), "Invalid guid and token");
            }
            if(chkToken.Result.Result.IsUsed)
            {
                return AppResult<ExternalRegisterResult>.CreateFailed(
                    new ApplicationException("Invalid guid and token"), "Invalid guid and token");
            }
            // check if email is the same
            if(chkToken.Result.Result.Email != args.Email.Trim())
            {
                return AppResult<ExternalRegisterResult>.CreateFailed(
                    new ApplicationException("Invalid guid and token"), "Invalid guid and token");
            }

            // update external login token
            var externalLoginRes = await externalLoginTokenData.UpdateToken(new Framework.ApiCommand.ApiData.ExternalLoginToken.Request.UpdateExternalLoginTokenArgs {
                Id = chkToken.Result.Result.Id,
                IsUsed = true
            });

            if(!externalLoginRes.Succeeded || externalLoginRes.Result == null)
            {
                return AppResult<ExternalRegisterResult>.CreateFailed(
                    new ApplicationException(externalLoginRes.Message), externalLoginRes.Message);
            }
            if(externalLoginRes.Succeeded && !externalLoginRes.Result.IsSuccess)
            {
                return AppResult<ExternalRegisterResult>.CreateFailed(
                    new ApplicationException(externalLoginRes.Result.ErrorInfo?.Message), "An error occured in SubmitExternalRegisterHandler");
            }

            var createCustomer = await customerData.CreateCustomerWithPassword(new Framework.ApiCommand.ApiData.Customer.Request.CreateCustomerWithPasswordArgs {
                Birthdate = args.Birthdate,
                Email = args.Email,
                ExternalLogin = true,
                FirstName = args.FirstName,
                LastName = args.LastName,
                ProfilePath = args.ProfilePath,
                Password = args.Password
            });

            if(!createCustomer.Succeeded || createCustomer.Result == null)
            {
                return AppResult<ExternalRegisterResult>.CreateFailed(createCustomer.Error.Exception,createCustomer.Message);
            }

            if(createCustomer.Succeeded && !createCustomer.Result.IsSuccess)
            {
                return AppResult<ExternalRegisterResult>.CreateFailed(
                    new ApplicationException(createCustomer.Result.ErrorInfo?.Message), "An error occured in SubmitExternalRegisterHandler");
            }

            var created = createCustomer.Result.Result;

            return AppResult<ExternalRegisterResult>.CreateSucceeded(new ExternalRegisterResult {
                Birthdate = created.Birthdate,
                Email = created.Email,
                FirstName = created.FirstName,
                Id = created.Id,
                LastName = created.LastName,
                ProfileImg = created.ProfileImg
            }, "Successfully registered");

        }
        catch (Exception ex)
        {
            return AppResult<ExternalRegisterResult>.CreateFailed(ex, "An error occured in SubmitExternalRegisterHandler");
        }
    }
}