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

        public async Task<AppResult<GetAllInclusiveTransactionResult>> GetAllInclusiveTransactions(GetAllInclusiveTransactionArgs args, string token)
        {
            try
            {
                var result = await flurlClient
                .WithOAuthBearerToken(token)
                .Request("Admin/GetAllInclusiveTransactions")
                .SetQueryParams(args)
                .GetJsonAsync<GetAllInclusiveTransactionResult>();

                return AppResult<GetAllInclusiveTransactionResult>.CreateSucceeded(result, "Successfully called get admin inclusive transactiond api");
            }
            catch (FlurlHttpException ex)
            {
                var error = await ex.GetResponseJsonAsync();
                return AppResult<GetAllInclusiveTransactionResult>.CreateFailed(ex, ex.Message);
            }
            catch (Exception ex)
            {
                return AppResult<GetAllInclusiveTransactionResult>.CreateFailed(ex, "An error occured when calling get admin inclusive transactiond api");
            }
        }

        public async Task<AppResult<AdminCreateCouponResult>> CreateCoupon(AdminCreateCouponArgs args, string token)
        {
            try
            {
                var result = await flurlClient
                    .WithOAuthBearerToken(token)
                    .Request("Admin/CreateCoupon")
                    .PostJsonAsync(args)
                    .ReceiveJson<AdminCreateCouponResult>();

                return AppResult<AdminCreateCouponResult>.CreateSucceeded(result, "Successfully called create coupon api");
            }
            catch (FlurlHttpException ex)
            {
                var error = await ex.GetResponseJsonAsync();
                return AppResult<AdminCreateCouponResult>.CreateFailed(ex, ex.Message);
            }
            catch (Exception ex)
            {
                return AppResult<AdminCreateCouponResult>.CreateFailed(ex, "An error occured when calling create coupon api");
            }
        }

        public async Task<AppResult<GetDisbursementResult>> GetDisbursements(GetDisbursementArgs args, string token)
        {
            try
            {
                var result = await flurlClient
                    .WithOAuthBearerToken(token)
                    .Request("Admin/GetDisbursements")
                    .SetQueryParams(args)
                    .GetJsonAsync<GetDisbursementResult>();

                return AppResult<GetDisbursementResult>.CreateSucceeded(result, "Successfully called disbursements information api");
            }
            catch (FlurlHttpException ex)
            {
                var error = await ex.GetResponseJsonAsync();
                return AppResult<GetDisbursementResult>.CreateFailed(ex, ex.Message);
            }
            catch (Exception ex)
            {
                return AppResult<GetDisbursementResult>.CreateFailed(ex, "An error occured when calling disbursements information api");
            }
        }
    }
}
