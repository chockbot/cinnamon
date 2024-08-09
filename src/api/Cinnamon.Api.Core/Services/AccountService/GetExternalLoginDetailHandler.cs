using System.Text;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Interactors;
using Cinnamon.Api.Core.Services.AccountService.Interactors.Results;
using Cinnamon.Framework.Common;
using Microsoft.AspNetCore.WebUtilities;

namespace Cinnamon.Api.Core.Services.AccountService;

public class GetExternalLoginDetailHandler : IGetExternalLoginDetailHandler
{
    private readonly IExternalLoginTokenData externalLoginTokenData;

    public GetExternalLoginDetailHandler(IExternalLoginTokenData externalLoginTokenData)
    {
        this.externalLoginTokenData = externalLoginTokenData;
    }

    public AppResult<GetExternalLoginDetailResult> Execute(GetExternalLoginDetailArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<GetExternalLoginDetailResult>.CreateFailed(ex, "An error occured in GetExternalLoginDetailHandler");
        }
    }

    public async Task<AppResult<GetExternalLoginDetailResult>> ExecuteAsync(GetExternalLoginDetailArgs args)
    {
        try
        {
            if(string.IsNullOrEmpty(args.Guid) || string.IsNullOrEmpty(args.Token))
            {
                return AppResult<GetExternalLoginDetailResult>.CreateFailed(
                    new ApplicationException("Invalid token and guid"), "Invalid token and guid");
            }

            // check token and guid
            var chkToken = await externalLoginTokenData.GetLoginToken(new Framework.ApiCommand.ApiData.ExternalLoginToken.Request.GetLoginTokenArgs {
                Guid = args.Guid,
                Token = args.Token,
            });
            if(!chkToken.Succeeded || chkToken.Result == null)
            {
                return AppResult<GetExternalLoginDetailResult>.CreateFailed(
                    new ApplicationException("An error occured in SubmitExternalRegisterHandler"), "An error occured in GetExternalLoginDetailHandler");
            }
            if(chkToken.Succeeded && !chkToken.Result.IsSuccess)
            {
                return AppResult<GetExternalLoginDetailResult>.CreateFailed(
                    new ApplicationException("Invalid guid and token"), "Invalid guid and token");
            }
            // can get login token details if not use yet
            if(chkToken.Result.Result.IsUsed)
            {
                return AppResult<GetExternalLoginDetailResult>.CreateFailed(
                    new ApplicationException("Invalid guid and token"), "Invalid guid and token");
            }

            return AppResult<GetExternalLoginDetailResult>.CreateSucceeded(new GetExternalLoginDetailResult {
                Email = chkToken.Result.Result.Email,
                FirstName = chkToken.Result.Result.FirstName,
                LastName = chkToken.Result.Result.LastName,
                IsEmptyUsername = chkToken.Result.Result.IsEmptyUsername
            }, "Success getting external login details");
        }
        catch (Exception ex)
        {
            return AppResult<GetExternalLoginDetailResult>.CreateFailed(ex, "An error occured in GetExternalLoginDetailHandler");
        }
    }
}