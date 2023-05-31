using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Interactors;
using Cinnamon.Api.Core.Services.AccountService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.AccountService;

public class UpdateConnectionIdHandler : IUpdateConnectionIdHandler
{
    private readonly ICustomerData customerData;
    private readonly IGetProfileHandler getProfileHandler;

    public UpdateConnectionIdHandler(ICustomerData customerData, IGetProfileHandler getProfileHandler)
    {
        this.customerData = customerData;
        this.getProfileHandler = getProfileHandler;
    }

    public AppResult<UpdateConnectionIdResult> Execute(UpdateConnectionIdArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<UpdateConnectionIdResult>.CreateFailed(ex, "An error occured in UpdateConnectionIdHandler");
        }
    }

    public async Task<AppResult<UpdateConnectionIdResult>> ExecuteAsync(UpdateConnectionIdArgs args)
    {
        try
        {
            // get profile details
            var profileRes = await getProfileHandler.ExecuteAsync(new GetProfileArgs { });
            if (!profileRes.Succeeded || profileRes.Result == null)
            {
                return AppResult<UpdateConnectionIdResult>.CreateFailed(new ApplicationException(profileRes.Message), profileRes.Message);
            }
            var profile = profileRes.Result;

            var result = await customerData.UpdateCustomer(new Framework.ApiCommand.ApiData.Customer.Request.UpdateCustomerArgs
            {
                CustomerId = profile.Id,
                ConnectionId = args.ConnectionId,
            });
            if (!result.Succeeded || result.Result == null)
            {
                return AppResult<UpdateConnectionIdResult>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }

            if (result.Succeeded && !result.Result.IsSuccess)
            {
                return AppResult<UpdateConnectionIdResult>.CreateFailed(
                    new ApplicationException(result.Result.ErrorInfo?.Message), "An error occured in UpdateConnectionIdHandler");
            }

            return AppResult<UpdateConnectionIdResult>.CreateSucceeded(new UpdateConnectionIdResult
            {
                CustomerId = result.Result.Result.Id,
                ConnectionId = result.Result.Result.ConnectionId
            }, "Successfully updated customer connection ID");
        }
        catch (Exception ex)
        {
            return AppResult<UpdateConnectionIdResult>.CreateFailed(ex, "An error occured in UpdateConnectionIdHandler");
        }
    }
}