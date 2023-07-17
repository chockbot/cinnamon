using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Interactors;
using Cinnamon.Api.Core.Services.AccountService.Interactors.Results;
using Cinnamon.Api.Core.Services.AdminService.Handlers;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.AccountService;

public class BlockedAccountHandler : IBlockedAccountHandler
{
    private readonly IGetProfileHandler getProfileHandler;
    private readonly IGetAdminUserByEmailHandler getAdminUserByEmailHandler;
    private readonly ICustomerData customerData;

    public BlockedAccountHandler(IGetProfileHandler getProfileHandler, IGetAdminUserByEmailHandler getAdminUserByEmailHandler,
        ICustomerData customerData)
    {
        this.getProfileHandler = getProfileHandler;
        this.getAdminUserByEmailHandler = getAdminUserByEmailHandler;
        this.customerData = customerData;
    }

    public AppResult<BlockedAccountResult> Execute(BlockedAccountArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<BlockedAccountResult>.CreateFailed(ex, "An error occured in BlockedAccountHandler");
        }
    }

    public async Task<AppResult<BlockedAccountResult>> ExecuteAsync(BlockedAccountArgs args)
    {
        try
        {
            var getCurrentProfile = await getProfileHandler.ExecuteAsync(new GetProfileArgs {});
            if(!getCurrentProfile.Succeeded || getCurrentProfile.Result == null)
            {
                return AppResult<BlockedAccountResult>.CreateFailed(new ApplicationException(getCurrentProfile.Message), getCurrentProfile.Message);
            }

            var getAdminRes = await getAdminUserByEmailHandler.ExecuteAsync(new AdminService.Interactors.GetAdminUserByEmailArgs {
                Email = getCurrentProfile.Result.Email
            });
            if(!getAdminRes.Succeeded || getAdminRes.Result == null)
            {
                return AppResult<BlockedAccountResult>.CreateFailed(new ApplicationException("Not allowed. Invalid request"), "Not allowed. Invalid request");
            }

            var updateRes = await customerData.UpdateCustomer(new Framework.ApiCommand.ApiData.Customer.Request.UpdateCustomerArgs {
                CustomerId = args.Id,
                IsAccountBan = args.IsBlock
            });
            if(!updateRes.Succeeded || updateRes.Result == null || !updateRes.Result.IsSuccess)
            {
                return AppResult<BlockedAccountResult>.CreateFailed(new ApplicationException(updateRes.Result?.ErrorInfo?.Message), updateRes.Message);
            }

            return AppResult<BlockedAccountResult>.CreateSucceeded(new BlockedAccountResult { }, "Successfully blocked/Unblocked Customer");
        }
        catch (Exception ex)
        {
            return AppResult<BlockedAccountResult>.CreateFailed(ex, "An error occured in BlockedAccountHandler");
        }
    }
}