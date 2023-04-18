using Cinnamon.Framework.ApiCommand.ApiData.ExternalLoginToken.Request;
using Cinnamon.Framework.ApiCommand.ApiData.ExternalLoginToken.Response;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Modules.DataAccess.Handlers;
public interface IExternalLoginTokenData
{
    Task<AppResult<GetExternalLoginTokenResult>> GetLoginToken(string token, string guid);
    Task<AppResult<CreateExternalLoginTokenResult>> CreateToken(CreateExterLoginTokenArgs args);
    Task<AppResult<UpdateExternalLoginTokenResult>> UpdateToken(UpdateExternalLoginTokenArgs args);
}
