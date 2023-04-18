using Cinnamon.Framework.ApiCommand.ApiData.DTO.ResetPassword;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Data.Services.Repository.Interfaces;

public interface IResetPasswordRepository
{
    Task<AppResult<ResetPasswordDTO>> GetByIdAsync(int id);
    Task<AppResult<ResetPasswordDTO>> GetByGuidTokenAsync(string guid, string token);
    Task<AppResult<IEnumerable<ResetPasswordDTO>>> GetAllAsync(int? count, int? skip);
    Task<AppResult<IEnumerable<ResetPasswordDTO>>> GetAllAsync();
    Task<AppResult<ResetPasswordDTO>> Create(string email, string guid, string token, bool isUsed, string generatedToken);
    Task<AppResult<ResetPasswordDTO>> Update(int id, bool isUsed);
}