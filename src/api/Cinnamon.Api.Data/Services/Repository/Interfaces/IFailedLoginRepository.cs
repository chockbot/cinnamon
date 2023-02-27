using Cinnamon.Framework.ApiCommand.ApiData.DTO.FailedLogin;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Data.Services.Repository.Interfaces;

public interface IFailedLoginRepository 
{
    Task<AppResult<FailedLoginDTO>> CreateAsync(string email, string metadata, DateTime loginDate);
    Task<AppResult<IEnumerable<FailedLoginDTO>>> GetFailedLogins(string email, DateTime from, DateTime to);
    Task<AppResult<IEnumerable<FailedLoginDTO>>> RemoveLogins(IEnumerable<FailedLoginDTO> logins);
}