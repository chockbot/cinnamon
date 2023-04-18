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
    public class BarangayData : IBarangayData
    {
        private readonly IFlurlClient flurlClient;
        public BarangayData(ApplicationConfig config, IFlurlClientFactory flurlFac)
        {
            flurlClient = flurlFac.Get(config.ApiDataUrl);
        }

        public async Task<AppResult<GetAllBarangayResult>> GetAllBarangaysByCityCode(GetAllBarangayArgs args)
        {
            try
            {
                var result = await flurlClient
                                .Request("Location/barangays")
                                .SetQueryParams(args)
                                .GetJsonAsync<GetAllBarangayResult>();

                return AppResult<GetAllBarangayResult>.CreateSucceeded(result, "Successfully getting get all barangays api");
            }
            catch (FlurlHttpException ex)
            {
                return AppResult<GetAllBarangayResult>.CreateFailed(ex, ex.Message);
            }
            catch (Exception ex)
            {
                return AppResult<GetAllBarangayResult>.CreateFailed(ex, "An error occured when getting all barangays api");
            }
        }
    }
}
