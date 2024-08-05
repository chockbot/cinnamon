using Cinnamon.Framework.ApiCommand.ApiData.TokenGenerated.Request;
using Cinnamon.Framework.ApiCommand.ApiData.TokenGenerated.Response;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Modules.DataAccess.Handlers;

public interface ITokenGeneratedData 
{
    Task<AppResult<CreateTokenResult>> CreateTokenGenerated(CreateTokenArgs args);
    Task<AppResult<GetTokenResult>> GetTokenGenerated(string guid, string token);
    Task<AppResult<UpdateTokenResult>> UpdateToken(UpdateTokenArgs args, int id);
}