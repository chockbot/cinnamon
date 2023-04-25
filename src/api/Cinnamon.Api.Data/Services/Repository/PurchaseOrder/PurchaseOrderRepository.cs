using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Entities = Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Framework.Common;
using Cinnamon.Api.Data.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.PurchaseOrder;
using System.Linq.Expressions;

namespace Cinnamon.Api.Data.Services.Repository.PurchaseOrder;

public class PurchaseOrderRepository : IPurchaseOrderRepository
{
    private readonly IDataStore dataStore;

    public PurchaseOrderRepository(IDataStore dataStore)
    {
        this.dataStore = dataStore;
    }

    public async Task<AppResult<PurchaseOrderDTO>> Create(int activityId, int scheduleId, int customerId, 
        decimal total, decimal convinienceFee, string? coupon, decimal? couponAmount, decimal overallTotal, 
        int status, string payload, decimal creditAmount, decimal unitPrice, int unitCount)
    {
        try
        {
            // check activity id if existed
            var activityRes = await dataStore.Activity.GetByIdAsync(activityId);
            if(!activityRes.Succeeded || activityRes.Result == null)
            {
                return AppResult<PurchaseOrderDTO>.CreateFailed(new ApplicationException("Can't find activity id provided"),"Can't find activity id provided");
            }

            // check schedule if existed
            var scheduleRes = await dataStore.ActivitySchedule.GetByIdAsync(scheduleId);
            if(!scheduleRes.Succeeded || scheduleRes.Result == null)
            {
                return AppResult<PurchaseOrderDTO>.CreateFailed(new ApplicationException("Can't find schedule id provided"),"Can't find schedule id provided");
            }

            // check if schedule is associated to activity
            if(activityRes.Result.Id != scheduleRes.Result.ActivityId)
            {
                return AppResult<PurchaseOrderDTO>.CreateFailed(
                    new ApplicationException("Schedule id provided is not associated to activity"),"Schedule id provided is not associated to activity");
            }

            // check customer if existed
            var customerRes = await dataStore.Customer.GetByIdAsync(customerId);
            if(!customerRes.Succeeded || customerRes.Result == null)
            {
                return AppResult<PurchaseOrderDTO>.CreateFailed(new ApplicationException("Can't find customer id provided"),"Can't find customer id provided");
            }

            var purchaseOrder = new Entities.PurchaseOrder {
                ActivityId = activityId,
                ConvinienceFee = convinienceFee,
                Coupon = coupon,
                CouponAmount = couponAmount,
                CustomerId = customerId,
                OverallTotal = overallTotal,
                ScheduleId = scheduleId,
                Total = total,
                Status = status,
                Payload = payload,
                CreditAmount = creditAmount,
                UnitCount = unitCount,
                UnitPrice = unitPrice
            };

            var createdPurchaseOrder = await dataStore.PurchaseOrder.Add(purchaseOrder);
            if(!createdPurchaseOrder.Succeeded || createdPurchaseOrder.Result == null) 
            {
                return AppResult<PurchaseOrderDTO>.CreateFailed(
                    new ApplicationException("An error occured when saving purchase order"), "An error occured when saving purchase order");
            }

            var activity = activityRes.Result;
            activity.IsNew = false;
            activity.PurchaseOrderCount = activity.PurchaseOrderCount + 1;

            var updateActivityResult = await dataStore.Activity.Update(activity);
            if (!updateActivityResult.Succeeded || updateActivityResult.Result == null)
            {
                return AppResult<PurchaseOrderDTO>.CreateFailed(
                    new ApplicationException("An error occured when updating activity IsNew/PurchaseOrderCount field"), "An error occured when updating activity IsNew/PurchaseOrderCount field");
            }

            return AppResult<PurchaseOrderDTO>.CreateSucceeded(new PurchaseOrderDTO {
                Id = createdPurchaseOrder.Result.Id,
                ActivityId = activityId,
                ConvinienceFee = convinienceFee,
                Coupon = coupon,
                CouponAmount = couponAmount,
                CustomerId = customerId,
                OverallTotal = overallTotal,
                ScheduleId = scheduleId,
                Total = total,
                Status = status,
                Payload = payload,
                CreditAmount = creditAmount,
                UnitCount = unitCount,
                UnitPrice = unitPrice
            }, "Successfully created purchase order");
        }
        catch (Exception ex)
        {
            return AppResult<PurchaseOrderDTO>.CreateFailed(ex, "An error occured when creating purchase order");
        }
    }

    public async Task<AppResult<IEnumerable<PurchaseOrderDTO>>> GetAllAsync(int? count, int? skip, 
        bool? includeActivity, bool? includeSchedule, int? customerId, int? status)
    {
        try
        {
            var includes = new List<Expression<Func<Entities.PurchaseOrder, object>>>();
            if(includeActivity.HasValue && includeActivity.Value) includes.Add(p => p.Activity);
            if(includeSchedule.HasValue && includeSchedule.Value) includes.Add(p => p.Schedule);

            Expression<Func<Entities.PurchaseOrder, bool>> filter = 
                p => (customerId.HasValue ? p.CustomerId == customerId.Value : true) &&
                    (status.HasValue ? p.Status == status.Value : true);
            
            var result = await dataStore.PurchaseOrder.FindAsync(filter, count, skip, includes);
            if(!result.Succeeded || result.Result == null)
            {
                return AppResult<IEnumerable<PurchaseOrderDTO>>.CreateFailed(result.Error.Exception, result.Message);
            }

            var purchaseOrders = result.Result.Select(p => {
                var dto = new PurchaseOrderDTO {
                    ActivityId = p.ActivityId,
                    ConvinienceFee = p.ConvinienceFee,
                    Coupon = p.Coupon,
                    CouponAmount = p.CouponAmount,
                    CustomerId = p.CustomerId,
                    Id = p.Id,
                    OverallTotal = p.OverallTotal,
                    ScheduleId = p.ScheduleId,
                    Total = p.Total,
                    Status = p.Status,
                    Payload = p.Payload,
                    CreditAmount = p.CreditAmount,
                    UnitCount = p.UnitCount,
                    UnitPrice = p.UnitPrice
                };

                // include activity details
                if(includeActivity.HasValue && includeActivity.Value)
                {
                    dto.Activity = new PurchaseOrderDTO.AssociatedActivity {
                        Description = p.Activity.Description,
                        Id = p.Activity.Id,
                        Title = p.Activity.Title
                    };
                }

                // include schedule details
                if(includeSchedule.HasValue && includeSchedule.Value)
                {
                    dto.Schedule = new PurchaseOrderDTO.AssociatedSchedule {
                        DateTime = p.Schedule.DateTime,
                        Id = p.Schedule.Id,
                        Name = p.Schedule.Name
                    };
                }

                return dto;
            });

            return AppResult<IEnumerable<PurchaseOrderDTO>>.CreateSucceeded(purchaseOrders, "Successfully getting purchase orders");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<PurchaseOrderDTO>>.CreateFailed(ex, "An error occured when getting purchase order");
        }
    }

    public async Task<AppResult<IEnumerable<PurchaseOrderDTO>>> GetAllAsync()
    {
        try
        {
            var result = await dataStore.PurchaseOrder.GetAllAsync();
            if(!result.Succeeded || result.Result == null)
            {
                return AppResult<IEnumerable<PurchaseOrderDTO>>.CreateFailed(result.Error.Exception, result.Message);
            }

            var purchaseOrders = result.Result.Select(p => {
                return new PurchaseOrderDTO {
                    ActivityId = p.ActivityId,
                    ConvinienceFee = p.ConvinienceFee,
                    Coupon = p.Coupon,
                    CouponAmount = p.CouponAmount,
                    CustomerId = p.CustomerId,
                    Id = p.Id,
                    OverallTotal = p.OverallTotal,
                    ScheduleId = p.ScheduleId,
                    Total = p.Total,
                    Status = p.Status,
                    Payload = p.Payload,
                    CreditAmount = p.CreditAmount,
                    UnitCount = p.UnitCount,
                    UnitPrice = p.UnitPrice
                };
            });

            return AppResult<IEnumerable<PurchaseOrderDTO>>.CreateSucceeded(purchaseOrders, "Successfully getting purchase orders");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<PurchaseOrderDTO>>.CreateFailed(ex, "An error occured when getting purchase order");
        }
    }

    public async Task<AppResult<PurchaseOrderDTO>> GetByIdAsync(int id)
    {
        try
        {
            var result = await dataStore.PurchaseOrder.GetByIdAsync(id);
            if(!result.Succeeded || result.Result == null)
            {
                return AppResult<PurchaseOrderDTO>.CreateFailed(result.Error.Exception, result.Message);
            }

            return AppResult<PurchaseOrderDTO>.CreateSucceeded(new PurchaseOrderDTO {
                ActivityId = result.Result.ActivityId,
                ConvinienceFee = result.Result.ConvinienceFee,
                Coupon = result.Result.Coupon,
                CouponAmount = result.Result.CouponAmount,
                CustomerId = result.Result.CustomerId,
                Id = result.Result.Id,
                OverallTotal = result.Result.OverallTotal,
                ScheduleId = result.Result.ScheduleId,
                Total = result.Result.Total,
                Status = result.Result.Status,
                Payload = result.Result.Payload,
                CreditAmount = result.Result.CreditAmount,
                UnitCount = result.Result.UnitCount,
                UnitPrice = result.Result.UnitPrice
            }, "Successfully get purchase order by id");
        }
        catch (Exception ex)
        {
            return AppResult<PurchaseOrderDTO>.CreateFailed(ex, "An error occured when getting purchase order by id");
        }
    }

    public async Task<AppResult<PurchaseOrderDTO>> Update(int purchaseOrderId, int? scheduleId, decimal? total, 
        decimal? convinienceFee, string? coupon, decimal? couponAmount, decimal? overallTotal, int? status, decimal? creditAmount,
        decimal? unitPrice, int? unitCount)
    {
        try
        {
            // check purchase order if existed
            var purchaseOrderRes = await dataStore.PurchaseOrder.GetByIdAsync(purchaseOrderId);
            if(!purchaseOrderRes.Succeeded || purchaseOrderRes.Result == null)
            {
                return AppResult<PurchaseOrderDTO>.CreateFailed(
                    new ApplicationException("Can't find purchase order provided"), "Can't find purchase order provided");
            }
            var purchaseOrder = purchaseOrderRes.Result;

            // check schedule if existed
            if(scheduleId.HasValue)
            {
                var scheduleRes = await dataStore.ActivitySchedule.GetByIdAsync(scheduleId.Value);
                if(!scheduleRes.Succeeded || scheduleRes.Result == null)
                {
                    return AppResult<PurchaseOrderDTO>.CreateFailed(
                        new ApplicationException("Can't find schedule id provided"), "Can't find schedule id provided");
                }

                // check if schedule associated to activity
                if(purchaseOrder.ActivityId != scheduleRes.Result.ActivityId)
                {
                    return AppResult<PurchaseOrderDTO>.CreateFailed(
                    new ApplicationException("Schedule id provided is not associated to activity"),"Schedule id provided is not associated to activity");
                }
            }

            purchaseOrder.ScheduleId = scheduleId ?? purchaseOrder.ScheduleId;
            purchaseOrder.Total = total ?? purchaseOrder.Total;
            purchaseOrder.ConvinienceFee = convinienceFee ?? purchaseOrder.ConvinienceFee;
            purchaseOrder.Coupon = coupon ?? purchaseOrder.Coupon;
            purchaseOrder.CouponAmount = couponAmount ?? purchaseOrder.CouponAmount;
            purchaseOrder.OverallTotal = overallTotal ?? purchaseOrder.OverallTotal;
            purchaseOrder.Status = status ?? purchaseOrder.Status;
            purchaseOrder.CreditAmount = creditAmount ?? purchaseOrder.CreditAmount;
            purchaseOrder.UnitCount = unitCount ?? purchaseOrder.UnitCount;
            purchaseOrder.UnitPrice = unitPrice ?? purchaseOrder.UnitPrice;

            var updatedPurchaseOrder = await dataStore.PurchaseOrder.Update(purchaseOrder);
            if(!updatedPurchaseOrder.Succeeded || updatedPurchaseOrder.Result == null)
            {
                return AppResult<PurchaseOrderDTO>.CreateFailed(
                    new ApplicationException("An error occured when updating purchase order"), "An error occured when updating purchase order");
            }
            var updated = updatedPurchaseOrder.Result;

            return AppResult<PurchaseOrderDTO>.CreateSucceeded(new PurchaseOrderDTO {
                Id = updated.Id,
                ActivityId = updated.ActivityId,
                ConvinienceFee = updated.ConvinienceFee,
                Coupon = updated.Coupon,
                CouponAmount = updated.CouponAmount,
                CustomerId = updated.CustomerId,
                OverallTotal = updated.OverallTotal,
                ScheduleId = updated.ScheduleId,
                Total = updated.Total,
                Status = updated.Status,
                Payload = updated.Payload,
                CreditAmount = updated.CreditAmount,
                UnitCount = updated.UnitCount,
                UnitPrice = updated.UnitPrice
            }, "Successfully updated purchase order");
        }
        catch (Exception ex)
        {
            return AppResult<PurchaseOrderDTO>.CreateFailed(ex, "An error occured when updating purchase order");
        }
    }

    public async Task<AppResult<IEnumerable<PurchaseOrderDTO>>> GetAllPurchaseOrderNeedToPayout()
    {
        try
        {
            var result = await dataStore.PurchaseOrder.GetAllPurchaseOrdersNeedPayout();
            if(!result.Succeeded || result.Result == null)
            {
                return AppResult<IEnumerable<PurchaseOrderDTO>>.CreateFailed(result.Error.Exception, result.Message);
            }

            var purchaseOrders = result.Result.Select(p => {
                return new PurchaseOrderDTO {
                    ActivityId = p.ActivityId,
                    ConvinienceFee = p.ConvinienceFee,
                    Coupon = p.Coupon,
                    CouponAmount = p.CouponAmount,
                    CustomerId = p.CustomerId,
                    Id = p.Id,
                    OverallTotal = p.OverallTotal,
                    ScheduleId = p.ScheduleId,
                    Total = p.Total,
                    Status = p.Status,
                    Payload = p.Payload,
                    CreditAmount = p.CreditAmount
                };
            });

            return AppResult<IEnumerable<PurchaseOrderDTO>>.CreateSucceeded(purchaseOrders, "Successfully getting purchase orders");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<PurchaseOrderDTO>>.CreateFailed(ex, "An error occured when getting purchase order");
        }
    }

    public async Task<AppResult<IEnumerable<PurchaseOrderDTO>>> UpdatePurchaseOrdersStatus(IEnumerable<int> ids, int status)
    {
        try
        {
            var entities = ids.Select(i => {
                return new Entities.PurchaseOrder {
                    Id = i,
                    Status = status
                };
            });

            var updateStatusResult = await dataStore.PurchaseOrder.UpdatePurchaseOrdersByStatus(entities);
            if(!updateStatusResult.Succeeded || updateStatusResult.Result == null)
            {
                return AppResult<IEnumerable<PurchaseOrderDTO>>.CreateFailed(new ApplicationException(updateStatusResult.Message), updateStatusResult.Message);
            }

            return AppResult<IEnumerable<PurchaseOrderDTO>>.CreateSucceeded(updateStatusResult.Result.Select(p => {
                return new PurchaseOrderDTO {
                    Id = p.Id,
                    Status = p.Status
                };
            }), "Successfully update purchase orders");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<PurchaseOrderDTO>>.CreateFailed(ex, "An error occured when updating purchase orders");
        }
    }
}