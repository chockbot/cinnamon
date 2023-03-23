using Cinnamon.Framework.Common;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.Activity;

namespace Cinnamon.Api.Data.Services.Repository.Interfaces;

public interface IAdminUserRepository
{
    Task<AppResult<AdminUserDTO>> GetAdminUserByEmailAsync(string email);
}