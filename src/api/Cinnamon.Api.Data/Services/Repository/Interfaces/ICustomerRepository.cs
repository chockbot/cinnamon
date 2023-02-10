using Cinnamon.Framework.Common;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.Customer;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.Waitlist;

namespace Cinnamon.Api.Data.Services.Repository.Interfaces;

public interface ICustomerRepository
{
    Task<AppResult<CustomerDTO>> GetByIdAsync(int id);
    Task<AppResult<CustomerDTO>> GetByEmailAsync(string email);
    Task<AppResult<IEnumerable<CustomerDTO>>> GetAllAsync(bool? isActive, int? count, int? skip);
    Task<AppResult<IEnumerable<CustomerDTO>>> GetAllAsync();
    Task<AppResult<CustomerDTO>> Create(string userId, string firstname, string lastname, string email, DateTime birthdate,
        string? about, string profilePath, bool ismaker, bool externalLogin, string handler);
    Task<AppResult<CustomerDTO>> CreateWithPassword(string firstname, string lastname, string email, DateTime birthdate,
        string? about, string profilePath, bool isMaker, bool externalLogin, string pasword, string handler);
    Task<AppResult<CustomerDTO>> CheckLogin(string email, string password);
    Task<AppResult<CustomerDTO>> Update(int customerId, string? firstname, string? lastname, string? email, DateTime? birthdate,
        string? about, string? profilePath, bool? ismaker, bool? externalLogin, bool? isVerified, string? frontIdImagePath, string? backIdImageParh);
    Task<AppResult<GovernmentIDsDTO>> GetGovermentId(int customerID);
    Task<AppResult<ProfilePictureDTO>> GetProfilePicture(int customerID);
    Task<AppResult<CustomerDTO>> GetByHandlerAsync(string handler);
}