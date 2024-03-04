using Cinnamon.Api.Core.Config;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Framework.ApiCommand.ApiData.DynamicContent.Request;
using Cinnamon.Framework.ApiCommand.ApiData.DynamicContent.Response;
using Cinnamon.Framework.Common;
using Flurl.Http;
using Flurl.Http.Configuration;

namespace Cinnamon.Api.Core.Modules.DataAccess.DynamincContent;

public class DynamincContentData : IDynamicContentData
{
    private readonly IFlurlClient flurlClient;

    public DynamincContentData(ApplicationConfig config, IFlurlClientFactory flurlFac)
    {
        flurlClient = flurlFac.Get(config.ApiDataUrl);
    }

    public async Task<AppResult<CreateDynamicContentResult>> CreateDynamicContent(CreateDynamicContentArgs args)
    {
        try
        {
            var result = await flurlClient
                            .Request("DynamicContents")
                            .PostJsonAsync(args)
                            .ReceiveJson<CreateDynamicContentResult>();
            return AppResult<CreateDynamicContentResult>.CreateSucceeded(result, "Successfully posting create dynamic content api");
        }
        catch (FlurlHttpException ex)
        {
            var flurlError = await ex.GetResponseJsonAsync();
            return AppResult<CreateDynamicContentResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<CreateDynamicContentResult>.CreateFailed(ex, "An error occured when posting create dynamic content api");
        }
    }

    public async Task<AppResult<GetDynamicContentResult>> GetDynamicContent(string identifier)
    {
        try
        {
            var result = await flurlClient
                            .Request($"DynamicContents/{identifier}")
                            .GetJsonAsync<GetDynamicContentResult>();
            return AppResult<GetDynamicContentResult>.CreateSucceeded(result, "Successfully get dynamic content by identifier api");
        }
        catch (FlurlHttpException ex)
        {
            var flurlError = await ex.GetResponseJsonAsync();
            return AppResult<GetDynamicContentResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetDynamicContentResult>.CreateFailed(ex, "An error occured when getting dynamic content by identifier api");
        }
    }

    public async Task<AppResult<UpdateDynamicContentResult>> UpdateDynamicContent(UpdateDynamicContentArgs args)
    {
        try
        {
            var result = await flurlClient
                            .Request("DynamicContents/UpdateDynamicContent")
                            .PostJsonAsync(args)
                            .ReceiveJson<UpdateDynamicContentResult>();
            return AppResult<UpdateDynamicContentResult>.CreateSucceeded(result, "Successfully posting update dynamic content api.");
        }
        catch (FlurlHttpException ex)
        {
            var flurlError = await ex.GetResponseJsonAsync();
            return AppResult<UpdateDynamicContentResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<UpdateDynamicContentResult>.CreateFailed(ex, "An error occured when posting update dynamic content api.");
        }
    }
}