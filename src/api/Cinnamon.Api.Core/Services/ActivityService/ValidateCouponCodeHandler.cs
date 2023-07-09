using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Interactors;
using Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.ActivityService;

public class ValidateCouponCodeHandler : IValidateCouponCodeHandler
{
    private readonly ICouponData couponData;
    private readonly IGetActivityHandler getActivityHandler;
    private readonly IAdminUserData adminUserData;
    private readonly IGetCustomerByIdHandler getCustomerByIdHandler;

    public ValidateCouponCodeHandler(ICouponData couponData, IGetActivityHandler getActivityHandler,
        IAdminUserData adminUserData, IGetCustomerByIdHandler getCustomerByIdHandler)
    {
        this.couponData = couponData;
        this.getActivityHandler = getActivityHandler;
        this.adminUserData = adminUserData;
        this.getCustomerByIdHandler = getCustomerByIdHandler;
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

            // inactive coupon code
            if(coupon.Status == 0)
            {
                return AppResult<ValidateCouponCodeResult>.CreateFailed(new ApplicationException("Invalid coupon code."), "Invalid coupon code.");
            }

            // maximum spend not fulfilled
            if(coupon.DiscountType == 0 && coupon.MaximumSpend > args.Amount)
            {
                return AppResult<ValidateCouponCodeResult>.CreateFailed(new ApplicationException("Invalid coupon code."), "Invalid coupon code.");
            }

            // coupon code already expired
            DateTime now = DateTime.Now;
            if(!(coupon.From <= now && coupon.To >= now))
            {
                return AppResult<ValidateCouponCodeResult>.CreateFailed(new ApplicationException("Invalid coupon code."), "Invalid coupon code.");
            }

            // coupon code not applicable to selected activity
            if(coupon.ActivityId != 0 && coupon.ActivityId != args.ActivityId)
            {
                return AppResult<ValidateCouponCodeResult>.CreateFailed(new ApplicationException("Invalid coupon code."), "Invalid coupon code.");
            }

            // if activity id = 0 means applied to all
            // need to check who created the coupon code admin or maker
            if(coupon.ActivityId == 0)
            {
                var customerRes = await getCustomerByIdHandler.ExecuteAsync(new AccountService.Interactors.GetCustomerByIdArgs {
                    Id = coupon.CustomerId
                });
                if(!customerRes.Succeeded || customerRes.Result == null)
                {
                    return AppResult<ValidateCouponCodeResult>.CreateFailed(new ApplicationException("Invalid coupon code."), "Invalid coupon code.");
                }
                var customer = customerRes.Result;

                var adminRes = await adminUserData.GetAdminUserByEmail(new Framework.ApiCommand.ApiData.AdminUser.Request.GetAdminUserByEmailArgs {
                    Email = customer.Email
                });
                if(!adminRes.Succeeded || adminRes.Result == null)
                {
                    return AppResult<ValidateCouponCodeResult>.CreateFailed(new ApplicationException("Invalid request."), "Invalid request.");
                }
                bool isAdmin = adminRes.Succeeded && adminRes.Result != null && adminRes.Result.IsSuccess;

                // should apply the coupon code only in associated activity provider
                // else apply to all activities because coupon code created by admin
                if(!isAdmin)
                {
                    var activityRes = await getActivityHandler.ExecuteAsync(new GetActivityArgs {
                        ActivityId = args.ActivityId,
                        CustomerId = coupon.CustomerId
                    });
                    if(!activityRes.Succeeded || activityRes.Result == null)
                    {
                        return AppResult<ValidateCouponCodeResult>.CreateFailed(new ApplicationException("Invalid coupon code."), "Invalid coupon code.");
                    }
                }
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