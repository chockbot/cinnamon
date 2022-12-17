using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Framework.ApiCommand.ApiData.Customer.Request;
using Cinnamon.Framework.ApiCommand.ApiData.Customer.Response;
using Cinnamon.Framework.Common;
using Flurl;
using Flurl.Http;
using Cinnamon.Api.Core.Config;
using Flurl.Http.Configuration;

namespace Cinnamon.Api.Core.Modules.DataAccess.Customer;

public class CustomerData : ICustomerData
{
    private readonly IFlurlClient flurlClient;

    public CustomerData(ApplicationConfig config, IFlurlClientFactory flurlFac)
    {
        flurlClient = flurlFac.Get(config.ApiDataUrl);
    }

    public async Task<AppResult<CreateCustomerResult>> CreateCustomer(CreateCustomerArgs args)
    {
        try
        {
            var result = await flurlClient
                            .Request("Customer/CreateCustomer")
                            .PostJsonAsync(args)
                            .ReceiveJson<CreateCustomerResult>();

            return AppResult<CreateCustomerResult>.CreateSucceeded(result, "Successfully posting create customer api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<CreateCustomerResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<CreateCustomerResult>.CreateFailed(ex, "An error occured when posting create customer api");
        }
    }

    public async Task<AppResult<CreateCustomerResult>> CreateCustomerWithPassword(CreateCustomerWithPasswordArgs args)
    {
        try
        {
            var result = await flurlClient
                            .Request("Customer/CreateCustomerWithPassword")
                            .PostJsonAsync(args)
                            .ReceiveJson<CreateCustomerResult>();
            
            return AppResult<CreateCustomerResult>.CreateSucceeded(result, "Successfully posting create customer api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<CreateCustomerResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<CreateCustomerResult>.CreateFailed(ex, "An error occured when posting create customer api");
        }
    }

    public async Task<AppResult<GetAllCustomerResult>> GetAllCustomers(GetAllCustomersArgs args)
    {
        try
        {
            var result = await flurlClient
                            .Request("Customer/GetAllCustomers")
                            .SetQueryParams(
                                new {
                                    countPerPage = args.CountPerPage,
                                    isVerified = args.IsVerified,
                                    pageIndex = args.PageIndex }).GetJsonAsync<GetAllCustomerResult>();

            return AppResult<GetAllCustomerResult>.CreateSucceeded(result, "Successfully getting get all customers api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetAllCustomerResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetAllCustomerResult>.CreateFailed(ex, "An error occured when getting all customers api");
        }
    }

    public async Task<AppResult<GetCustomerResult>> GetCustomerById(int id)
    {
        try
        {
            var result = await flurlClient
                            .Request($"Customer/GetCustomerById/{id}")
                            .GetJsonAsync<GetCustomerResult>();
            
            return AppResult<GetCustomerResult>.CreateSucceeded(result, "Successfully getting customer by id api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetCustomerResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetCustomerResult>.CreateFailed(ex, "An error occured when getting customer by id api");
        }
    }

    public async Task<AppResult<GetCustomerResult>> GetCustomerByEmail(string email)
    {
        try
        {
            var result = await flurlClient
                            .Request($"Customer/GetCustomerByEmail/{email}")
                            .GetJsonAsync<GetCustomerResult>();
            
            return AppResult<GetCustomerResult>.CreateSucceeded(result, "Successfully getting customer by email api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetCustomerResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetCustomerResult>.CreateFailed(ex, "An error occured when getting customer by email api");
        }
    }

    public async Task<AppResult<UpdateCustomerResult>> UpdateCustomer(UpdateCustomerArgs args)
    {
        try
        {
            var result = await flurlClient
                            .Request("Customer/UpdateCustomer")
                            .PostJsonAsync(args)
                            .ReceiveJson<UpdateCustomerResult>();
            
            return AppResult<UpdateCustomerResult>.CreateSucceeded(result, "Successfully posting update customer api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<UpdateCustomerResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<UpdateCustomerResult>.CreateFailed(ex, "An error occured when posting update customer api");
        }
    }

    public async Task<AppResult<CheckCustomerLoginResult>> CheckCustomerLogin(CheckCustomerLoginArgs args)
    {
        try
        {
            var result = await flurlClient
                            .Request("Customer/CheckCustomerLogin")
                            .PostJsonAsync(args)
                            .ReceiveJson<CheckCustomerLoginResult>();
            
            return AppResult<CheckCustomerLoginResult>.CreateSucceeded(result, "Successfully checking customer login api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<CheckCustomerLoginResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<CheckCustomerLoginResult>.CreateFailed(ex, "An error occured when checking customer login api");
        }
    }
}