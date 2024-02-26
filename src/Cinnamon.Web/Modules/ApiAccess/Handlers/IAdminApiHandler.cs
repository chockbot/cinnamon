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
        Task<AppResult<CreateAnnouncementResult>> CreateAnnouncement(CreateAnnouncementArgs args, string token);
        Task<AppResult<UpdateAnnouncementResult>> UpdateAnnouncement(UpdateAnnouncementArgs args, string token);
        Task<AppResult<GetAllAnnouncementsResult>> GetAllAnnouncements(string token);
        Task<AppResult<DeleteAnnouncementResult>> DeleteAnnouncement(DeleteAnnouncementArgs args, string token);
    }
}
