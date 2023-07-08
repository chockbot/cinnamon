using System.Security.Claims;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Interactors;
using Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.ActivityService;

public class UpdateCouponStatusHandler : IUpdateCouponStatusHandler
{
    private readonly ICouponData couponData;
    private readonly IHttpContextAccessor httpContext;

    public UpdateCouponStatusHandler(ICouponData couponData, IHttpContextAccessor httpContext)
    {
        this.couponData = couponData;
        this.httpContext = httpContext;
    }

    public AppResult<UpdateCouponStatusResult> Execute(UpdateCouponStatusArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<UpdateCouponStatusResult>.CreateFailed(ex, "An error occured in UpdateCouponStatusHandler");
        }
    }

    public async Task<AppResult<UpdateCouponStatusResult>> ExecuteAsync(UpdateCouponStatusArgs args)
    {
        try
        {
            // get customer id saved in claims
            var customerId = httpContext.HttpContext?.User.FindFirstValue("UserId");
            int id = customerId != null ? Convert.ToInt32(customerId) : 0;

            // validate status
            if(args.Status != 0 && args.Status != 1)
            {
                return AppResult<UpdateCouponStatusResult>.CreateFailed(new ApplicationException("Invalid request"), "Invalid request");
            }

            var couponRes = await couponData.GetCouponById(args.Id);
            if(!couponRes.Succeeded || couponRes.Result == null || !couponRes.Result.IsSuccess)
            {
                return AppResult<UpdateCouponStatusResult>.CreateFailed(new ApplicationException(couponRes.Result?.ErrorInfo?.Message), couponRes.Message);
            }
            var coupon = couponRes.Result.Result;

            if(coupon.CustomerId != id)
            {
                return AppResult<UpdateCouponStatusResult>.CreateFailed(new ApplicationException("Invalid request"), "Invalid request");
            }

            var updateRes = await couponData.UpdateCoupon(new Framework.ApiCommand.ApiData.Coupon.Request.UpdateCouponArgs {
                Id = args.Id,
                Status = args.Status
            });
            if(!updateRes.Succeeded || updateRes.Result == null || !updateRes.Result.IsSuccess)
            {
                return AppResult<UpdateCouponStatusResult>.CreateFailed(new ApplicationException(updateRes.Result?.ErrorInfo?.Message), updateRes.Message);
            }

            return AppResult<UpdateCouponStatusResult>.CreateSucceeded(new UpdateCouponStatusResult {}, "Successfully update coupon status");
        }
        catch (Exception ex)
        {
            return AppResult<UpdateCouponStatusResult>.CreateFailed(ex, "An error occured in UpdateCouponStatusHandler");
        }
    }
}