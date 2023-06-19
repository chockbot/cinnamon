using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Interactors;
using Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.ActivityService;

public class CreateCouponHandler : ICreateCouponHandler
{
    private readonly ICouponData couponData;

    public CreateCouponHandler(ICouponData couponData)
    {
        this.couponData = couponData;
    }

    public AppResult<CreateCouponResult> Execute(CreateCouponArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<CreateCouponResult>.CreateFailed(ex, "An error occured in CreateCouponHandler");
        }
    }

    public async Task<AppResult<CreateCouponResult>> ExecuteAsync(CreateCouponArgs args)
    {
        try
        {
            // check if time is valid
            if(args.ToDate < args.FromDate)
            {
                return AppResult<CreateCouponResult>.CreateFailed(new ApplicationException("Provide valid date from an date to"), "Provide valid date from an date to");
            }

            // check discount type
            if(args.DiscountType != 0 && args.DiscountType != 1)
            {
                return AppResult<CreateCouponResult>.CreateFailed(new ApplicationException("Provide valid discount type"), "Provide valid discount type");
            }

            // for percentage amount validation
            if(args.DiscountType == 0 && (args.Amount > 100 || args.Amount < 0))
            {
                return AppResult<CreateCouponResult>.CreateFailed(new ApplicationException("Provide valid percentage value"), "Provide valid percentage value");
            }

            
            // check ef promo code already exist, if activity id = 0 check only customer id and promo code
            // else check activity id, code and customer id
            int? activityId = args.ActivityId == 0 ? null : args.ActivityId;
            var checkResult = await couponData.IsPromotionCodeExist(new Framework.ApiCommand.ApiData.Coupon.Request.IsPromotionCodeExistArgs {
                ActivityId = activityId,
                Code = args.Code,
                CustomerId = args.CustomerId
            });
            if(!checkResult.Succeeded || checkResult.Result == null || !checkResult.Result.IsSuccess)
            {
                return AppResult<CreateCouponResult>.CreateFailed(new ApplicationException(checkResult.Result?.ErrorInfo?.Message), checkResult.Message);
            }

            if(checkResult.Result.Result)
            {
                return AppResult<CreateCouponResult>.CreateFailed(new ApplicationException("Coupon code already exist."), "Coupon code already exist.");
            }

            var createResult = await couponData.CreateCoupon(new Framework.ApiCommand.ApiData.Coupon.Request.CreateCouponArgs {
                ActivityId = args.ActivityId,
                Amount = args.Amount,
                Code = args.Code,
                CustomerId = args.CustomerId,
                DiscountType = args.DiscountType,
                From = args.FromDate,
                IsAdmin = args.IsAdmin,
                MaximumSpend = args.MaximumSpend,
                Name = args.Name,
                Status = 0,
                To = args.ToDate
            });
            if(!createResult.Succeeded || createResult.Result == null || !createResult.Result.IsSuccess)
            {
                return AppResult<CreateCouponResult>.CreateFailed(new ApplicationException(createResult.Result?.ErrorInfo?.Message), createResult.Message);
            }
            var created = createResult.Result.Result;

            return AppResult<CreateCouponResult>.CreateSucceeded(new CreateCouponResult {
                ActivityId = created.ActivityId,
                Amount = created.Amount,
                Code = created.Code,
                CustomerId = created.CustomerId,
                DiscountType = created.DiscountType,
                FromDate = created.From,
                Id = created.Id,
                IsAdmin = args.IsAdmin,
                MaximumSpend = created.MaximumSpend,
                Name = created.Name,
                Status = created.Status,
                ToDate = created.To
            }, "Coupon code successfully created.");
        }
        catch (Exception ex)
        {
            return AppResult<CreateCouponResult>.CreateFailed(ex, "An error occured in CreateCouponHandler");
        }
    }
}