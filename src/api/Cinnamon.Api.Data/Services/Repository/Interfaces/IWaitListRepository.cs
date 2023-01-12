using Cinnamon.Framework.ApiCommand.ApiData.DTO.Waitlist;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Data.Services.Repository.Interfaces;

public interface IWaitListRepository
{
    Task<AppResult<WaitListDTO>> GetByIdAsync(int id);
    Task<AppResult<WaitListDTO>> GetByGuidAsync(string guid);
    Task<AppResult<WaitListDTO>> GetByEmailAsync(string email);
    Task<AppResult<IEnumerable<WaitListDTO>>> GetAllAsync(int? count, int? skip);
    Task<AppResult<IEnumerable<WaitListDTO>>> GetAllAsync();
    Task<AppResult<WaitListDTO>> Create(string email, string guid, string token, bool isVerified);
    Task<AppResult<WaitListDTO>> Update(string email, string? guid, string? token, bool? isVerified);
}