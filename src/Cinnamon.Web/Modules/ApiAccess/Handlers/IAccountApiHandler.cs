using Cinnamon.Framework.ApiCommand.ApiCore.Account.Request;
using Cinnamon.Framework.ApiCommand.ApiCore.Account.Response;
using Cinnamon.Framework.Common;

namespace Cinnamon.Web.Modules.ApiAccess.Handlers;

public interface IAccountApiHandler 
{
    Task<AppResult<SubmitRegisterResult>> Register(SubmitRegisterArgs args);
    Task<AppResult<SubmitRegisterResult>> ExternalRegister(SubmitExternalRegisterArgs args);
    Task<AppResult<RegisterWaitlistResult>> RegisterWaitlist(RegisterWaitlistArgs args);
    Task<AppResult<VerifyRegisteredEmailResult>> VerifyRegisteredEmail(VerifyRegisteredEmailArgs args);
    Task<AppResult<ResendVerificationResult>> ResendVerification(ResendVerificationArgs args);
    Task<AppResult<VerifiedLoginResult>> Login(VerifiedLoginArgs args);
    Task<AppResult<ExternalLoginResult>> ExternalLogin(ExternalLoginArgs args);
    Task<AppResult<GetExternalLoginDetailResult>> GetExternalLoginDetail(string token, string guid);
    Task<AppResult<GetProfileResult>> GetProfile(GetProfileArgs args, string token);
    Task<AppResult<GetFamilyMemberResult>> GetFamilyMembers(string token);
    Task<AppResult<UpdateFamilyMemberResult>> UpdateFamilyMembers(UpdateFamilyMemberArgs args, string token);
    Task<AppResult<CreateFamilyMemberResult>> CreateFamilyMembers(CreateFamilyMemberArgs args, string token);
    Task<AppResult<DeleteFamilyMembersResult>> DeleteFamilyMembers(DeleteFamilyMembersArgs args, string token);
    Task<AppResult<UpdateProfileDetailsResult>> UpdateProfileDetails(UpdateProfileDetailsArgs args, string token);
    Task<AppResult<GetGovernmentIdsResult>> GetGovermentIds(string token);
    Task<AppResult<GetProfilePictureResult>> GetProfilePicture(string token);
    Task<AppResult<UploadGovernmentIdsResult>> UploadGovernmentIds(UploadGovernmentIdsArgs args, string token);
    Task<AppResult<UploadProfilePictureResult>> UploadProfilePicture(UploadProfilePictureArgs args, string token);
    Task<AppResult<GetWaitListResult>> GetAllWaitList();
    Task<AppResult<GetCustomerByEmailResult>> GetCustomerByEmail(string token, string email);
    Task<AppResult<GetWaitListByGuidResult>> GetWaitListByGuid(string token, string guid);
    Task<AppResult<GetCustomerByIdResult>> GetCustomerById(int id);
    Task<AppResult<GetCustomerByIdResult>> GetMakerDetailByHandler(string handler);
    Task<AppResult<ResetPasswordResult>> ResetPassword(ResetPasswordArgs args);
} 