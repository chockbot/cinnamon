using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.OteSchedule;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Data.Repository.Interfaces;

public interface IOteTicket : IGenericEntity<OteTicket> 
{
    Task<AppResult<IEnumerable<OteTicket>>> GetByActivityId(int activityId, string searchValue,int searchBy, int? count, int? skip, bool includeCustomer = false, bool includeImageData = false);
    Task<AppResult<IEnumerable<OteScheduleDTO>>> GetTicketDetails(int activityId);
    Task<AppResult<IEnumerable<Entities.OteTicket>>> GetByPurchaseOrderId(int purchaseOrderId, bool includeCustomer = false, bool includeImageData = false);
}