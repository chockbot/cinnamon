using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Api.Core.Services.AdminService.Handlers;
using Cinnamon.Api.Core.Services.AdminService.Interactors;
using Cinnamon.Api.Core.Services.AdminService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.AdminService;

public class CreateCouponHandler : ICreateCouponHandler
{
    private readonly Services.ActivityService.Handlers.ICreateCouponHandler createCouponHandler;
    private readonly IGetProfileHandler getProfileHandler;
    private readonly IAdminUserData adminUserData;
    private readonly Services.ActivityService.Handlers.IGetActivityHandler getActivityHandler;

    public CreateCouponHandler(Services.ActivityService.Handlers.ICreateCouponHandler createCouponHandler,
        IGetProfileHandler getProfileHandler, IAdminUserData adminUserData, 
        Services.ActivityService.Handlers.IGetActivityHandler getActivityHandler)
    {
        this.createCouponHandler = createCouponHandler;
        this.getProfileHandler = getProfileHandler;
        this.adminUserData = adminUserData;
        this.getActivityHandler = getActivityHandler;
    }
    
    public AppResult<CreateCouponResult> Execute(CreateCouponArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<CreateCouponResult>.CreateFailed(ex, "An error ocurred in CreateCouponHandler");
        }
    }

    public async Task<AppResult<CreateCouponResult>> ExecuteAsync(CreateCouponArgs args)
    {
        try
        {
            var getProfileRes = await getProfileHandler.ExecuteAsync(new AccountService.Interactors.GetProfileArgs());
            if(!getProfileRes.Succeeded || getProfileRes.Result == null)
            {
                return AppResult<CreateCouponResult>.CreateFailed(new ApplicationException(getProfileRes.Message), getProfileRes.Message);
            }

            // check if account is admin
            var adminRes = await adminUserData.GetAdminUserByEmail(new Framework.ApiCommand.ApiData.AdminUser.Request.GetAdminUserByEmailArgs {
                Email = getProfileRes.Result.Email
            });
            if(!adminRes.Succeeded || adminRes.Result == null || !adminRes.Result.IsSuccess)
            {
                return AppResult<CreateCouponResult>.CreateFailed(new ApplicationException("Invalid request."), "Invalid request.");
            }

            // check activity, skip if activity id == 0
            if(args.ActivityId != 0)
            {
                var activityRes = await getActivityHandler.ExecuteAsync(new ActivityService.Interactors.GetActivityArgs {
                    ActivityId = args.ActivityId
                });
                if(!activityRes.Succeeded || activityRes.Result == null)
                {
                    return AppResult<CreateCouponResult>.CreateFailed(new ApplicationException("Invalid request."), "Invalid request.");
                }
            }

            var createRes = await createCouponHandler.ExecuteAsync(new ActivityService.Interactors.CreateCouponArgs {
                ActivityId = args.ActivityId,
                Amount = args.Amount,
                Code = args.Code,
                CustomerId = getProfileRes.Result.Id,
                DiscountType = args.DiscountType,
                FromDate = args.FromDate,
                IsAdmin = true,
                MaximumSpend = args.MaximumSpend,
                Name = args.Name,
                ToDate = args.ToDate
            });
            if(!createRes.Succeeded || createRes.Result == null)
            {
                return AppResult<CreateCouponResult>.CreateFailed(new ApplicationException(createRes.Message), createRes.Message);
            }
            var created = createRes.Result;

            return AppResult<CreateCouponResult>.CreateSucceeded(new CreateCouponResult {
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
            }, "Successfully create coupon code");
        }
        catch (Exception ex)
        {
            return AppResult<CreateCouponResult>.CreateFailed(ex, "An error ocurred in CreateCouponHandler");
        }
    }
}