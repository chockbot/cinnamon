using Cinnamon.Framework.Common;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.Customer;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.Waitlist;

namespace Cinnamon.Api.Data.Services.Repository.Interfaces;

public interface ICustomerRepository
{
    Task<AppResult<CustomerDTO>> GetByIdAsync(int id);
    Task<AppResult<CustomerDTO>> GetByEmailAsync(string email);
    Task<AppResult<IEnumerable<CustomerDTO>>> GetAllAsync(bool? isActive, string searchValue, 
        int? count, int? skip, string? handlerLike = null, 
        bool? isOfficialPartner = false, bool? hasVerification = false);
    Task<AppResult<IEnumerable<CustomerDTO>>> GetAllAsync();
    Task<AppResult<CustomerDTO>> Create(string userId, string firstname, string lastname, string email, DateTime birthdate, string phoneNumber,
        string? about, string profilePath, bool ismaker, bool externalLogin, string handler, bool hasAcceptedTerms, bool IsGuest);
    Task<AppResult<CustomerDTO>> CreateWithPassword(string firstname, string lastname, string email, DateTime birthdate, string phoneNumber,
        string? about, string profilePath, bool isMaker, bool externalLogin, string pasword, string handler, bool hasAcceptedTerms, bool IsGuest);
    Task<AppResult<CustomerDTO>> CheckLogin(string email, string password);
    Task<AppResult<CustomerDTO>> Update(int customerId, string? firstname, string? lastname, string? email, DateTime? birthdate, string? phoneNumber,
        string? about, string? profilePath, bool? ismaker, bool? externalLogin, int? isVerified, DateTime? isVerifiedDate, 
        string? frontIdImagePath, string? backIdImageParh, decimal? totalCredits, bool? isOG, DateTime? isOGDate, bool? isOF, 
        DateTime? isOfficialDate, string? connectionId, bool? isAccountBan, string? handler = null);
    Task<AppResult<GovernmentIDsDTO>> GetGovermentId(int customerID);
    Task<AppResult<ProfilePictureDTO>> GetProfilePicture(int customerID);
    Task<AppResult<CustomerDTO>> GetByHandlerAsync(string handler);
    Task<AppResult<string>> GenerateResetPasswordToken(string email);
    Task<AppResult<bool>> ResetPassword(string email, string token, string newPassword);
    Task<AppResult<bool>> ChangeEmailAddress(string currentEmail, string newEmail);
}