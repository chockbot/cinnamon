using Cinnamon.Core.Common;
using Cinnamon.Core.Module.PurchaseService.Interactors;
using Cinnamon.Core.Module.PurchaseService.Interactors.Results;
using Cinnamon.Core.Models;

namespace Cinnamon.Core.Module.PurchaseService.Handler.SubmitOrder;

public class SubmitOrderHandler : ISubmitOrderHandler
{
    public AppResult<SubmitOrderResult> Execute(SubmitOrderArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<SubmitOrderResult>.CreateFailed(ex, "An error occured in SubmitOrderHandler");
        }
    }

    public async Task<AppResult<SubmitOrderResult>> ExecuteAsync(SubmitOrderArgs args)
    {
        try
        {
            // check activity
            var activity = await CoreDI.DataStore.Activities.GetActivityByIdAsync(args.ActivityId);
            if(activity == null)
            {
                return AppResult<SubmitOrderResult>.CreateFailed(new ApplicationException("Can't find activity"), "Can't find activity");
            }

            // check customer
            var customer = await CoreDI.DataStore.Customers.GetCustomerById(args.CustomerId);
            if(customer == null)
            {
                return AppResult<SubmitOrderResult>.CreateFailed(new ApplicationException("Can't find customer"), "Can't find customer");
            }
            
            // check schedule
            var schedule = activity.ScheduleList.FirstOrDefault(i => i.Id == args.ScheduleId);
            if(schedule == null)
            {
                return AppResult<SubmitOrderResult>.CreateFailed(
                    new ApplicationException("Can't find schedule associated to activity"), "Can't find schedule associated to activity");
            }

            decimal subTotal = schedule.Price * args.NumberOfHeads;
            decimal fee = subTotal * .15m;
            // temporart discount amount
            decimal discount = string.IsNullOrEmpty(args.CouponCode) ? 0 : 50;
            decimal overallTotal = (subTotal + fee) - discount;

            var purchaseData = new PurchaseOrderModel {
                ActivityId = args.ActivityId,
                ConvinienceFee = fee,
                Coupon = args.CouponCode,
                Total = subTotal + fee,
                CouponAmount = discount,
                OverallTotal = overallTotal,
            };

            var purchaseRes = await CoreDI.DataStore.PurchaseOrder.SaveDataAsync(purchaseData);
            if(!purchaseRes.Message.ToLower().Contains("saved"))
            {
                return AppResult<SubmitOrderResult>.CreateFailed(
                    new ApplicationException("Error occured when saving purchase order"), "Error occured when saving purchase order");
            }

            var ongoingActivityData = new OngoingActivityModel {
                ActivityId = activity.Id,
                CustomerId = customer.Id,
                PurchaseOrderId = purchaseData.Id,
            };

            var ongoingRes = await CoreDI.DataStore.OngoingActivity.SaveDataAsync(ongoingActivityData);
            if(!ongoingRes.Message.ToLower().Contains("saved"))
            {
                return AppResult<SubmitOrderResult>.CreateFailed(
                    new ApplicationException("Error occured when saving activity purchased"), "Error occured when saving activity purchased");
            }

            return AppResult<SubmitOrderResult>.CreateSucceeded(new SubmitOrderResult {TotalAmount = overallTotal}, "Purchased successfully");
        }
        catch (Exception ex)
        {
            return AppResult<SubmitOrderResult>.CreateFailed(ex, "An error occured in SubmitOrderHandler");
        }
    }
}