using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Interactors;
using Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.ActivityService;

public class ValidateCouponCodeHandler : IValidateCouponCodeHandler
{
    private readonly ICouponData couponData;

    public ValidateCouponCodeHandler(ICouponData couponData)
    {
        this.couponData = couponData;
    }

    public AppResult<ValidateCouponCodeResult> Execute(ValidateCouponCodeArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<ValidateCouponCodeResult>.CreateFailed(ex, "An error occured in ValidateCouponCodeHandler");
        }
    }

    public async Task<AppResult<ValidateCouponCodeResult>> ExecuteAsync(ValidateCouponCodeArgs args)
    {
        try
        {
            var couponRes = await couponData.GetCouponByCode(new Framework.ApiCommand.ApiData.Coupon.Request.GetCouponByCodeArgs {
                Code = args.CouponCode
            });
            if(!couponRes.Succeeded || couponRes.Result == null || !couponRes.Result.IsSuccess)
            {
                return AppResult<ValidateCouponCodeResult>.CreateFailed(new ApplicationException("Invalid coupon code."), "Invalid coupon code.");
            }
            var coupon = couponRes.Result.Result;

            if(coupon.Status == 0)
            {
                return AppResult<ValidateCouponCodeResult>.CreateFailed(new ApplicationException("Invalid coupon code."), "Invalid coupon code.");
            }

            if(coupon.ActivityId != 0 && coupon.ActivityId != args.ActivityId)
            {
                return AppResult<ValidateCouponCodeResult>.CreateFailed(new ApplicationException("Invalid coupon code."), "Invalid coupon code.");
            }

            if(coupon.DiscountType == 0 && coupon.MaximumSpend > args.Amount)
            {
                return AppResult<ValidateCouponCodeResult>.CreateFailed(new ApplicationException("Invalid coupon code."), "Invalid coupon code.");
            }

            return AppResult<ValidateCouponCodeResult>.CreateSucceeded(new ValidateCouponCodeResult {
                Amount = coupon.Amount,
                DiscountType = coupon.DiscountType,
                IsValid = true,
                MaximumSpend = coupon.MaximumSpend
            }, "Successfully validate coupon code");
        }
        catch (Exception ex)
        {
            return AppResult<ValidateCouponCodeResult>.CreateFailed(ex, "An error occured in ValidateCouponCodeHandler");
        }
    }
}