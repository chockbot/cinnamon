using Cinnamon.Framework.ApiCommand.ApiData.DTO.ExternalLoginToken;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Data.Services.Repository.Interfaces
{
    public interface IExternalLoginTokenRepository
    {
        Task<AppResult<ExternalLoginTokenDTO>> GetByTokenAsync(string token, string guid);
        Task<AppResult<ExternalLoginTokenDTO>> CreateTokenAsync(string token, string guid, string email, 
            DateTime dateGenerated, string firstName, string lastName, bool isEmptyUsername);
        Task<AppResult<ExternalLoginTokenDTO>> UpdateTokenAsync(int id, bool isUsed);
    }
}
