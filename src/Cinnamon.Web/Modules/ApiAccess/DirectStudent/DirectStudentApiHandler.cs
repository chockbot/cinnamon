using Cinnamon.Framework.ApiCommand.ApiCore.DirectStudent.Request;
using Cinnamon.Framework.ApiCommand.ApiCore.DirectStudent.Response;
using Cinnamon.Framework.Common;
using Cinnamon.Web.Modules.ApiAccess.Handlers;
using Flurl.Http;
using Flurl.Http.Configuration;

namespace Cinnamon.Web.Modules.ApiAccess.DirectStudent;

public class DirectStudentApiHandler : IDirectStudentApiHandler
{
    private readonly IFlurlClient flurlClient;

    public DirectStudentApiHandler(IFlurlClientFactory flurlFac, Config.Config config)
    {
        flurlClient = flurlFac.Get(config.ApiUrl);
    }

    public async Task<AppResult<DirectStudentInfoReult>> DirectStudents(DirectStudentInfoArgs args, string token)
    {
        try
        {
            var result = await flurlClient
                .WithOAuthBearerToken(token)
                .Request("DirectStudents")
                .SetQueryParams(args)
                .GetJsonAsync<DirectStudentInfoReult>();

            return AppResult<DirectStudentInfoReult>.CreateSucceeded(result, "Successfully getting direct students api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<DirectStudentInfoReult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<DirectStudentInfoReult>.CreateFailed(ex, "An error occured when getting direct students api");
        }
    }
}