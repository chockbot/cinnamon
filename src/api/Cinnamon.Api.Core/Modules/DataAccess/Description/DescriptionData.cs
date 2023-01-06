using Cinnamon.Api.Core.Config;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Framework.ApiCommand.ApiData.Address.Request;
using Cinnamon.Framework.ApiCommand.ApiData.Address.Response;
using Flurl.Http.Configuration;
using Flurl.Http;
using Cinnamon.Framework.Common;
using Cinnamon.Framework.ApiCommand.ApiData.Description.Response;
using Cinnamon.Framework.ApiCommand.ApiData.Description.Request;

namespace Cinnamon.Api.Core.Modules.DataAccess.Description
{
    public class DescriptionData: IDescriptionData
    {
        private readonly IFlurlClient flurlClient;

        public DescriptionData(ApplicationConfig config, IFlurlClientFactory flurlFac)
        {
            flurlClient = flurlFac.Get(config.ApiDataUrl);
        }

        public async Task<AppResult<CreateDescriptionResult>> CreateDescription(CreateDescriptionArgs args)
        {
            try
            {
                var result = await flurlClient
                                .Request("Description/CreateDescription")
                                .PostJsonAsync(args)
                                .ReceiveJson<CreateDescriptionResult>();

                return AppResult<CreateDescriptionResult>.CreateSucceeded(result, "Successfully posting create description api");
            }
            catch (FlurlHttpException ex)
            {
                return AppResult<CreateDescriptionResult>.CreateFailed(ex, ex.Message);
            }
            catch (Exception ex)
            {
                return AppResult<CreateDescriptionResult>.CreateFailed(ex, "An error occured when posting create description api");
            }
        }

        public async Task<AppResult<GetAllDescriptionResult>> GetAllDescription()
        {
            try
            {
                var result = await flurlClient
                                .Request("Address/GetAllAddress")
                                .GetJsonAsync<GetAllDescriptionResult>();

                return AppResult<GetAllDescriptionResult>.CreateSucceeded(result, "Successfully getting get all description api");
            }
            catch (FlurlHttpException ex)
            {
                return AppResult<GetAllDescriptionResult>.CreateFailed(ex, ex.Message);
            }
            catch (Exception ex)
            {
                return AppResult<GetAllDescriptionResult>.CreateFailed(ex, "An error occured when getting all description api");
            }
        }

        public async Task<AppResult<GetDescriptionResult>> GetDescriptionById(int id)
        {
            try
            {
                var result = await flurlClient
                                .Request($"Description/GetDescriptionById/{id}")
                                .GetJsonAsync<GetDescriptionResult>();

                return AppResult<GetDescriptionResult>.CreateSucceeded(result, "Successfully getting description by id api");
            }
            catch (FlurlHttpException ex)
            {
                return AppResult<GetDescriptionResult>.CreateFailed(ex, ex.Message);
            }
            catch (Exception ex)
            {
                return AppResult<GetDescriptionResult>.CreateFailed(ex, "An error occured when getting description by id api");
            }
        }

        public async Task<AppResult<GetDescriptionResult>> GetDescriptionByActivityId(int id)
        {
            try
            {
                var result = await flurlClient
                                .Request($"Description/GetDescriptionByActivityId/{id}")
                                .GetJsonAsync<GetDescriptionResult>();

                return AppResult<GetDescriptionResult>.CreateSucceeded(result, "Successfully getting description by id api");
            }
            catch (FlurlHttpException ex)
            {
                return AppResult<GetDescriptionResult>.CreateFailed(ex, ex.Message);
            }
            catch (Exception ex)
            {
                return AppResult<GetDescriptionResult>.CreateFailed(ex, "An error occured when getting description by id api");
            }
        }

        public async Task<AppResult<UpdatedDescriptionResult>> UpdateDescription(UpdateDescriptionArgs args)
        {
            try
            {
                var result = await flurlClient
                                .Request("Description/UpdateDescription")
                                .PostJsonAsync(args)
                                .ReceiveJson<UpdatedDescriptionResult>();

                return AppResult<UpdatedDescriptionResult>.CreateSucceeded(result, "Successfully posting update description api");
            }
            catch (FlurlHttpException ex)
            {
                return AppResult<UpdatedDescriptionResult>.CreateFailed(ex, ex.Message);
            }
            catch (Exception ex)
            {
                return AppResult<UpdatedDescriptionResult>.CreateFailed(ex, "An error occured when posting update description api");
            }
        }
    }
}
