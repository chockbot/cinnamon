using Cinnamon.Framework.ApiCommand.ApiData.DTO.TokenGenerated;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Data.Services.Repository.Interfaces;

public interface ITokenGeneratedRepository 
{
    Task<AppResult<TokenGeneratedDTO>> CreateTokenGenearted(string tokenType, string guid, string token, string payload);
    Task<AppResult<TokenGeneratedDTO>> GetTokenGenerated(string guid, string token);
    Task<AppResult<TokenGeneratedDTO>> UpdateTokenGenearted(int id, string? tokenType, string? guid, string? token, string? payload);
}