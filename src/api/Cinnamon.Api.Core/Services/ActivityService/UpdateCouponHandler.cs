using System.Security.Claims;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Interactors;
using Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.ActivityService;

public class UpdateCouponHandler : IUpdateCouponHandler
{
    private readonly IHttpContextAccessor httpContext;
    private readonly ICouponData couponData;

    public UpdateCouponHandler(IHttpContextAccessor httpContext, ICouponData couponData)
    {
        this.httpContext = httpContext;
        this.couponData = couponData;
    }

    public AppResult<UpdateCouponResult> Execute(UpdateCouponArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<UpdateCouponResult>.CreateFailed(ex, "An error occured in UpdateCouponHandler");
        }
    }

    public async Task<AppResult<UpdateCouponResult>> ExecuteAsync(UpdateCouponArgs args)
    {
        try
        {
            // get customer id saved in claims
            var customerId = httpContext.HttpContext?.User.FindFirstValue("UserId");
            int id = customerId != null ? Convert.ToInt32(customerId) : 0;

            var getCouponRes = await couponData.GetCouponById(args.Id);
            if(!getCouponRes.Succeeded || getCouponRes.Result == null || !getCouponRes.Result.IsSuccess)
            {
                return AppResult<UpdateCouponResult>.CreateFailed(new ApplicationException("Unable to find coupon. Invalid request"), "Unable to find coupon. Invalid request");
            }
            var coupon = getCouponRes.Result.Result;

            // not allowed to update if not owner of the coupon
            if(coupon.CustomerId != id)
            {
                return AppResult<UpdateCouponResult>.CreateFailed(new ApplicationException("Action not allowed. Invalid request"), "Action not allowed. Invalid request");
            }

            var updateRes = await couponData.UpdateCoupon(new Framework.ApiCommand.ApiData.Coupon.Request.UpdateCouponArgs {
                Id = args.Id,
                Name = args.Name,
                From = args.From,
                To= args.To
            });
            if(!updateRes.Succeeded || updateRes.Result == null || !updateRes.Result.IsSuccess)
            {
                return AppResult<UpdateCouponResult>.CreateFailed(new ApplicationException(updateRes.Result?.ErrorInfo?.Message), updateRes.Message);
            }
            var updatedCoupon = updateRes.Result.Result;

            return AppResult<UpdateCouponResult>.CreateSucceeded(new UpdateCouponResult {
                ActivityId = updatedCoupon.ActivityId,
                Amount = updatedCoupon.Amount,
                Code = updatedCoupon.Code,
                CustomerId = updatedCoupon.CustomerId,
                DiscountType = updatedCoupon.DiscountType,
                FromDate = updatedCoupon.From,
                Id = updatedCoupon.Id,
                IsAdmin = updatedCoupon.IsAdmin,
                MaximumSpend = updatedCoupon.MaximumSpend,
                Name = updatedCoupon.Name,
                Status = updatedCoupon.Status,
                ToDate = updatedCoupon.To
            }, "Successfully update coupon");
        }
        catch (Exception ex)
        {
            return AppResult<UpdateCouponResult>.CreateFailed(ex, "An error occured in UpdateCouponHandler");
        }
    }
}