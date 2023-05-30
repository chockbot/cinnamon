using Cinnamon.Api.Core.Config;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Framework.ApiCommand.ApiData.CustomerPricing.Request;
using Cinnamon.Framework.ApiCommand.ApiData.CustomerPricing.Response;
using Cinnamon.Framework.Common;
using Flurl;
using Flurl.Http;
using Flurl.Http.Configuration;

namespace Cinnamon.Api.Core.Modules.DataAccess;

public class CustomerPricingData : ICustomerPricingData
{
    private readonly IFlurlClient flurlClient;

    public CustomerPricingData(ApplicationConfig config, IFlurlClientFactory flurlFac)
    {
        flurlClient = flurlFac.Get(config.ApiDataUrl);
    }

    public async Task<AppResult<CreateCustomerPricingResult>> CreateCustomerPricing(CreateCustomerPricingArgs args)
    {
        try
        {
            var result = await flurlClient
                            .Request("CustomerPricing/CreateCustomerPricing")
                            .PostJsonAsync(args)
                            .ReceiveJson<CreateCustomerPricingResult>();
            
            return AppResult<CreateCustomerPricingResult>.CreateSucceeded(result, "Successfully posting create customer pricing api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<CreateCustomerPricingResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<CreateCustomerPricingResult>.CreateFailed(ex, "An error occured when posting create customer pricing api");
        }
    }

    public async Task<AppResult<GetAllCustomerPricingResult>> GetAllCustomerPricing(GetAllCustomerPricingArgs args)
    {
        try
        {
            var result = await flurlClient
                            .Request("CustomerPricing/GetAllCustomerPricing")
                            .SetQueryParams(args)
                            .GetJsonAsync<GetAllCustomerPricingResult>();

            return AppResult<GetAllCustomerPricingResult>.CreateSucceeded(result, "Successfully getting get all customers pricing api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetAllCustomerPricingResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetAllCustomerPricingResult>.CreateFailed(ex, "An error occured when getting all customer pricing api");
        }
    }

    public async Task<AppResult<GetCustomerPricingResult>> GetCustomerPricingByCustomerId(int id)
    {
        try
        {
            var result = await flurlClient
                            .Request($"CustomerPricing/GetCustomerPricingByCustomerId/{id}")
                            .GetJsonAsync<GetCustomerPricingResult>();

            return AppResult<GetCustomerPricingResult>.CreateSucceeded(result, "Successfully getting get customers pricing api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetCustomerPricingResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetCustomerPricingResult>.CreateFailed(ex, "An error occured when getting customer pricing api");
        }
    }

    public async Task<AppResult<GetCustomerPricingResult>> GetCustomerPricingById(int id)
    {
        try
        {
            var result = await flurlClient
                            .Request($"CustomerPricing/GetCustomerPricingById/{id}")
                            .GetJsonAsync<GetCustomerPricingResult>();

            return AppResult<GetCustomerPricingResult>.CreateSucceeded(result, "Successfully getting get customers pricing api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetCustomerPricingResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetCustomerPricingResult>.CreateFailed(ex, "An error occured when getting customer pricing api");
        }
    }

    public async Task<AppResult<UpdateCustomerPricingResult>> UpdateCustomerPricing(UpdateCustomerPricingArgs args)
    {
        try
        {
            var result = await flurlClient
                            .Request("CustomerPricing/UpdateCustomerPricing")
                            .PostJsonAsync(args)
                            .ReceiveJson<UpdateCustomerPricingResult>();
            
            return AppResult<UpdateCustomerPricingResult>.CreateSucceeded(result, "Successfully posting create customer pricing api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<UpdateCustomerPricingResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<UpdateCustomerPricingResult>.CreateFailed(ex, "An error occured when posting update customer pricing api");
        }
    }
}