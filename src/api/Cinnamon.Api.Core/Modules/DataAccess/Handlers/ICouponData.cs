using Cinnamon.Framework.ApiCommand.ApiData.Coupon.Request;
using Cinnamon.Framework.ApiCommand.ApiData.Coupon.Response;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Modules.DataAccess.Handlers;

public interface ICouponData 
{
    Task<AppResult<CreateCouponResult>> CreateCoupon(CreateCouponArgs args);
    Task<AppResult<UpdateCouponResult>> UpdateCoupon(UpdateCouponArgs args);
    Task<AppResult<GetCouponResult>> GetCouponById(int id);
    Task<AppResult<GetAllCouponResult>> GetAllCoupon(GetAllCouponArgs args);
    Task<AppResult<IsPromotionCodeExistResult>> IsPromotionCodeExist(IsPromotionCodeExistArgs args);
}