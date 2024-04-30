using Cinnamon.Framework.ApiCommand.ApiCore.AdminUser.Request;
using Cinnamon.Framework.ApiCommand.ApiCore.AdminUser.Response;
using Cinnamon.Framework.Common;

namespace Cinnamon.Web.Modules.ApiAccess.Handlers
{
    public interface IAdminApiHandler
    {
        Task<AppResult<GetAdminUserByEmailResult>> GetAdminUserByEmail(GetAdminUserByEmailArgs args, string token);
        Task<AppResult<UpdateCustomerPricingResult>> CustomerPricing(UpdateCustomerPricingArgs args, string token);
        Task<AppResult<GetAllInclusiveTransactionResult>> GetAllInclusiveTransactions(GetAllInclusiveTransactionArgs args, string token);
        Task<AppResult<AdminCreateCouponResult>> CreateCoupon(AdminCreateCouponArgs args, string token);
        Task<AppResult<GetDisbursementResult>> GetDisbursements(GetDisbursementArgs args, string token);
        Task<AppResult<GetDisbursementDetailsResult>> GetDisbursementDetails(int disbursementId, string token);
        Task<AppResult<ManaulDisbursementResult>> ManaulDisbursementResult(ManualDisbursementArgs args, string token);
        Task<AppResult<CreateAnnouncementResult>> CreateAnnouncement(CreateAnnouncementArgs args, string token);
        Task<AppResult<UpdateAnnouncementResult>> UpdateAnnouncement(UpdateAnnouncementArgs args, string token);
        Task<AppResult<GetAllAnnouncementsResult>> GetAllAnnouncements(string token);
        Task<AppResult<DeleteAnnouncementResult>> DeleteAnnouncement(DeleteAnnouncementArgs args, string token);
        Task<AppResult<EventPoliciesResult>> EventPolicies(EventPoliciesArgs args, string token);
        Task<AppResult<EventPoliciesResult>> EventBuyerPolicies(EventBuyerPoliciesArgs args, string token);
        Task<AppResult<EventPoliciesResult>> EventSellerPolicies(EventSellerPoliciesArgs args, string token);
        Task<AppResult<PrivacyPoliciesResult>> PrivacyPolicies(PrivacyPoliciesArgs args, string token);
    }
}
