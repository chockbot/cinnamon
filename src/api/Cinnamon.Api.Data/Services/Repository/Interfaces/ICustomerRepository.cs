using Cinnamon.Framework.Common;
using Cinnamon.Api.Data.Services.Repository.Customer.DTO;

namespace Cinnamon.Api.Data.Services.Repository.Interfaces;

public interface ICustomerRepository
{
    Task<AppResult<CustomerDTO>> GetByIdAsync(int id);
    Task<AppResult<IEnumerable<CustomerDTO>>> GetAllAsync(bool? isActive, int? count, int? skip);
    Task<AppResult<IEnumerable<CustomerDTO>>> GetAllAsync();
    Task<AppResult<CustomerDTO>> Create(string userId, string firstname, string lastname, string email, DateTime birthdate,
        string? about, string profilePath, bool ismaker, bool externalLogin);
    Task<AppResult<CustomerDTO>> Update(int customerId, string? firstname, string? lastname, string? email, DateTime? birthdate,
        string? about, string? profilePath, bool? ismaker, bool? externalLogin, bool? isVerified, string? frontIdImagePath, string? backIdImageParh);
}