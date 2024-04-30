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

        public async Task<AppResult<GetDisbursementDetailsResult>> GetDisbursementDetails(int disbursementId, string token)
        {
            try
            {
                var result = await flurlClient
                    .WithOAuthBearerToken(token)
                    .Request($"Admin/GetDisbursements/{disbursementId}")
                    .GetJsonAsync<GetDisbursementDetailsResult>();

                return AppResult<GetDisbursementDetailsResult>.CreateSucceeded(result, "Successfully called disbursements information api");
            }
            catch (FlurlHttpException ex)
            {
                var error = await ex.GetResponseJsonAsync();
                return AppResult<GetDisbursementDetailsResult>.CreateFailed(ex, ex.Message);
            }
            catch (Exception ex)
            {
                return AppResult<GetDisbursementDetailsResult>.CreateFailed(ex, "An error occured when calling disbursements information api");
            }
        }

        public async Task<AppResult<ManaulDisbursementResult>> ManaulDisbursementResult(ManualDisbursementArgs args, string token)
        {
            try
            {
                var result = await flurlClient
                    .WithOAuthBearerToken(token)
                    .Request("Admin/ManualDisbursement")
                    .PostJsonAsync(args)
                    .ReceiveJson<ManaulDisbursementResult>();

                return AppResult<ManaulDisbursementResult>.CreateSucceeded(result, "Successfully post manual disbursement api");
            }
            catch (FlurlHttpException ex)
            {
                var error = await ex.GetResponseJsonAsync();
                return AppResult<ManaulDisbursementResult>.CreateFailed(ex, ex.Message);
            }
            catch (Exception ex)
            {
                return AppResult<ManaulDisbursementResult>.CreateFailed(ex, "An error occured when posting manual disbursement api");
            }
        }

        public async Task<AppResult<CreateAnnouncementResult>> CreateAnnouncement(CreateAnnouncementArgs args, string token)
        {
            try
            {
                var result = await flurlClient
                    .WithOAuthBearerToken(token)
                    .Request("Admin/CreateAnnouncement")
                    .PostJsonAsync(args)
                    .ReceiveJson<CreateAnnouncementResult>();

                return AppResult<CreateAnnouncementResult>.CreateSucceeded(result, "Successfully called create announcement api");
            }
            catch (FlurlHttpException ex)
            {
                var error = await ex.GetResponseJsonAsync();
                return AppResult<CreateAnnouncementResult>.CreateFailed(ex, ex.Message);
            }
            catch (Exception ex)
            {
                return AppResult<CreateAnnouncementResult>.CreateFailed(ex, "An error occured when calling create announcement api");
            }
        }

        public async Task<AppResult<UpdateAnnouncementResult>> UpdateAnnouncement(UpdateAnnouncementArgs args, string token)
        {
            try
            {
                var result = await flurlClient
                    .WithOAuthBearerToken(token)
                    .Request("Admin/UpdateAnnouncement")
                    .PostJsonAsync(args)
                    .ReceiveJson<UpdateAnnouncementResult>();

                return AppResult<UpdateAnnouncementResult>.CreateSucceeded(result, "Successfully called update announcement api");
            }
            catch (FlurlHttpException ex)
            {
                var error = await ex.GetResponseJsonAsync();
                return AppResult<UpdateAnnouncementResult>.CreateFailed(ex, ex.Message);
            }
            catch (Exception ex)
            {
                return AppResult<UpdateAnnouncementResult>.CreateFailed(ex, "An error occured when calling update announcement api");
            }
        }

        public async Task<AppResult<GetAllAnnouncementsResult>> GetAllAnnouncements(string token)
        {
            try
            {
                var result = await flurlClient
                    .WithOAuthBearerToken(token)
                    .Request("Admin/GetAllAnnouncements")
                    .GetJsonAsync<GetAllAnnouncementsResult>();

                return AppResult<GetAllAnnouncementsResult>.CreateSucceeded(result, "Successfully called get announcements api");
            }
            catch (FlurlHttpException ex)
            {
                var error = await ex.GetResponseJsonAsync();
                return AppResult<GetAllAnnouncementsResult>.CreateFailed(ex, ex.Message);
            }
            catch (Exception ex)
            {
                return AppResult<GetAllAnnouncementsResult>.CreateFailed(ex, "An error occured when calling get announcements api");
            }
        }

        public async Task<AppResult<DeleteAnnouncementResult>> DeleteAnnouncement(DeleteAnnouncementArgs args, string token)
        {
            try
            {
                var result = await flurlClient
                    .WithOAuthBearerToken(token)
                    .Request("Admin/DeleteAnnouncement")
                    .PostJsonAsync(args)
                    .ReceiveJson<DeleteAnnouncementResult>();

                return AppResult<DeleteAnnouncementResult>.CreateSucceeded(result, "Successfully called delete announcement api");
            }
            catch (FlurlHttpException ex)
            {
                var error = await ex.GetResponseJsonAsync();
                return AppResult<DeleteAnnouncementResult>.CreateFailed(ex, ex.Message);
            }
            catch (Exception ex)
            {
                return AppResult<DeleteAnnouncementResult>.CreateFailed(ex, "An error occured when calling delete announcement api");
            }
        }

        public async Task<AppResult<EventPoliciesResult>> EventPolicies(EventPoliciesArgs args, string token)
        {
            try
            {
                var result = await flurlClient
                    .WithOAuthBearerToken(token)
                    .Request("Admin/EventPolicies")
                    .PostJsonAsync(args)
                    .ReceiveJson<EventPoliciesResult>();

                return AppResult<EventPoliciesResult>.CreateSucceeded(result, "Successfully post event policies api.");
            }
            catch (FlurlHttpException ex)
            {
                var error = await ex.GetResponseJsonAsync();
                return AppResult<EventPoliciesResult>.CreateFailed(ex, ex.Message);
            }
            catch (Exception ex)
            {
                return AppResult<EventPoliciesResult>.CreateFailed(ex, "An error occured when posting event policies api.");
            }
        }

        public async Task<AppResult<EventPoliciesResult>> EventBuyerPolicies(EventBuyerPoliciesArgs args, string token)
        {
            try
            {
                var result = await flurlClient
                    .WithOAuthBearerToken(token)
                    .Request("Admin/EventBuyerPolicies")
                    .PostJsonAsync(args)
                    .ReceiveJson<EventPoliciesResult>();

                return AppResult<EventPoliciesResult>.CreateSucceeded(result, "Successfully post event policies api.");
            }
            catch (FlurlHttpException ex)
            {
                var error = await ex.GetResponseJsonAsync();
                return AppResult<EventPoliciesResult>.CreateFailed(ex, ex.Message);
            }
            catch (Exception ex)
            {
                return AppResult<EventPoliciesResult>.CreateFailed(ex, "An error occured when posting event policies api.");
            }
        }

        public async Task<AppResult<EventPoliciesResult>> EventSellerPolicies(EventSellerPoliciesArgs args, string token)
        {
            try
            {
                var result = await flurlClient
                    .WithOAuthBearerToken(token)
                    .Request("Admin/EventSellerPolicies")
                    .PostJsonAsync(args)
                    .ReceiveJson<EventPoliciesResult>();

                return AppResult<EventPoliciesResult>.CreateSucceeded(result, "Successfully post event policies api.");
            }
            catch (FlurlHttpException ex)
            {
                var error = await ex.GetResponseJsonAsync();
                return AppResult<EventPoliciesResult>.CreateFailed(ex, ex.Message);
            }
            catch (Exception ex)
            {
                return AppResult<EventPoliciesResult>.CreateFailed(ex, "An error occured when posting event policies api.");
            }
        }

        public async Task<AppResult<PrivacyPoliciesResult>> PrivacyPolicies(PrivacyPoliciesArgs args, string token)
        {
            try
            {
                var result = await flurlClient
                    .WithOAuthBearerToken(token)
                    .Request("Admin/PrivacyPolicies")
                    .PostJsonAsync(args)
                    .ReceiveJson<PrivacyPoliciesResult>();

                return AppResult<PrivacyPoliciesResult>.CreateSucceeded(result, "Successfully post event policies api.");
            }
            catch (FlurlHttpException ex)
            {
                var error = await ex.GetResponseJsonAsync();
                return AppResult<PrivacyPoliciesResult>.CreateFailed(ex, ex.Message);
            }
            catch (Exception ex)
            {
                return AppResult<PrivacyPoliciesResult>.CreateFailed(ex, "An error occured when posting event policies api.");
            }
        }
    }
}
