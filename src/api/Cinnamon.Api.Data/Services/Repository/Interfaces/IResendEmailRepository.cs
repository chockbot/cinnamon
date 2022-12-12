using Cinnamon.Api.Data.Services.Repository.ResendEmail.DTO;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Data.Services.Repository.Interfaces;

public interface IResendEmailRepository
{
    Task<AppResult<ResendEmailDTO>> GetByIdAsync(int id);
    Task<AppResult<IEnumerable<ResendEmailDTO>>> GetByEmailDateRange(string email, DateTime from, DateTime to);
    Task<AppResult<IEnumerable<ResendEmailDTO>>> GetAllAsync(int? count, int? skip);
    Task<AppResult<IEnumerable<ResendEmailDTO>>> GetAllAsync();
    Task<AppResult<ResendEmailDTO>> Create(string email, DateTime dateResend);
    Task<AppResult<ResendEmailDTO>> Update(int resendEmailId, string? email, DateTime? dateResend);
}