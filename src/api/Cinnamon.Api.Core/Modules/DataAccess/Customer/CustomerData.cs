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
                            .SetQueryParams(args)
                            .GetJsonAsync<GetAllCustomerResult>();

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

    public async Task<AppResult<GetCustomerResult>> GetCustomerByHandler(string handler)
    {
        try
        {
            var result = await flurlClient
                            .Request($"Customer/GetByHandler/{handler}")
                            .GetJsonAsync<GetCustomerResult>();
            
            return AppResult<GetCustomerResult>.CreateSucceeded(result, "Successfully getting customer by handler api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetCustomerResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetCustomerResult>.CreateFailed(ex, "An error occured when getting customer by handler api");
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

    public async Task<AppResult<GetGovernmentIdResult>> GetGovernmentIds(int customerId)
    {
        try
        {
            var result = await flurlClient
                            .Request($"Customer/GetGovernmendId/{customerId}")
                            .GetJsonAsync<GetGovernmentIdResult>();

            return AppResult<GetGovernmentIdResult>.CreateSucceeded(result, "Successfully getting customer government id api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetGovernmentIdResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetGovernmentIdResult>.CreateFailed(ex, "An error occured when getting customer government id api");
        }
    }

    public async Task<AppResult<GetProfilePictureResult>> GetProfilePicture(int customerId)
    {

        try
        {
            var result = await flurlClient
                            .Request($"Customer/GetProfilePicture/{customerId}")
                            .GetJsonAsync<GetProfilePictureResult>();

            return AppResult<GetProfilePictureResult>.CreateSucceeded(result, "Successfully getting customer profile picture api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetProfilePictureResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetProfilePictureResult>.CreateFailed(ex, "An error occured when getting customer profile picture api");
        }
    }

    public async Task<AppResult<GenerateResetPasswordTokenResult>> GenerateResetPasswordToken(GenerateResetPasswordTokenArgs args)
    {
        try
        {
            var result = await flurlClient
                            .Request("Customer/GenerateResetPasswordToken")
                            .PostJsonAsync(args)
                            .ReceiveJson<GenerateResetPasswordTokenResult>();

            return AppResult<GenerateResetPasswordTokenResult>.CreateSucceeded(result, "Successfully generate reset password token");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GenerateResetPasswordTokenResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GenerateResetPasswordTokenResult>.CreateFailed(ex, "An error occured when generate reset password token");
        }
    }

    public async Task<AppResult<ResetPasswordResult>> ResetPassword(ResetPasswordArgs args)
    {
        try
        {
            var result = await flurlClient
                            .Request("Customer/ResetPassword")
                            .PostJsonAsync(args)
                            .ReceiveJson<ResetPasswordResult>();

            return AppResult<ResetPasswordResult>.CreateSucceeded(result, "Successfully resetted password");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<ResetPasswordResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<ResetPasswordResult>.CreateFailed(ex, "An error occured when resetting the password");
        }
    }

    public async Task<AppResult<ChangeEmailAddressResult>> ChangeEmailAddress(ChangeEmailAddressArgs args)
    {
        try
        {
            var result = await flurlClient
                            .Request("Customer/ChangeEmailAddress")
                            .PostJsonAsync(args)
                            .ReceiveJson<ChangeEmailAddressResult>();

            return AppResult<ChangeEmailAddressResult>.CreateSucceeded(result, "Successfully changed email address");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<ChangeEmailAddressResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<ChangeEmailAddressResult>.CreateFailed(ex, "An error occured when changing the email address");
        }
    }

    public async Task<AppResult<CreateCustomerResult>> CreateGuestCustomer(CreateGuestCustomerArgs args)
    {
        try
        {
            var result = await flurlClient
                .Request("Customer/CreateGuestCustomer")
                .PostJsonAsync(args)
                .ReceiveJson<CreateCustomerResult>();

            return AppResult<CreateCustomerResult>.CreateSucceeded(result, "Successfully created guest customer");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<CreateCustomerResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<CreateCustomerResult>.CreateFailed(
                ex, 
                "An error occurred when creating guest customer"
            );
        }
    }
}