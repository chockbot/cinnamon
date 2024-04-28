using Cinnamon.Api.Data.Repository.Interfaces;
using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.PayoutLog;
using Cinnamon.Framework.Common;
using Entities = Cinnamon.Api.Data.Repository.Entities;

namespace Cinnamon.Api.Data.Services.Repository.PayoutLog;

public class PayoutLogRepository : IPayoutLogRepository
{
    private readonly IDataStore dataStore;

    public PayoutLogRepository(IDataStore dataStore)
    {
        this.dataStore = dataStore;
    }

    public async Task<AppResult<PayoutLogDTO>> Create(int purchaseOrderId, int customerId, 
        decimal amount, int status, string remarks, string payload)
    {
        try
        {
            var entity = new Entities.PayoutLog {
                Amount = amount,
                CustomerId = customerId,
                PurchaseOrderId = purchaseOrderId,
                Remarks = remarks,
                Status = status,
                Payload = payload
            };

            var result = await dataStore.PayoutLog.Add(entity);
            if(!result.Succeeded || result.Result == null)
            {
                return AppResult<PayoutLogDTO>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }
            var created = result.Result;

            return AppResult<PayoutLogDTO>.CreateSucceeded(new PayoutLogDTO {
                Amount = created.Amount,
                CustomerId = created.CustomerId,
                Id = created.Id,
                PurchaseOrderId = created.PurchaseOrderId,
                Remarks = created.Remarks,
                Status = created.Status,
                Payload = created.Payload
            }, "Successully create payout log");
        }
        catch (Exception ex)
        {   
            return AppResult<PayoutLogDTO>.CreateFailed(ex, "An error occured when creating payout log");
        }
    }

    public async Task<AppResult<IEnumerable<PayoutLogDTO>>> GetAllAsync(int? count, int? skip)
    {
        try
        {
             var result = await dataStore.PayoutLog.FindAsync(p => true, count, skip);
             if(!result.Succeeded || result.Result == null)
             {
                return AppResult<IEnumerable<PayoutLogDTO>>.CreateFailed(new ApplicationException(result.Message), result.Message);
             }

             return AppResult<IEnumerable<PayoutLogDTO>>.CreateSucceeded(result.Result.Select(p => {
                return new PayoutLogDTO {
                    Amount = p.Amount,
                    CustomerId = p.CustomerId,
                    Id = p.Id,
                    PurchaseOrderId = p.PurchaseOrderId,
                    Remarks = p.Remarks,
                    Status = p.Status,
                    Payload = p.Payload
                };
             }), "Successfully get payout logs");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<PayoutLogDTO>>.CreateFailed(ex, "An error occured when trying to get account logs");
        }
    }

    public async Task<AppResult<IEnumerable<PayoutLogDTO>>> GetAllAsync()
    {
        try
        {
            var result = await dataStore.PayoutLog.GetAllAsync();
             if(!result.Succeeded || result.Result == null)
             {
                return AppResult<IEnumerable<PayoutLogDTO>>.CreateFailed(new ApplicationException(result.Message), result.Message);
             }

             return AppResult<IEnumerable<PayoutLogDTO>>.CreateSucceeded(result.Result.Select(p => {
                return new PayoutLogDTO {
                    Amount = p.Amount,
                    CustomerId = p.CustomerId,
                    Id = p.Id,
                    PurchaseOrderId = p.PurchaseOrderId,
                    Remarks = p.Remarks,
                    Status = p.Status,
                    Payload = p.Payload
                };
             }), "Successfully get payout logs");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<PayoutLogDTO>>.CreateFailed(ex, "An error occured when trying to get account logs");
        }
    }

    public async Task<AppResult<PayoutLogDTO>> GetByIdAsync(int id)
    {
        try
        {
            var result = await dataStore.PayoutLog.GetByIdAsync(id);
            if(!result.Succeeded || result.Result == null)
            {
                return AppResult<PayoutLogDTO>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }
            var payoutLog = result.Result;

            return AppResult<PayoutLogDTO>.CreateSucceeded(new PayoutLogDTO {
                Amount = payoutLog.Amount,
                CustomerId = payoutLog.CustomerId,
                Id = payoutLog.Id,
                PurchaseOrderId = payoutLog.PurchaseOrderId,
                Remarks = payoutLog.Remarks,
                Status = payoutLog.Status,
                Payload = payoutLog.Payload
            }, "Successfully get payout log by id");
        }
        catch (Exception ex)
        {
            return AppResult<PayoutLogDTO>.CreateFailed(ex, "An error occured when getting payout log by id");
        }
    }

    public async Task<AppResult<PayoutLogDTO>> Update(int id, int? status, string? remarks)
    {
        try
        {
            var getPayoutLog = await dataStore.PayoutLog.GetByIdAsync(id);
            if(!getPayoutLog.Succeeded || getPayoutLog.Result == null)
            {
                return AppResult<PayoutLogDTO>.CreateFailed(new ApplicationException(getPayoutLog.Message), getPayoutLog.Message);
            }
            var payoutLog = getPayoutLog.Result;

            payoutLog.Status = status ?? payoutLog.Status;
            payoutLog.Remarks = remarks ?? payoutLog.Remarks;

            var updatePayoutLog = await dataStore.PayoutLog.Update(payoutLog);
            if(!updatePayoutLog.Succeeded || updatePayoutLog.Result == null)
            {
                return AppResult<PayoutLogDTO>.CreateFailed(new ApplicationException(updatePayoutLog.Message), updatePayoutLog.Message);
            }
            var updated = updatePayoutLog.Result;

            return AppResult<PayoutLogDTO>.CreateSucceeded(new PayoutLogDTO {
                Amount = updated.Amount,
                CustomerId = updated.CustomerId,
                Id = updated.Id,
                PurchaseOrderId = updated.PurchaseOrderId,
                Remarks = updated.Remarks,
                Status = updated.Status,
                Payload = updated.Payload
            }, "Successfully update payout log");
        }
        catch (Exception ex)
        {
            return AppResult<PayoutLogDTO>.CreateFailed(ex, "An error occured when updating payout log");
        }
    }

    public async Task<AppResult<IEnumerable<PayoutLogDTO>>> GetPayoutByProvider(int? id, DateTime? dateFrom, int status)
    {
        try
        {
            var result = await dataStore.PayoutLog.GetPayoutByProvider(id, dateFrom, status);
            if (!result.Succeeded || result.Result == null)
            {
                return AppResult<IEnumerable<PayoutLogDTO>>.CreateFailed(result.Error.Exception, result.Message);
            }

            var payouts = result.Result.Select(p => {
                return new PayoutLogDTO
                {
                    Id              = p.Id,
                    CustomerId      = p.CustomerId,
                    PurchaseOrderId = p.PurchaseOrderId,
                    Amount          = p.Amount,
                    Status          = p.Status,
                    PayoutDate      = p.CreatedOn
                };
            });

            return AppResult<IEnumerable<PayoutLogDTO>>.CreateSucceeded(payouts, "Successfully getting payouts by provider");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<PayoutLogDTO>>.CreateFailed(ex, "An error occured when getting payouts by provider");
        }
    }

}