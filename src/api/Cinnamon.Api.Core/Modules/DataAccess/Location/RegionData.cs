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
    public class RegionData : IRegionData
    {
        private readonly IFlurlClient flurlClient;
        public RegionData(ApplicationConfig config, IFlurlClientFactory flurlFac)
        {
            flurlClient = flurlFac.Get(config.ApiDataUrl);
        }

        public async Task<AppResult<GetAllRegionResult>> GetAllRegions(GetAllRegionArgs args)
        {
            try
            {
                var result = await flurlClient
                                .Request("Location/Regions")
                                .SetQueryParams(args)
                                .GetJsonAsync<GetAllRegionResult>();

                return AppResult<GetAllRegionResult>.CreateSucceeded(result, "Successfully getting get all regions api");
            }
            catch (FlurlHttpException ex)
            {
                return AppResult<GetAllRegionResult>.CreateFailed(ex, ex.Message);
            }
            catch (Exception ex)
            {
                return AppResult<GetAllRegionResult>.CreateFailed(ex, "An error occured when getting all regions api");
            }
        }
    }
}
