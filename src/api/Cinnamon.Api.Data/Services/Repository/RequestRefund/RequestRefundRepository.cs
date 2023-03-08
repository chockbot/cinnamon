using System.Linq.Expressions;
using Cinnamon.Api.Data.Repository.Interfaces;
using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.RequestRefund;
using Cinnamon.Framework.Common;
using Entities = Cinnamon.Api.Data.Repository.Entities;

namespace Cinnamon.Api.Data.Services.Repository.RequestRefund;

public class RequestRefundRepository : IRequestRefundRepository
{
    private readonly IDataStore dataStore;

    public RequestRefundRepository(IDataStore dataStore)
    {
        this.dataStore = dataStore;   
    }
    
    public async Task<AppResult<RequestRefundDTO>> Create(int customerId, int purhcaseOrderId, string experienceTitle, int status, string reason)
    {
        try
        {
            // check customer id
            var customerCheck = await dataStore.Customer.GetByIdAsync(customerId);
            if(!customerCheck.Succeeded || customerCheck.Result == null)
            {
                return AppResult<RequestRefundDTO>.CreateFailed(new ApplicationException("Invalid Customer Id"), "Invalid Customer Id");
            }

            // check purchaseOrder id
            var purchaseOrderCheck = await dataStore.PurchaseOrder.GetByIdAsync(purhcaseOrderId);
            if(!purchaseOrderCheck.Succeeded || purchaseOrderCheck.Result == null)
            {
                return AppResult<RequestRefundDTO>.CreateFailed(new ApplicationException("Invalid PurchaseOrder Id"), "Invalid PurchaseOrder Id");
            }

            var entity = new Entities.RequestRefund 
            {
                CustomerId = customerId,
                ExperienceTitle = experienceTitle,
                PurchaseOrderId = purhcaseOrderId,
                Status = status,
                Reason = reason
            };

            var result = await dataStore.RequestRefund.Add(entity);
            if(!result.Succeeded || result.Result == null)
            {
                return AppResult<RequestRefundDTO>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }
            var created = result.Result;

            return AppResult<RequestRefundDTO>.CreateSucceeded(new RequestRefundDTO {
                CustomerId = created.CustomerId,
                ExperienceTitle = created.ExperienceTitle,
                PurchaseOrderId = created.PurchaseOrderId,
                Status = created.Status,
                Reason = created.Reason
            }, "Successfully create request refund");
        }
        catch (Exception ex)
        {
            return AppResult<RequestRefundDTO>.CreateFailed(ex, "An error occured when creating request refund");
        }
    }

    public async Task<AppResult<IEnumerable<RequestRefundDTO>>> GetAllAsync(int? count, int? skip, bool? includePurchaseOrder, 
        bool? includeCustomer, int? customerId, int? status)
    {
        try
        {
            var includes = new List<Expression<Func<Entities.RequestRefund, object>>>();
            if(includeCustomer.HasValue && includeCustomer.Value) includes.Add(r => r.Customer);
            if(includePurchaseOrder.HasValue && includePurchaseOrder.Value) includes.Add(r => r.PurchaseOrder);

            Expression<Func<Entities.RequestRefund, bool>> filter = 
                r => (customerId.HasValue ? r.CustomerId == customerId.Value : true) &&
                    (status.HasValue ? r.Status == status.Value : true);

            var result = await dataStore.RequestRefund.FindAsync(filter, count, skip, includes);
            if(!result.Succeeded || result.Result == null)
            {
                return AppResult<IEnumerable<RequestRefundDTO>>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }

            var requestRefunds = result.Result.Select(r => {
                var dto = new RequestRefundDTO 
                {
                    CustomerId = r.CustomerId,
                    ExperienceTitle = r.ExperienceTitle,
                    PurchaseOrderId = r.PurchaseOrderId,
                    Status = r.Status,
                    Reason = r.Reason
                };

                // add customer details
                if(includeCustomer.HasValue && includeCustomer.Value)
                {
                    dto.Customer = new RequestRefundDTO.AssociatedCustomer {
                        FirstName = r.Customer.FirstName,
                        Id = r.Customer.Id,
                        LastName = r.Customer.LastName
                    };
                }

                // add purchase order details
                if(includePurchaseOrder.HasValue && includePurchaseOrder.Value)
                {
                    dto.PurchaseOrder = new RequestRefundDTO.AssociatedPurchase {
                        ActivityId = r.PurchaseOrder.ActivityId,
                        Id = r.PurchaseOrder.Id,
                        OverAllTotal = r.PurchaseOrder.OverallTotal,
                        ScheduleId = r.PurchaseOrder.ScheduleId
                    };
                }

                return dto;
            });

            return AppResult<IEnumerable<RequestRefundDTO>>.CreateSucceeded(requestRefunds, "Successfully get request refunds");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<RequestRefundDTO>>.CreateFailed(ex, "An error occured when getting request refund data");
        }
    }

    public async Task<AppResult<IEnumerable<RequestRefundDTO>>> GetAllAsync()
    {
        try
        {
            var result = await dataStore.RequestRefund.GetAllAsync();
            if(!result.Succeeded || result.Result == null)
            {
                return AppResult<IEnumerable<RequestRefundDTO>>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }

            var requestRefunds = result.Result.Select(r => {
                var dto = new RequestRefundDTO 
                {
                    CustomerId = r.CustomerId,
                    ExperienceTitle = r.ExperienceTitle,
                    PurchaseOrderId = r.PurchaseOrderId,
                    Status = r.Status,
                    Reason = r.Reason
                };

                return dto;
            });

            return AppResult<IEnumerable<RequestRefundDTO>>.CreateSucceeded(requestRefunds, "Successfully get request refunds");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<RequestRefundDTO>>.CreateFailed(ex, "An error occured when getting request refund data");
        }
    }

    public async Task<AppResult<RequestRefundDTO>> GetByIdAsync(int id)
    {
        try
        {
            var result = await dataStore.RequestRefund.GetByIdAsync(id);
            if(!result.Succeeded || result.Result == null)
            {
                return AppResult<RequestRefundDTO>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }

            return AppResult<RequestRefundDTO>.CreateSucceeded(new RequestRefundDTO {
                CustomerId = result.Result.CustomerId,
                ExperienceTitle = result.Result.ExperienceTitle,
                PurchaseOrderId = result.Result.PurchaseOrderId,
                Status = result.Result.Status,
                Reason = result.Result.Reason
            }, "Successfully get request refund by id");

        }
        catch (Exception ex)
        {
            return AppResult<RequestRefundDTO>.CreateFailed(ex, "An error occured when getting request refund by id");
        }
    }

    public async Task<AppResult<RequestRefundDTO>> Update(int id, int status)
    {
        try
        {
            var requestRefundChk = await dataStore.RequestRefund.GetByIdAsync(id);
            if(!requestRefundChk.Succeeded || requestRefundChk.Result == null)
            {
                return AppResult<RequestRefundDTO>.CreateFailed(new ApplicationException("Invalud request reqund id"), "Invalud request reqund id");
            }
            requestRefundChk.Result.Status = status;

            var updatedRes = await dataStore.RequestRefund.Update(requestRefundChk.Result);
            if(!updatedRes.Succeeded || updatedRes.Result == null)
            {
                return AppResult<RequestRefundDTO>.CreateFailed(new ApplicationException(updatedRes.Message), updatedRes.Message);
            }
            var updated = updatedRes.Result;

            return AppResult<RequestRefundDTO>.CreateSucceeded(new RequestRefundDTO {
                CustomerId = updated.CustomerId,
                ExperienceTitle = updated.ExperienceTitle,
                PurchaseOrderId = updated.PurchaseOrderId,
                Status = updated.Status,
                Reason = updated.Reason
            }, "Successfully updated requst refund");
        }
        catch (Exception ex)
        {
            return AppResult<RequestRefundDTO>.CreateFailed(ex, "An error occured when updating request refund by id");
        }
    }
}