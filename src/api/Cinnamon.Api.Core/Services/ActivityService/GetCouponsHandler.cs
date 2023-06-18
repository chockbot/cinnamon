using System.Security.Claims;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Interactors;
using Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.ActivityService;

public class GetCouponsHandler : IGetCouponsHandler
{
    private readonly ICouponData couponData;
    private readonly IHttpContextAccessor httpContext;

    public GetCouponsHandler(ICouponData couponData, IHttpContextAccessor httpContext)
    {
        this.couponData = couponData;
        this.httpContext = httpContext;
    }

    public AppResult<GetCouponsResult> Execute(GetCouponsArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<GetCouponsResult>.CreateFailed(ex, "An error occured in GetCouponsHandler");
        }
    }

    public async Task<AppResult<GetCouponsResult>> ExecuteAsync(GetCouponsArgs args)
    {
        try
        {
            // get customer id saved in claims
            var customerId = httpContext.HttpContext?.User.FindFirstValue("UserId");
            int id = customerId != null ? Convert.ToInt32(customerId) : 0;

            var couponsRes = await couponData.GetAllCoupon(new Framework.ApiCommand.ApiData.Coupon.Request.GetAllCouponArgs {
                CustomerId = id,
                IncludeActivity = true
            });
            if(!couponsRes.Succeeded || couponsRes.Result == null || !couponsRes.Result.IsSuccess)
            {
                return AppResult<GetCouponsResult>.CreateFailed(new ApplicationException(couponsRes.Result?.ErrorInfo?.Message), couponsRes.Message);
            }

            return AppResult<GetCouponsResult>.CreateSucceeded(new GetCouponsResult {
                Coupons = couponsRes.Result.Result.Select(c => {
                    return new GetCouponsResult.Coupon {
                        ActivityId = c.ActivityId,
                        Amount = c.Amount,
                        AppliedActivity = c.ActivityApplied != null ? new GetCouponsResult.Coupon.Activity {
                            Id = c.ActivityApplied.Id,
                            Name = c.ActivityApplied.Title
                        } : null,
                        Code = c.Code,
                        CustomerId = c.CustomerId,
                        DiscountType = c.DiscountType,
                        FromDate = c.From,
                        Id = c.Id,
                        IsAdmin = c.IsAdmin,
                        MaximumSpend = c.MaximumSpend,
                        Name = c.Name,
                        Status = c.Status,
                        ToDate = c.To
                    };
                })
            }, "Successfully get all coupon codes");

        }
        catch (Exception ex)
        {
            return AppResult<GetCouponsResult>.CreateFailed(ex, "An error occured in GetCouponsHandler");
        }
    }
}