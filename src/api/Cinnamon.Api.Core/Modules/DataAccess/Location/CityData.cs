using Cinnamon.Api.Core.Config;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Framework.ApiCommand.ApiData.Location.Response;
using Cinnamon.Framework.ApiCommand.ApiData.Student.Request;
using Cinnamon.Framework.Common;
using Flurl.Http.Configuration;
using Flurl.Http;
using Cinnamon.Framework.ApiCommand.ApiData.Student.Response;
using Cinnamon.Framework.ApiCommand.ApiData.Location.Request;

namespace Cinnamon.Api.Core.Modules.DataAccess.Location
{
    public class CityData : ICityData
    {
        private readonly IFlurlClient flurlClient;
        public CityData(ApplicationConfig config, IFlurlClientFactory flurlFac)
        {
            flurlClient = flurlFac.Get(config.ApiDataUrl);
        }

        public async Task<AppResult<GetAllCitiesResult>> GetAllCitiesByRegionCode(GetAllCitiesArgs args)
        {
            try
            {
                var result = await flurlClient
                                .Request("Location/Cities")
                                .SetQueryParams(args)
                                .GetJsonAsync<GetAllCitiesResult>();

                return AppResult<GetAllCitiesResult>.CreateSucceeded(result, "Successfully getting get all cities api");
            }
            catch (FlurlHttpException ex)
            {
                return AppResult<GetAllCitiesResult>.CreateFailed(ex, ex.Message);
            }
            catch (Exception ex)
            {
                return AppResult<GetAllCitiesResult>.CreateFailed(ex, "An error occured when getting all cities api");
            }
        }
    }
}
