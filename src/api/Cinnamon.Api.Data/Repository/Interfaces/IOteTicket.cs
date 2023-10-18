using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Data.Repository.Interfaces;

public interface IOteTicket : IGenericEntity<OteTicket> 
{
    Task<AppResult<IEnumerable<Entities.OteTicket>>> GetByActivityId(int activityId, bool includeCustomer = false, bool includeImageData = false);
    Task<AppResult<IEnumerable<Entities.OteTicket>>> GetByPurchaseOrderId(int purchaseOrderId, bool includeCustomer = false, bool includeImageData = false);
}