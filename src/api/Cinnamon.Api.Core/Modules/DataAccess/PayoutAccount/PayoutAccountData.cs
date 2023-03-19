using Cinnamon.Api.Core.Config;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Framework.ApiCommand.ApiData.PayoutAccount.Request;
using Cinnamon.Framework.ApiCommand.ApiData.PayoutAccount.Response;
using Cinnamon.Framework.Common;
using Flurl.Http;
using Flurl.Http.Configuration;

namespace Cinnamon.Api.Core.Modules.DataAccess.PayoutAccount;

public class PayoutAccountData : IPayoutAccountData
{
    private readonly IFlurlClient flurlClient;

	public PayoutAccountData(ApplicationConfig config, IFlurlClientFactory flurlFac)
	{
        flurlClient = flurlFac.Get(config.ApiDataUrl);
    }

    public async Task<AppResult<CreatePayoutAccountResult>> CreatePayoutAccount(CreatePayoutAccountArgs args)
    {
        try
        {
            var result = await flurlClient
                .Request("PayoutAccount/CreatePayoutAccount")
                .PostJsonAsync(args)
                .ReceiveJson<CreatePayoutAccountResult>();

            return AppResult<CreatePayoutAccountResult>.CreateSucceeded(result, "Successfully posting create payout account api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<CreatePayoutAccountResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<CreatePayoutAccountResult>.CreateFailed(ex, "An error occured when posting create payout account api");
        }
    }

    public async Task<AppResult<GetAllPayoutAccountsResult>> GetAllPayoutAccounts(GetAllPayoutAccountsArgs args)
    {
        try
        {
            var result = await flurlClient
                            .Request("PayoutAccount/GetAllPayoutAccounts")
                            .SetQueryParams(args)
                            .GetJsonAsync<GetAllPayoutAccountsResult>();

            return AppResult<GetAllPayoutAccountsResult>.CreateSucceeded(result, "Successfully getting get all payout accounts api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetAllPayoutAccountsResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetAllPayoutAccountsResult>.CreateFailed(ex, "An error occured when getting all payout accounts api");
        }
    }

    public async Task<AppResult<GetPayoutAccountResult>> GetPayoutAccountByCustomerId(int id)
    {
        try
        {
            var result = await flurlClient
                            .Request($"PayoutAccount/GetPayoutAccountByCustomerId/{id}")
                            .GetJsonAsync<GetPayoutAccountResult>();

            return AppResult<GetPayoutAccountResult>.CreateSucceeded(result, "Successfully getting payout account by customer id api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetPayoutAccountResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetPayoutAccountResult>.CreateFailed(ex, "An error occured when getting payout account by customer id api");
        }
    }

    public async Task<AppResult<GetPayoutAccountResult>> GetPayoutAccountById(int id)
    {
        try
        {
            var result = await flurlClient
                            .Request($"PayoutAccount/GetPayoutAccountById/{id}")
                            .GetJsonAsync<GetPayoutAccountResult>();

            return AppResult<GetPayoutAccountResult>.CreateSucceeded(result, "Successfully getting payout account by id api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetPayoutAccountResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetPayoutAccountResult>.CreateFailed(ex, "An error occured when getting payout account by id api");
        }
    }

    public async Task<AppResult<UpdatePayoutAccountResult>> UpdatePayoutAccount(UpdatePayoutAccountArgs args)
    {
        try
        {
            var result = await flurlClient
                            .Request("PayoutAccount/UpdatePayoutAccount")
                            .PostJsonAsync(args)
                            .ReceiveJson<UpdatePayoutAccountResult>();

            return AppResult<UpdatePayoutAccountResult>.CreateSucceeded(result, "Successfully posting update payout account api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<UpdatePayoutAccountResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<UpdatePayoutAccountResult>.CreateFailed(ex, "An error occured when posting update payout account api");
        }
    }
}