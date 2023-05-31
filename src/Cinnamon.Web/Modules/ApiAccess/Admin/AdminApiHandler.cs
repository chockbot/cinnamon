using Cinnamon.Framework.ApiCommand.ApiCore.AdminUser.Request;
using Cinnamon.Framework.ApiCommand.ApiCore.AdminUser.Response;
using Cinnamon.Framework.Common;
using Cinnamon.Web.Modules.ApiAccess.Activity;
using Cinnamon.Web.Modules.ApiAccess.Handlers;
using Flurl.Http;
using Flurl.Http.Configuration;

namespace Cinnamon.Web.Modules.ApiAccess.Admin
{
    public class AdminApiHandler : IAdminApiHandler
    {
        private readonly IFlurlClient flurlClient;
        private readonly ILogger _logger;

        public AdminApiHandler(IFlurlClientFactory flurlFac, Config.Config config, ILogger<AdminApiHandler> logger)
        {
            flurlClient = flurlFac.Get(config.ApiUrl);
            _logger = logger;
        }

        public async Task<AppResult<GetAdminUserByEmailResult>> GetAdminUserByEmail(GetAdminUserByEmailArgs args, string token)
        {
            try
            {
                var result = await flurlClient
                .WithOAuthBearerToken(token)
                .Request("Admin/User")
                .SetQueryParams(args)
                .GetJsonAsync<GetAdminUserByEmailResult>();

                return AppResult<GetAdminUserByEmailResult>.CreateSucceeded(result, "Successfully called get admin user by email api");
            }
            catch (FlurlHttpException ex)
            {
                var error = await ex.GetResponseJsonAsync();
                return AppResult<GetAdminUserByEmailResult>.CreateFailed(ex, ex.Message);
            }
            catch (Exception ex)
            {
                return AppResult<GetAdminUserByEmailResult>.CreateFailed(ex, "An error occured when calling get admin user by email api");
            }
        }

        public async Task<AppResult<UpdateCustomerPricingResult>> CustomerPricing(UpdateCustomerPricingArgs args, string token)
        {
            try
            {
                var result = await flurlClient
                    .WithOAuthBearerToken(token)
                    .Request("Admin/CustomerPricing")
                    .PostJsonAsync(args)
                    .ReceiveJson<UpdateCustomerPricingResult>();

                return AppResult<UpdateCustomerPricingResult>.CreateSucceeded(result, "Successfully post customer pricing api");
            }
            catch (FlurlHttpException ex)
            {
                var error = await ex.GetResponseJsonAsync();
                return AppResult<UpdateCustomerPricingResult>.CreateFailed(ex, ex.Message);
            }
            catch (Exception ex)
            {
                return AppResult<UpdateCustomerPricingResult>.CreateFailed(ex, "An error occured when posting customer pricing api");
            }
        }
    }
}
