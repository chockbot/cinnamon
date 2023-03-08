using Cinnamon.Framework.Common;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.RequestRefund;

namespace Cinnamon.Api.Data.Services.Repository.Interfaces;

public interface IRequestRefundRepository 
{
    Task<AppResult<RequestRefundDTO>> GetByIdAsync(int id);
    Task<AppResult<IEnumerable<RequestRefundDTO>>> GetAllAsync(int? count, int? skip, 
        bool? includePurchaseOrder, bool? includeCustomer, int? customerId, int? status);
    Task<AppResult<IEnumerable<RequestRefundDTO>>> GetAllAsync();
    Task<AppResult<RequestRefundDTO>> Create(int customerId, int purhcaseOrderId, 
        string experienceTitle, int status, string reason);
    Task<AppResult<RequestRefundDTO>> Update(int id, int status);
}