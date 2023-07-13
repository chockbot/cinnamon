using System.Security.Claims;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Interactors;
using Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.ActivityService;

public class ProviderCreateCouponHandler : IProviderCreateCouponHandler
{
    private readonly IHttpContextAccessor httpContext;
    private readonly IGetActivityHandler getActivityHandler;
    private readonly ICreateCouponHandler createCouponHandler;

    public ProviderCreateCouponHandler(IHttpContextAccessor httpContext, IGetActivityHandler getActivityHandler, 
        ICreateCouponHandler createCouponHandler)
    {
        this.httpContext = httpContext;
        this.getActivityHandler = getActivityHandler;
        this.createCouponHandler = createCouponHandler;
    }

    public AppResult<ProviderCreateCouponResult> Execute(ProviderCreateCouponArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<ProviderCreateCouponResult>.CreateFailed(ex, "An error occured in ProviderCreateCouponHandler");
        }
    }

    public async Task<AppResult<ProviderCreateCouponResult>> ExecuteAsync(ProviderCreateCouponArgs args)
    {
        try
        {
            // get customer id saved in claims
            var customerId = httpContext.HttpContext?.User.FindFirstValue("UserId");
            int id = customerId != null ? Convert.ToInt32(customerId) : 0;

            // validate activity if associated to customer, skip if all applied to associated activities
            if(args.ActivityId != 0)
            {
                var activityRes = await getActivityHandler.ExecuteAsync(new GetActivityArgs {
                    ActivityId = args.ActivityId,
                    CustomerId = id
                });
                if(!activityRes.Succeeded || activityRes.Result == null)
                {
                    return AppResult<ProviderCreateCouponResult>.CreateFailed(new ApplicationException("Invalid request"), "Invalid request");
                }
            }

            var createRes = await createCouponHandler.ExecuteAsync(new CreateCouponArgs {
                ActivityId = args.ActivityId,
                Amount = args.Amount,
                Code = args.Code,
                CustomerId = id,
                DiscountType = args.DiscountType,
                FromDate = args.FromDate,
                IsAdmin = false,
                MaximumSpend = args.MaximumSpend,
                Name = args.Name,
                ToDate = args.ToDate
            });
            if(!createRes.Succeeded || createRes.Result == null)
            {
                return AppResult<ProviderCreateCouponResult>.CreateFailed(new ApplicationException(createRes.Message), createRes.Message);
            }
            var created = createRes.Result;

            return AppResult<ProviderCreateCouponResult>.CreateSucceeded(new ProviderCreateCouponResult {
                ActivityId = created.ActivityId,
                Amount = created.Amount,
                Code = created.Code,
                CustomerId = created.CustomerId,
                DiscountType = created.DiscountType,
                FromDate = created.FromDate,
                Id = created.Id,
                IsAdmin = created.IsAdmin,
                MaximumSpend = created.MaximumSpend,
                Name = created.Name,
                Status = created.Status,
                ToDate = created.ToDate,
                DateCreated = created.DateCreated
            }, "Successfully create coupon code");
        }
        catch (Exception ex)
        {
            return AppResult<ProviderCreateCouponResult>.CreateFailed(ex, "An error occured in ProviderCreateCouponHandler");
        }
    }
}