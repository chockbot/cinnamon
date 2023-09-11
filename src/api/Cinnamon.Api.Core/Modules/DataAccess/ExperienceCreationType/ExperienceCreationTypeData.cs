using Cinnamon.Api.Core.Config;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Framework.ApiCommand.ApiData.ExperienceCreationType.Request;
using Cinnamon.Framework.ApiCommand.ApiData.ExperienceCreationType.Response;
using Cinnamon.Framework.Common;
using Flurl.Http;
using Flurl.Http.Configuration;

namespace Cinnamon.Api.Core.Modules.DataAccess.ExperienceCreationType
{
    public class ExperienceCreationTypeData : IExperienceCreationTypeData
    {
        private readonly IFlurlClient flurlClient;

        public ExperienceCreationTypeData(ApplicationConfig config, IFlurlClientFactory flurlFac)
        {
            flurlClient = flurlFac.Get(config.ApiDataUrl);
        }
        public async Task<AppResult<GetExperienceCreationTypeResult>> GetExperienceCreationTypes(GetExperienceCreationTypeArgs args)
        {
            try
            {
                var result = await flurlClient
                            .Request("ExperienceCreationType")
                            .SetQueryParams(args)
                            .GetJsonAsync<GetExperienceCreationTypeResult>();

                return AppResult<GetExperienceCreationTypeResult>.CreateSucceeded(result, "Successfully called get experience creation types api");
            }
            catch (FlurlHttpException ex)
            {
                return AppResult<GetExperienceCreationTypeResult>.CreateFailed(ex, ex.Message);
            }
            catch (Exception ex)
            {
                return AppResult<GetExperienceCreationTypeResult>.CreateFailed(ex, "An error occured when getting experience creation types api");
            }
        }
    }
}
