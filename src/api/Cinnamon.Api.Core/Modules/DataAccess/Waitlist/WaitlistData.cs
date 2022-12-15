using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Framework.Common;
using Flurl;
using Flurl.Http;
using Cinnamon.Api.Core.Config;
using Flurl.Http.Configuration;
using Cinnamon.Framework.ApiCommand.ApiData.Waitlist.Response;
using Cinnamon.Framework.ApiCommand.ApiData.Waitlist.Request;

namespace Cinnamon.Api.Core.Modules.DataAccess.Waitlist;

public class WaitlistData : IWaitListData
{
    private readonly IFlurlClient flurlClient;

    public WaitlistData(ApplicationConfig config, IFlurlClientFactory flurlFac)
    {
        flurlClient = flurlFac.Get(config.ApiDataUrl);
    }

    public async Task<AppResult<GetWaitlistResult>> GetWaitListByEmail(string email)
    {
        try
        {
            var result = await flurlClient
                            .Request($"WaitList/GetWaitlistByEmail/{email}")
                            .GetJsonAsync<GetWaitlistResult>();

            return AppResult<GetWaitlistResult>.CreateSucceeded(result, "Successfully getting waitlist by email api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetWaitlistResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetWaitlistResult>.CreateFailed(ex, "An error occured when getting waitlist by email api");
        }
    }

    public async Task<AppResult<GetWaitlistResult>> GetWaitListById(int id)
    {
        try
        {
            var result = await flurlClient
                            .Request($"WaitList/GetWaitlistById/{id}")
                            .GetJsonAsync<GetWaitlistResult>();

            return AppResult<GetWaitlistResult>.CreateSucceeded(result, "Successfully getting waitlist by id api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetWaitlistResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetWaitlistResult>.CreateFailed(ex, "An error occured when getting waitlist by id api");
        }
    }

    public async Task<AppResult<GetWaitlistResult>> GetWaitListByGuid(string guid)
    {
        try
        {
            var result = await flurlClient
                            .Request($"WaitList/GetWaitlistByGuid/{guid}")
                            .GetJsonAsync<GetWaitlistResult>();

            return AppResult<GetWaitlistResult>.CreateSucceeded(result, "Successfully getting waitlist by guid api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetWaitlistResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetWaitlistResult>.CreateFailed(ex, "An error occured when getting waitlist by guid api");
        }
    }

    public async Task<AppResult<GetAllWaitlistResult>> GetAllWaitlist(GetAllWaitlistArgs args)
    {
        try
        {
            var result = await flurlClient
                            .Request($"WaitList/GetAllWaitlist")
                            .SetQueryParams(args)
                            .GetJsonAsync<GetAllWaitlistResult>();

            return AppResult<GetAllWaitlistResult>.CreateSucceeded(result, "Successfully getting all waitlist");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetAllWaitlistResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetAllWaitlistResult>.CreateFailed(ex, "An error occured when getting all waitlist");
        }
    }

    public async Task<AppResult<CreatedWaitlistResult>> CreateWaitlist(CreateWaitlistArgs args)
    {
        try
        {
            var result = await flurlClient
                            .Request($"WaitList/CreateWaitlist")
                            .PostJsonAsync(args)
                            .ReceiveJson<CreatedWaitlistResult>();

            return AppResult<CreatedWaitlistResult>.CreateSucceeded(result, "Successfully getting all waitlist");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<CreatedWaitlistResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<CreatedWaitlistResult>.CreateFailed(ex, "An error occured when getting all waitlist");
        }
    }

    public async Task<AppResult<UpdateWaitlistResult>> UpdateWaitlist(UpdateWaitlistArgs args)
    {
        try
        {
            var result = await flurlClient
                            .Request($"WaitList/UpdateWaitlist")
                            .PostJsonAsync(args)
                            .ReceiveJson<UpdateWaitlistResult>();

            return AppResult<UpdateWaitlistResult>.CreateSucceeded(result, "Successfully getting all waitlist");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<UpdateWaitlistResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<UpdateWaitlistResult>.CreateFailed(ex, "An error occured when getting all waitlist");
        }
    }
}