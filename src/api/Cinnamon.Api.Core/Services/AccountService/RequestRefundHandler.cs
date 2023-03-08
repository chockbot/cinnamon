using System.Security.Claims;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Interactors;
using Cinnamon.Api.Core.Services.AccountService.Interactors.Results;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.AccountService;

public class RequestRefundHandler : IRequestRefundHandler
{
    private readonly IHttpContextAccessor httpContext;
    private readonly IPurchaseOrderData purchaseOrderData;
    private readonly IGetActivityHandler getActivityHandler;
    private readonly IRequestRefundData requestRefundData;

    public RequestRefundHandler(IHttpContextAccessor httpContext, IPurchaseOrderData purchaseOrderData,
        IGetActivityHandler getActivityHandler, IRequestRefundData requestRefundData)
    {
        this.httpContext = httpContext;
        this.purchaseOrderData = purchaseOrderData;
        this.getActivityHandler = getActivityHandler;
        this.requestRefundData = requestRefundData;
    }

    public AppResult<RequestRefundResult> Execute(RequestRefundArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<RequestRefundResult>.CreateFailed(ex, "An error occured in RequestRefundHandler");
        }
    }

    public async Task<AppResult<RequestRefundResult>> ExecuteAsync(RequestRefundArgs args)
    {
        try
        {
            // get customer id saved in claims
            var customerId = httpContext.HttpContext?.User.FindFirstValue("UserId");
            if(customerId == null)
            {
                return AppResult<RequestRefundResult>.CreateFailed(
                    new ApplicationException("Unable to determine current account login"), "Unable to determine current account login");
            }
            int id = Convert.ToInt32(customerId);

            // check purchase order id if associated to customer
            var purchaseOrderRes = await purchaseOrderData.GetPurchaseOrderById(args.PurchaseOrderId);
            if(!purchaseOrderRes.Succeeded || purchaseOrderRes.Result == null || !purchaseOrderRes.Result.IsSuccess)
            {
                return AppResult<RequestRefundResult>.CreateFailed(
                    new ApplicationException("Invalid purchase order id"), "Invalid purchase order id");
            }
            var purchaseOrder = purchaseOrderRes.Result.Result;

            // can only request if purchase order status = 1
            if(purchaseOrder.Status != 1)
            {
                return AppResult<RequestRefundResult>.CreateFailed(
                    new ApplicationException("Invalid purchase order id"), "Invalid purchase order id");
            }

            if(purchaseOrder.CustomerId != id)
            {
                return AppResult<RequestRefundResult>.CreateFailed(
                    new ApplicationException("Invalid purchase order id"), "Invalid purchase order id");
            }

            // get activity by id
            var activityRes = await getActivityHandler.ExecuteAsync(new ActivityService.Interactors.GetActivityArgs {
                ActivityId = purchaseOrder.ActivityId
            });
            if(!activityRes.Succeeded || activityRes.Result == null)
            {
                return AppResult<RequestRefundResult>.CreateFailed(
                    new ApplicationException("Invalid purchase order id"), "Invalid purchase order id");
            }

            // check if already have pending request
            var requestCheck = await requestRefundData.GetAllRequestRefund(new Framework.ApiCommand.ApiData.RequestRefund.Request.GetAllRequestRefundArgs {
                CustomerId = id,
                Status = 0
            });
            if(!requestCheck.Succeeded || requestCheck.Result == null || !requestCheck.Result.IsSuccess)
            {
                return AppResult<RequestRefundResult>.CreateFailed(
                    new ApplicationException(requestCheck.Result?.ErrorInfo?.Message), requestCheck.Message);
            }
            if(requestCheck.Result.Result.Where(r => r.PurchaseOrderId == args.PurchaseOrderId).Count() > 0)
            {
                return AppResult<RequestRefundResult>.CreateFailed(
                    new ApplicationException("Already have pending request"), "Already have pending request");
            }

            var createRequest = await requestRefundData.CreateRequestRefund(new Framework.ApiCommand.ApiData.RequestRefund.Request.CreateRequestRefundArgs {
                CustomerId = id,
                ExperienceTitle = activityRes.Result.Title,
                PurchaseOrderId = args.PurchaseOrderId,
                Reason = args.Reason,
                Status = 0
            });

            if(!createRequest.Succeeded || createRequest.Result == null || !createRequest.Result.IsSuccess)
            {
                return AppResult<RequestRefundResult>.CreateFailed(
                    new ApplicationException(createRequest.Result?.ErrorInfo?.Message), createRequest.Message);
            }

            return AppResult<RequestRefundResult>.CreateSucceeded(new RequestRefundResult {}, "Successfully request refund");
        }
        catch (Exception ex)
        {
            return AppResult<RequestRefundResult>.CreateFailed(ex, "An error occured in RequestRefundHandler");
        }
    }
}