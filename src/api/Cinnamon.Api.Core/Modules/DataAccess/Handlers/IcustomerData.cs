using Cinnamon.Framework.ApiCommand.ApiData.Customer.Response;
using Cinnamon.Framework.ApiCommand.ApiData.Customer.Request;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Modules.DataAccess.Handlers;

public interface ICustomerData 
{
    Task<AppResult<GetCustomerResult>> GetCustomerById(int id);
    Task<AppResult<GetCustomerResult>> GetCustomerByEmail(string email);
    Task<AppResult<GetAllCustomerResult>> GetAllCustomers(GetAllCustomersArgs args);
    Task<AppResult<CreateCustomerResult>> CreateCustomer(CreateCustomerArgs args);
    Task<AppResult<CreateCustomerResult>> CreateCustomerWithPassword(CreateCustomerWithPasswordArgs args);
    Task<AppResult<UpdateCustomerResult>> UpdateCustomer(UpdateCustomerArgs args);
    Task<AppResult<CheckCustomerLoginResult>> CheckCustomerLogin(CheckCustomerLoginArgs args);
    Task<AppResult<GetGovernmentIdResult>> GetGovernmentIds(int customerId);
    Task<AppResult<GetProfilePictureResult>> GetProfilePicture(int customerId);
    Task<AppResult<GetCustomerResult>> GetCustomerByHandler(string handler);
    Task<AppResult<GenerateResetPasswordTokenResult>> GenerateResetPasswordToken(GenerateResetPasswordTokenArgs args);
    Task<AppResult<ResetPasswordResult>> ResetPassword(ResetPasswordArgs args);
    Task<AppResult<ChangeEmailAddressResult>> ChangeEmailAddress(ChangeEmailAddressArgs args);
    Task<AppResult<CreateCustomerResult>> CreateGuestCustomer(CreateGuestCustomerArgs args);
}