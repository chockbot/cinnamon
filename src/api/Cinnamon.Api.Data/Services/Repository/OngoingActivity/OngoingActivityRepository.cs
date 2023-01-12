using Cinnamon.Api.Data.Repository.Interfaces;
using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Entities = Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Framework.Common;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.OngoingActivity;

namespace Cinnamon.Api.Data.Services.Repository.OngoingActivity;

public class OngoingActivityRepository : IOngoingActivityRepository
{
    private readonly IDataStore dataStore;

    public OngoingActivityRepository(IDataStore dataStore)
    {
        this.dataStore = dataStore;
    }

    public async Task<AppResult<OngoingActivityDTO>> Create(int activityId, int customerId, int purchaseOrderId)
    {
        try
        {
            // check activity if existed
            var activityRes = await dataStore.Activity.GetByIdAsync(activityId);
            if(!activityRes.Succeeded || activityRes.Result == null)
            {
                return AppResult<OngoingActivityDTO>.CreateFailed(
                    new ApplicationException("Can't find activity id provided"), "Can't find activity id provided");
            }

            // check customer if existed
            var customerRes = await dataStore.Customer.GetByIdAsync(customerId);
            if(!customerRes.Succeeded || customerRes.Result == null)
            {
                return AppResult<OngoingActivityDTO>.CreateFailed(
                    new ApplicationException("Can't find customer id provided"), "Can't find customer id provided");
            }

            // check purchase order if existed
            var purchaseOrderRes = await dataStore.PurchaseOrder.GetByIdAsync(purchaseOrderId);
            if(!purchaseOrderRes.Succeeded || purchaseOrderRes.Result == null)
            {
                return AppResult<OngoingActivityDTO>.CreateFailed(
                    new ApplicationException("Can't find purchase order id provided"), "Can't find purchase order id provided");
            }

            var ongoingActivity = new Entities.OngoingActivity {
                ActivityId = activityId,
                CustomerId = customerId,
                PurchaseOrderId = purchaseOrderId
            };

            var createdRes = await dataStore.OngoingActivity.Add(ongoingActivity);
            if(!createdRes.Succeeded || createdRes.Result == null)
            {
                return AppResult<OngoingActivityDTO>.CreateFailed(
                    new ApplicationException("An error occured when creating ongoing activity"), "An error occured when creating ongoing activity");
            }

            return AppResult<OngoingActivityDTO>.CreateSucceeded(new OngoingActivityDTO {
                ActivityId = activityId,
                CustomerId = customerId,
                PurchaseOrderId = purchaseOrderId,
                Id = createdRes.Result.Id
            },"Successfully created ongoing activity");
        }
        catch (Exception ex)
        {
            return AppResult<OngoingActivityDTO>.CreateFailed(ex, "An error occured when creating ongoing activity");
        }
    }

    public async Task<AppResult<IEnumerable<OngoingActivityDTO>>> GetAllAsync(int? count, int? skip)
    {
        try
        {
            var result = await dataStore.OngoingActivity.FindAsync(p => true, count, skip);
            if(!result.Succeeded || result.Result == null)
            {
                return AppResult<IEnumerable<OngoingActivityDTO>>.CreateFailed(result.Error.Exception, result.Message);
            }

            var ongoingActivities = result.Result.Select(o => {
                return new OngoingActivityDTO {
                    ActivityId = o.ActivityId,
                    CustomerId = o.CustomerId,
                    PurchaseOrderId = o.PurchaseOrderId,
                    Id = o.Id
                };
            });

            return AppResult<IEnumerable<OngoingActivityDTO>>.CreateSucceeded(ongoingActivities, "Successfully getting ongoing activities");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<OngoingActivityDTO>>.CreateFailed(ex, "An error occured when getting ongoing activities");
        }
    }

    public async Task<AppResult<IEnumerable<OngoingActivityDTO>>> GetAllAsync()
    {
        try
        {
            var result = await dataStore.OngoingActivity.GetAllAsync();
            if(!result.Succeeded || result.Result == null)
            {
                return AppResult<IEnumerable<OngoingActivityDTO>>.CreateFailed(result.Error.Exception, result.Message);
            }

            var ongoingActivities = result.Result.Select(o => {
                return new OngoingActivityDTO {
                    ActivityId = o.ActivityId,
                    CustomerId = o.CustomerId,
                    PurchaseOrderId = o.PurchaseOrderId,
                    Id = o.Id
                };
            });

            return AppResult<IEnumerable<OngoingActivityDTO>>.CreateSucceeded(ongoingActivities, "Successfully getting ongoing activities");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<OngoingActivityDTO>>.CreateFailed(ex, "An error occured when getting all ongoing activities");
        }
    }

    public async Task<AppResult<OngoingActivityDTO>> GetByIdAsync(int id)
    {
        try
        {
            var result = await dataStore.OngoingActivity.GetByIdAsync(id);
            if(!result.Succeeded || result.Result == null)
            {
                return AppResult<OngoingActivityDTO>.CreateFailed(
                    new ApplicationException("Can't find ongoing activity by id"), "Can't find ongoing activity by id");
            }

            return AppResult<OngoingActivityDTO>.CreateSucceeded(new OngoingActivityDTO {
                ActivityId = result.Result.ActivityId,
                CustomerId = result.Result.CustomerId,
                Id = result.Result.Id,
                PurchaseOrderId = result.Result.PurchaseOrderId
            }, "Successfully getting ongoing activity by id");
        }
        catch (Exception ex)
        {
            return AppResult<OngoingActivityDTO>.CreateFailed(ex, "An error occured when getting ongoing activity by id");
        }
    }

    public async Task<AppResult<OngoingActivityDTO>> Update(int ongoingActivityId, int? activityId, int? customerId, int? purchaseOrderId)
    {
        try
        {
            // check if activity existed
            if(activityId.HasValue)
            {
                var activityRes = await dataStore.Activity.GetByIdAsync(activityId.Value);
                if(!activityRes.Succeeded || activityRes.Result == null)
                {
                    return AppResult<OngoingActivityDTO>.CreateFailed(
                        new ApplicationException("Can't find activity id provided"), "Can't find activity id provided");
                }
            }

            // check customer
            if(customerId.HasValue)
            {
                var customerRes = await dataStore.Customer.GetByIdAsync(customerId.Value);
                if(!customerRes.Succeeded || customerRes.Result == null)
                {
                    return AppResult<OngoingActivityDTO>.CreateFailed(
                        new ApplicationException("Can't find customer id provided"), "Can't find customer id provided");
                }
            }

            // check purchase order
            if(purchaseOrderId.HasValue)
            {
                var purchaseOrderRes = await dataStore.PurchaseOrder.GetByIdAsync(purchaseOrderId.Value);
                if(!purchaseOrderRes.Succeeded || purchaseOrderRes.Result == null)
                {
                    return AppResult<OngoingActivityDTO>.CreateFailed(
                        new ApplicationException("Can't find purchase order id provided"), "Can't find purchase order id provided");
                }
            }

            // check ongoing activity if existed
            var ongoingRes = await dataStore.OngoingActivity.GetByIdAsync(ongoingActivityId);
            if(!ongoingRes.Succeeded || ongoingRes.Result == null)
            {
                return AppResult<OngoingActivityDTO>.CreateFailed(
                        new ApplicationException("Can't find ongoing activity id"), "Can't find ongoing activity id");
            }
            var ongoingActivity = ongoingRes.Result;

            ongoingActivity.ActivityId = activityId ?? ongoingActivity.ActivityId;
            ongoingActivity.CustomerId = customerId ?? ongoingActivity.CustomerId;
            ongoingActivity.PurchaseOrderId = purchaseOrderId ?? ongoingActivity.PurchaseOrderId;

            var updatedRes = await dataStore.OngoingActivity.Update(ongoingActivity);
            if(!updatedRes.Succeeded || updatedRes.Result == null)
            {
                return AppResult<OngoingActivityDTO>.CreateFailed(
                    new ApplicationException("An error occured when updating ongoing activity"), "An error occured when updating ongoing activity");
            }

            return AppResult<OngoingActivityDTO>.CreateSucceeded(new OngoingActivityDTO {
                ActivityId = updatedRes.Result.ActivityId,
                CustomerId = updatedRes.Result.CustomerId,
                PurchaseOrderId = updatedRes.Result.PurchaseOrderId,
                Id = updatedRes.Result.Id
            }, "Successfully updated ongoing activity");
        }
        catch (Exception ex)
        {
            return AppResult<OngoingActivityDTO>.CreateFailed(ex, "An error occured when updating ongoing activity");
        }
    }
}