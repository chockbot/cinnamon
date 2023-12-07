using Cinnamon.Api.Core.Config;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Framework.ApiCommand.ApiData.AddOns.Request;
using Cinnamon.Framework.ApiCommand.ApiData.AddOns.Response;
using Cinnamon.Framework.Common;
using Flurl.Http;
using Flurl.Http.Configuration;

namespace Cinnamon.Api.Core.Modules.DataAccess.AddOns;
public class AddOnsData : IAddOnsData
{
    private readonly IFlurlClient _flurlClient;
    public AddOnsData(IFlurlClientFactory flurlFac, ApplicationConfig config)
    {
        _flurlClient = flurlFac.Get(config.ApiDataUrl);
    }
    public async Task<AppResult<GetAddOnResult>> GetAddOnById(int id)
    {
        try
        {
            var result = await _flurlClient
                            .Request($"AddOn/GetAddOnById/{id}")
                            .GetJsonAsync<GetAddOnResult>();

            return AppResult<GetAddOnResult>.CreateSucceeded(result, "Successfully getting add-on by id api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetAddOnResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetAddOnResult>.CreateFailed(ex, "An error occurred when getting add-on by id api");
        }
    }
    public async Task<AppResult<GetAllAddOnResult>> GetAllAddOns()
    {
        try
        {
            var result = await _flurlClient
                            .Request("AddOn/GetAllAddOns")
                            .GetJsonAsync<GetAllAddOnResult>();

            return AppResult<GetAllAddOnResult>.CreateSucceeded(result, "Successfully getting get all add-ons api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetAllAddOnResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetAllAddOnResult>.CreateFailed(ex, "An error occurred when getting all add-ons api");
        }
    }
    public async Task<AppResult<CreateAddOnResult>> CreateAddOn(CreateAddOnArgs args)
    {
        try
        {
            var result = await _flurlClient
                            .Request("AddOn/CreateAddOn")
                            .PostJsonAsync(args)
                            .ReceiveJson<CreateAddOnResult>();

            return AppResult<CreateAddOnResult>.CreateSucceeded(result, "Successfully posting create add-ons api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<CreateAddOnResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<CreateAddOnResult>.CreateFailed(ex, "An error occurred when posting create add-ons api");
        }
    }
    public async Task<AppResult<CreateAddOnsResult>> CreateManyAddOns(CreateAddOnsArgs args)
    {
        try
        {
            var result = await _flurlClient
                            .Request("AddOn/CreateManyAddOns")
                            .PostJsonAsync(args)
                            .ReceiveJson<CreateAddOnsResult>();

            return AppResult<CreateAddOnsResult>.CreateSucceeded(result, "Successfully posting create add-ons api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<CreateAddOnsResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<CreateAddOnsResult>.CreateFailed(ex, "An error occurred when posting create add-ons api");
        }
    }
    public async Task<AppResult<DeleteAddOnsResult>> DeleteManyAddOns(DeleteAddOnsArgs args)
    {
        try
        {
            var result = await _flurlClient
                            .Request("AddOn/DeleteManyAddOns")
                            .PostJsonAsync(args)
                            .ReceiveJson<DeleteAddOnsResult>();

            return AppResult<DeleteAddOnsResult>.CreateSucceeded(result, "Successfully posting delete many add-ons api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<DeleteAddOnsResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<DeleteAddOnsResult>.CreateFailed(ex, "An error occurred when posting delete many add-ons api");
        }
    }
    public async Task<AppResult<UpdateAddOnResult>> UpdateAddOn(UpdateAddOnArgs args)
    {
        try
        {
            var result = await _flurlClient
                            .Request("AddOn/UpdateAdOn")
                            .PostJsonAsync(args)
                            .ReceiveJson<UpdateAddOnResult>();

            return AppResult<UpdateAddOnResult>.CreateSucceeded(result, "Successfully posting update add-on api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<UpdateAddOnResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<UpdateAddOnResult>.CreateFailed(ex, "An error occurred when posting update add-on api");
        }
    }
    public async Task<AppResult<UpdateAddOnsResult>> UpdateManyAddOns(UpdateAddOnsArgs args)
    {
        try
        {
            var result = await _flurlClient
                            .Request("AddOn/UpdateManyAddOns")
                            .PostJsonAsync(args)
                            .ReceiveJson<UpdateAddOnsResult>();

            return AppResult<UpdateAddOnsResult>.CreateSucceeded(result, "Successfully posting update many add-ons api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<UpdateAddOnsResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<UpdateAddOnsResult>.CreateFailed(ex, "An error occured when posting update many add-ons api");
        }
    }
}
