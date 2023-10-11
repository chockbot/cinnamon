using Cinnamon.Framework.ApiCommand.ApiData.TokenGenerated.Request;
using Cinnamon.Framework.ApiCommand.ApiData.TokenGenerated.Response;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Modules.DataAccess.Handlers;

public interface ITokenGeneratedData 
{
    Task<AppResult<CreateTokenResult>> CreateSubCategory(CreateTokenArgs args);
    Task<AppResult<GetTokenResult>> GetAllSubCategory(string guid, string token);
}