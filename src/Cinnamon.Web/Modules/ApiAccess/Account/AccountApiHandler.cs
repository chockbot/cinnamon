using Cinnamon.Framework.ApiCommand.ApiCore.Account.Request;
using Cinnamon.Framework.ApiCommand.ApiCore.Account.Response;
using Cinnamon.Framework.Common;
using Cinnamon.Web.Modules.ApiAccess.Handlers;
using Flurl.Http;
using Flurl.Http.Configuration;
using Microsoft.AspNetCore.Mvc;

namespace Cinnamon.Web.Modules.ApiAccess.Account;

public class AccountApiHandler : IAccountApiHandler
{
    private readonly IFlurlClient flurlClient;

    public AccountApiHandler(IFlurlClientFactory flurlFac, Config.Config config)
    {
        flurlClient = flurlFac.Get(config.ApiUrl);
    }

    public async Task<AppResult<SubmitRegisterResult>> Register(SubmitRegisterArgs args)
    {
        try
        {
            var result = await flurlClient
                .Request("Account/Register")
                .PostJsonAsync(args)
                .ReceiveJson<SubmitRegisterResult>();

            return AppResult<SubmitRegisterResult>.CreateSucceeded(result, "Successfully posting register api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<SubmitRegisterResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<SubmitRegisterResult>.CreateFailed(ex, "An error occured when posting register api");
        }
    }

    public async Task<AppResult<SubmitRegisterResult>> ExternalRegister(SubmitExternalRegisterArgs args)
    {
        try
        {
            var result = await flurlClient
                .Request("Account/ExternalRegister")
                .PostJsonAsync(args)
                .ReceiveJson<SubmitRegisterResult>();

            return AppResult<SubmitRegisterResult>.CreateSucceeded(result, "Successfully posting register api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<SubmitRegisterResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<SubmitRegisterResult>.CreateFailed(ex, "An error occured when posting register api");
        }
    }

    public async Task<AppResult<RegisterWaitlistResult>> RegisterWaitlist(RegisterWaitlistArgs args)
    {
        try
        {
            var result = await flurlClient
                .Request("Account/RegisterWaitlist")
                .PostJsonAsync(args)
                .ReceiveJson<RegisterWaitlistResult>();

            return AppResult<RegisterWaitlistResult>.CreateSucceeded(result, "Successfully posting register waitlist api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<RegisterWaitlistResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<RegisterWaitlistResult>.CreateFailed(ex, "An error occured when posting register waitlist api");
        }
    }

    public async Task<AppResult<VerifyRegisteredEmailResult>> VerifyRegisteredEmail(VerifyRegisteredEmailArgs args)
    {
        try
        {
            var result = await flurlClient
                .Request("Account/VerifyRegisteredEmail")
                .PostJsonAsync(args)
                .ReceiveJson<VerifyRegisteredEmailResult>();

            return AppResult<VerifyRegisteredEmailResult>.CreateSucceeded(result, "Successfully posting verified registered email api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<VerifyRegisteredEmailResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<VerifyRegisteredEmailResult>.CreateFailed(ex, "An error occured when posting verified registered email api");
        }
    }

    public async Task<AppResult<ResendVerificationResult>> ResendVerification(ResendVerificationArgs args)
    {
        try
        {
            var result = await flurlClient
                .Request("Account/ResendVerification")
                .PostJsonAsync(args)
                .ReceiveJson<ResendVerificationResult>();

            return AppResult<ResendVerificationResult>.CreateSucceeded(result, "Successfully posting resend email verification api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<ResendVerificationResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<ResendVerificationResult>.CreateFailed(ex, "An error occured when posting resend email verification api");
        }
    }

    public async Task<AppResult<VerifiedLoginResult>> Login(VerifiedLoginArgs args)
    {
        try
        {
            var result = await flurlClient
                .Request("Account/Login")
                .PostJsonAsync(args)
                .ReceiveJson<VerifiedLoginResult>();

            return AppResult<VerifiedLoginResult>.CreateSucceeded(result, "Successfully posting account login api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<VerifiedLoginResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<VerifiedLoginResult>.CreateFailed(ex, "An error occured when posting account login api");
        }
    }

    public async Task<AppResult<ExternalLoginResult>> ExternalLogin(ExternalLoginArgs args)
    {
        try
        {
            var result = await flurlClient
                .Request("Account/tMSSMcKhx9YpmcYCAfCkGnSfau8SE8")
                .PostJsonAsync(args)
                .ReceiveJson<ExternalLoginResult>();

            return AppResult<ExternalLoginResult>.CreateSucceeded(result, "Successfully posting account login api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<ExternalLoginResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<ExternalLoginResult>.CreateFailed(ex, "An error occured when posting account login api");
        }
    }

    public async Task<AppResult<GetExternalLoginDetailResult>> GetExternalLoginDetail(string token, string guid)
    {
        try
        {
            var result = await flurlClient
                .Request($"Account/GetExternalLoginDetail/{token}/{guid}")
                .GetJsonAsync<GetExternalLoginDetailResult>();

            return AppResult<GetExternalLoginDetailResult>.CreateSucceeded(result, "Successfully getting external login detail api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetExternalLoginDetailResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetExternalLoginDetailResult>.CreateFailed(ex, "An error occured when getting external login detail api");
        }
    }

    public async Task<AppResult<GetProfileResult>> GetProfile(GetProfileArgs args, string token)
    {
        try
        {
            var result = await flurlClient
                            .WithOAuthBearerToken(token)
                            .Request("/Account/GetProfile")
                            .GetJsonAsync<GetProfileResult>();

            return AppResult<GetProfileResult>.CreateSucceeded(result, "Successfully getting profile api");
        }
        catch (FlurlHttpException ex)
        {
            var r = ex.Call.Request.Headers;
            return AppResult<GetProfileResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetProfileResult>.CreateFailed(ex, "An error occured when getting profile api");
        }
    }

    public async Task<AppResult<GetFamilyMemberResult>> GetFamilyMembers(string token)
    {
        try
        {
            var result = await flurlClient
                            .WithOAuthBearerToken(token)
                            .Request($"Account/GetFamilyMembers")
                            .GetJsonAsync<GetFamilyMemberResult>();

            return AppResult<GetFamilyMemberResult>.CreateSucceeded(result, "Successfully getting family members api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetFamilyMemberResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetFamilyMemberResult>.CreateFailed(ex, "An error occured when getting family members api");
        }
    }

    public async Task<AppResult<UpdateFamilyMemberResult>> UpdateFamilyMembers(UpdateFamilyMemberArgs args, string token)
    {
        try
        {
            var result = await flurlClient
                .WithOAuthBearerToken(token)
                .Request("Account/UpdateFamilyMembers")
                .PostJsonAsync(args)
                .ReceiveJson<UpdateFamilyMemberResult>();

            return AppResult<UpdateFamilyMemberResult>.CreateSucceeded(result, "Successfully posting update family members api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<UpdateFamilyMemberResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<UpdateFamilyMemberResult>.CreateFailed(ex, "An error occured when posting update family members api");
        }
    }

    public async Task<AppResult<CreateFamilyMemberResult>> CreateFamilyMembers(CreateFamilyMemberArgs args, string token)
    {
        try
        {
            var result = await flurlClient
                .WithOAuthBearerToken(token)
                .Request("Account/CreateFamilyMembers")
                .PostJsonAsync(args)
                .ReceiveJson<CreateFamilyMemberResult>();

            return AppResult<CreateFamilyMemberResult>.CreateSucceeded(result, "Successfully posting create family members api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<CreateFamilyMemberResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<CreateFamilyMemberResult>.CreateFailed(ex, "An error occured when posting create family members api");
        }
    }

    public async Task<AppResult<DeleteFamilyMembersResult>> DeleteFamilyMembers(DeleteFamilyMembersArgs args, string token)
    {
        try
        {
            var result = await flurlClient
                .WithOAuthBearerToken(token)
                .Request("Account/DeleteFamilyMembers")
                .PostJsonAsync(args)
                .ReceiveJson<DeleteFamilyMembersResult>();

            return AppResult<DeleteFamilyMembersResult>.CreateSucceeded(result, "Successfully posting delete family members api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<DeleteFamilyMembersResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<DeleteFamilyMembersResult>.CreateFailed(ex, "An error occured when posting delete family members api");
        }
    }

    public async Task<AppResult<UpdateProfileDetailsResult>> UpdateProfileDetails(UpdateProfileDetailsArgs args, string token)
    {
        try
        {
            var result = await flurlClient
                .WithOAuthBearerToken(token)
                .Request("Account/UpdateProfileDetails")
                .PostJsonAsync(args)
                .ReceiveJson<UpdateProfileDetailsResult>();

            return AppResult<UpdateProfileDetailsResult>.CreateSucceeded(result, "Successfully posting update profile details api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<UpdateProfileDetailsResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<UpdateProfileDetailsResult>.CreateFailed(ex, "An error occured when posting update profile details api");
        }
    }

    public async Task<AppResult<GetGovernmentIdsResult>> GetGovermentIds(string token)
    {
        try
        {
            var result = await flurlClient
                            .WithOAuthBearerToken(token)
                            .Request($"Account/GetGovermentIds")
                            .GetJsonAsync<GetGovernmentIdsResult>();

            return AppResult<GetGovernmentIdsResult>.CreateSucceeded(result, "Successfully getting government ids api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetGovernmentIdsResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetGovernmentIdsResult>.CreateFailed(ex, "An error occured when getting government ids api");
        }
    }

    public async Task<AppResult<UploadGovernmentIdsResult>> UploadGovernmentIds(UploadGovernmentIdsArgs args, string token)
    {
        try
        {
            var result = await flurlClient
                .WithOAuthBearerToken(token)
                .Request("Account/UploadGovernmentIds")
                .PostMultipartAsync(mp => 
                    {
                        mp.AddFile("FrontImageId", args.FrontImageId.OpenReadStream(), 
                            args.FrontImageId.FileName, args.FrontImageId.ContentType)
                        .AddFile("BackImageId", args.BackImageId.OpenReadStream(), 
                            args.BackImageId.FileName, args.BackImageId.ContentType);
                    })
                .ReceiveJson<UploadGovernmentIdsResult>();

            return AppResult<UploadGovernmentIdsResult>.CreateSucceeded(result, "Successfully posting upload government ids api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<UploadGovernmentIdsResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<UploadGovernmentIdsResult>.CreateFailed(ex, "An error occured when posting upload government ids api");
        }
    }
   
    public async Task<AppResult<UploadProfilePictureResult>> UploadProfilePicture(UploadProfilePictureArgs args, string token)
    {
        try
        {
            var result = await flurlClient
                .WithOAuthBearerToken(token)
                .Request("Account/UploadProfilePicture")
                 .PostMultipartAsync(mp =>
                 {
                     mp.AddFile("ProfileImageId", args.ProfileImageId.OpenReadStream(),
                         args.ProfileImageId.FileName, args.ProfileImageId.ContentType);
                 })
                .ReceiveJson<UploadProfilePictureResult>();

            return AppResult<UploadProfilePictureResult>.CreateSucceeded(result, "Successfully posting upload profile picture api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<UploadProfilePictureResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<UploadProfilePictureResult>.CreateFailed(ex, "An error occured when posting upload profile picture api");
        }
    }

    public async Task<AppResult<GetProfilePictureResult>> GetProfilePicture(string token)
    {
        try
        {
            var result = await flurlClient
                            .WithOAuthBearerToken(token)
                            .Request("Account/GetProfilePicture")
                            .GetJsonAsync<GetProfilePictureResult>();

            return AppResult<GetProfilePictureResult>.CreateSucceeded(result, "Successfully getting profile picture api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetProfilePictureResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetProfilePictureResult>.CreateFailed(ex, "An error occured when getting profile picture api");
        }
    }

    public async Task<AppResult<GetWaitListResult>> GetAllWaitList()
    {
        try
        {
            var result = await flurlClient
                .Request("Account/GetAllWaitList")
                .GetJsonAsync<GetWaitListResult>();

            return AppResult<GetWaitListResult>.CreateSucceeded(result, "Successfully getting customer waitlist api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetWaitListResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetWaitListResult>.CreateFailed(ex, "An error occured when getting customer waitlist api");
        }
    }
    
    public async Task<AppResult<GetCustomerByEmailResult>> GetCustomerByEmail(string email, string token)
    {
        try
        {
            var result = await flurlClient
                .WithOAuthBearerToken(token)
                .Request($"Account/GetCustomerByEmail/{email}")
                .GetJsonAsync<GetCustomerByEmailResult>();

            return AppResult<GetCustomerByEmailResult>.CreateSucceeded(result, "Successfully getting cutomer by email api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetCustomerByEmailResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetCustomerByEmailResult>.CreateFailed(ex, "An error occured when getting cutomer by email api");
        }
    }

    public async Task<AppResult<GetWaitListByGuidResult>> GetWaitListByGuid(string token, string guid)
    {
        try
        {
            var result = await flurlClient
                .WithOAuthBearerToken(token)
                .Request($"Account/GetWaitListByGuid/{guid}")
                .GetJsonAsync<GetWaitListByGuidResult>();

            return AppResult<GetWaitListByGuidResult>.CreateSucceeded(result, "Successfully getting waitlist by guid api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetWaitListByGuidResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetWaitListByGuidResult>.CreateFailed(ex, "An error occured when getting waitlist by guid api");
        }
    }

    public async Task<AppResult<GetCustomerByIdResult>> GetCustomerById(int id)
    {
        try
        {
            var result = await flurlClient
                .Request($"Account/GetCustomerById/{id}")
                .GetJsonAsync<GetCustomerByIdResult>();

            return AppResult<GetCustomerByIdResult>.CreateSucceeded(result, "Successfully getting cutomer by email api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetCustomerByIdResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetCustomerByIdResult>.CreateFailed(ex, "An error occured when getting cutomer by email api");
        }
    }

    public async Task<AppResult<GetCustomerByIdResult>> GetMakerDetailByHandler(string handler)
    {
        try
        {
            var result = await flurlClient
                .Request($"Account/GetCustomerByHandler/{handler}")
                .GetJsonAsync<GetCustomerByIdResult>();

            return AppResult<GetCustomerByIdResult>.CreateSucceeded(result, "Successfully getting cutomer by handler api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetCustomerByIdResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetCustomerByIdResult>.CreateFailed(ex, "An error occured when getting cutomer by handler api");
        }
    }

    public async Task<AppResult<ResetPasswordResult>> ResetPassword(ResetPasswordArgs args)
    {
        try
        {
            var result = await flurlClient
                .Request("Account/ResetPassword")
                .PostJsonAsync(args)
                .ReceiveJson<ResetPasswordResult>();

            return AppResult<ResetPasswordResult>.CreateSucceeded(result, "Successfully posting reset password api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<ResetPasswordResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<ResetPasswordResult>.CreateFailed(ex, "An error occured when posting reset password api");
        }
    }

    public async Task<AppResult<VerifyResetPasswordResult>> VerifyResetPassword(VerifyResetPasswordArgs args)
    {
        try
        {
            var result = await flurlClient
                .Request("Account/VerifyResetPassword")
                .PostJsonAsync(args)
                .ReceiveJson<VerifyResetPasswordResult>();

            return AppResult<VerifyResetPasswordResult>.CreateSucceeded(result, "Successfully verify reset password api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<VerifyResetPasswordResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<VerifyResetPasswordResult>.CreateFailed(ex, "An error occured when verify reset password api");
        }
    }

    public async Task<AppResult<RequestRefundResult>> RequestRefund(RequestRefundArgs args, string token)
    {
        try
        {
            var result = await flurlClient
                .WithOAuthBearerToken(token)
                .Request("Account/RequestRefund")
                .PostJsonAsync(args)
                .ReceiveJson<RequestRefundResult>();

            return AppResult<RequestRefundResult>.CreateSucceeded(result, "Successfully request refund api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<RequestRefundResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<RequestRefundResult>.CreateFailed(ex, "An error occured when request refund  api");
        }
    }

    public async Task<AppResult<GetRequestedRefundsResult>> GetRequestedRefunds(GetRequestedRefundsArgs args, string token)
    {
        try
        {
            var result = await flurlClient
                .WithOAuthBearerToken(token)
                .Request("Account/GetRequestedRefunds")
                .SetQueryParams(args)
                .GetJsonAsync<GetRequestedRefundsResult>();

            return AppResult<GetRequestedRefundsResult>.CreateSucceeded(result, "Successfully get requested refunds api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetRequestedRefundsResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetRequestedRefundsResult>.CreateFailed(ex, "An error occured when get requested refunds  api");
        }
    }
}