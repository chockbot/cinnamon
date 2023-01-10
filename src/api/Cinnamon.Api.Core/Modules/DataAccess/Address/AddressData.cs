using Cinnamon.Api.Core.Config;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Framework.ApiCommand.ApiData.Address.Request;
using Cinnamon.Framework.ApiCommand.ApiData.Address.Response;
using Cinnamon.Framework.ApiCommand.ApiData.Customer.Response;
using Cinnamon.Framework.Common;
using Flurl.Http;
using Flurl.Http.Configuration;

namespace Cinnamon.Api.Core.Modules.DataAccess.Address
{
    public class AddressData : IAddressData
    {
        private readonly IFlurlClient flurlClient;

        public AddressData(ApplicationConfig config, IFlurlClientFactory flurlFac)
        {
            flurlClient = flurlFac.Get(config.ApiDataUrl);
        }

        public async Task<AppResult<CreateAddressResult>> CreateAddress(CreateAddressArgs args)
        {
            try
            {
                var result = await flurlClient
                                .Request("Address/CreateAddress")
                                .PostJsonAsync(args)
                                .ReceiveJson<CreateAddressResult>();

                return AppResult<CreateAddressResult>.CreateSucceeded(result, "Successfully posting create address api");
            }
            catch (FlurlHttpException ex)
            {
                return AppResult<CreateAddressResult>.CreateFailed(ex, ex.Message);
            }
            catch (Exception ex)
            {
                return AppResult<CreateAddressResult>.CreateFailed(ex, "An error occured when posting create address api");
            }
        }

        public async Task<AppResult<GetAllAddressResult>> GetAllAddress(GetAllAddressArgs args)
        {
            try
            {
                var result = await flurlClient
                                .Request("Address/GetAllAddress")
                                .GetJsonAsync<GetAllAddressResult>();

                return AppResult<GetAllAddressResult>.CreateSucceeded(result, "Successfully getting get all address api");
            }
            catch (FlurlHttpException ex)
            {
                return AppResult<GetAllAddressResult>.CreateFailed(ex, ex.Message);
            }
            catch (Exception ex)
            {
                return AppResult<GetAllAddressResult>.CreateFailed(ex, "An error occured when getting all address api");
            }
        }

        public async Task<AppResult<GetAddressResult>> GetAddressById(int id)
        {
            try
            {
                var result = await flurlClient
                                .Request($"Address/GetAddressById/{id}")
                                .GetJsonAsync<GetAddressResult>();

                return AppResult<GetAddressResult>.CreateSucceeded(result, "Successfully getting address by id api");
            }
            catch (FlurlHttpException ex)
            {
                return AppResult<GetAddressResult>.CreateFailed(ex, ex.Message);
            }
            catch (Exception ex)
            {
                return AppResult<GetAddressResult>.CreateFailed(ex, "An error occured when getting address by id api");
            }
        }

        public async Task<AppResult<GetAddressResult>> GetAddressByActivityId(int id)
        {
            try
            {
                var result = await flurlClient
                                .Request($"Address/GetAddressByActivityId/{id}")
                                .GetJsonAsync<GetAddressResult>();

                return AppResult<GetAddressResult>.CreateSucceeded(result, "Successfully getting address by id api");
            }
            catch (FlurlHttpException ex)
            {
                return AppResult<GetAddressResult>.CreateFailed(ex, ex.Message);
            }
            catch (Exception ex)
            {
                return AppResult<GetAddressResult>.CreateFailed(ex, "An error occured when getting address by id api");
            }
        }

        public async Task<AppResult<UpdatedAddressResult>> UpdateAddress(UpdateAddressArgs args)
        {
            try
            {
                var result = await flurlClient
                                .Request("Address/UpdateAddress")
                                .PostJsonAsync(args)
                                .ReceiveJson<UpdatedAddressResult>();

                return AppResult<UpdatedAddressResult>.CreateSucceeded(result, "Successfully posting update address api");
            }
            catch (FlurlHttpException ex)
            {
                return AppResult<UpdatedAddressResult>.CreateFailed(ex, ex.Message);
            }
            catch (Exception ex)
            {
                return AppResult<UpdatedAddressResult>.CreateFailed(ex, "An error occured when posting update address api");
            }
        }
    }
}
