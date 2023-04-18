using System.Security.Claims;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Interactors;
using Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.ActivityService;

public class GetRefundableExperienceHandler : IGetRefundableExperienceHandler
{
    private readonly IHttpContextAccessor httpContext;
    private readonly IPurchaseOrderData purchaseOrderData;

    public GetRefundableExperienceHandler(IHttpContextAccessor httpContext, IPurchaseOrderData purchaseOrderData)
    {
        this.httpContext = httpContext;
        this.purchaseOrderData = purchaseOrderData;
    }

    public AppResult<GetRefundableExperienceResult> Execute(GetRefundableExperienceArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<GetRefundableExperienceResult>.CreateFailed(ex, "An error occured in GetRefundableExperienceHandler");
        }
    }

    public async Task<AppResult<GetRefundableExperienceResult>> ExecuteAsync(GetRefundableExperienceArgs args)
    {
        try
        {
            // get customer id saved in claims
            var customerId = httpContext.HttpContext?.User.FindFirstValue("UserId");
            if(customerId == null)
            {
                return AppResult<GetRefundableExperienceResult>.CreateFailed(
                    new ApplicationException("Unable to determine current account login"), "Unable to determine current account login");
            }
            int id = Convert.ToInt32(customerId);

            // get purchase orders with successfull status
            var purchaseOrdersRes = await purchaseOrderData.GetAllPurchaseOrder(new Framework.ApiCommand.ApiData.PurchaseOrder.Request.GetAllPurchaseOrderArgs {
                CustomerId = id,
                IncludeActivity = true,
                IncludeSchedule = true,
                Status = 1
            });

            if(!purchaseOrdersRes.Succeeded || purchaseOrdersRes.Result == null || !purchaseOrdersRes.Result.IsSuccess)
            {
                return AppResult<GetRefundableExperienceResult>.CreateFailed(
                    new ApplicationException(purchaseOrdersRes.Result?.ErrorInfo?.Message), purchaseOrdersRes.Message);
            }
            var result = purchaseOrdersRes.Result.Result;

            return AppResult<GetRefundableExperienceResult>.CreateSucceeded(new GetRefundableExperienceResult {
                RefundableExperiences = result.Select(p => {
                    return new GetRefundableExperienceResult.RefundableExperience {
                        Name = $"{p.Activity.Title} - {p.Schedule.Name} ({p.Schedule.DateTime})",
                        PurchaseOrderId = p.Id
                    };
                })
            }, "Successfully get refundable experience");
        }
        catch (Exception ex)
        {
            return AppResult<GetRefundableExperienceResult>.CreateFailed(ex, "An error occured in GetRefundableExperienceHandler");
        }
    }
}