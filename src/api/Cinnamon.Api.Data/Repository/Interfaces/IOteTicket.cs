using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.OteSchedule;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.OteTicket;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Data.Repository.Interfaces;

public interface IOteTicket : IGenericEntity<OteTicket> 
{
    Task<AppResult<IEnumerable<OteTicket>>> GetByActivityId(int activityId, int dateId, string searchValue,int SearchBy, int? count, int? skip, bool includeCustomer = false, bool includeImageData = false);
    Task<AppResult<IEnumerable<OteScheduleDTO>>> GetTicketDetails(int activityId, int dateId);
    Task<AppResult<IEnumerable<Entities.OteTicket>>> GetByPurchaseOrderId(int purchaseOrderId, bool includeCustomer = false, bool includeImageData = false);
    Task<AppResult<IEnumerable<BookedCustomerDTO>>> BookedCustomers(int activityId, int? dateId, int limit, int offset);
    Task<AppResult<IEnumerable<OteTicketDTO>>> GetAllTicketPurchased(int activityId);
}