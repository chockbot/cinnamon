using Cinnamon.Framework.ApiCommand.ApiData.DTO.OteTicket;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.OteSchedule;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Data.Services.Repository.Interfaces;

public interface IOteTicketRepository 
{
    Task<AppResult<OteTicketDTO>> GetByCode(string code);
    Task<AppResult<IEnumerable<OteTicketDTO>>> GetByActivityId(int activityId, int dateId, string searchValue, int SearchBy, int? count, int? skip, bool includeCustomer = false, bool includeImageAsResult = false);
    Task<AppResult<IEnumerable<OteTicketDTO>>> CreateMany(IEnumerable<OteTicketDTO> tickets, bool includeImageAsResult = false);
    Task<AppResult<OteTicketDTO>> Update(OteTicketDTO ticket);
    Task<AppResult<IEnumerable<OteTicketDTO>>> GetByPurchaseOrderId(int purchaseOrderId, bool includeCustomer = false, bool includeImageAsResult = false);
    Task<AppResult<IEnumerable<OteScheduleDTO>>> GetTicketDetails(int activityId, int oteDateId);
    Task<AppResult<OteSharedLinkDTO>> CreateSharedLink(OteSharedLinkDTO sharedLinkdto);
    Task<AppResult<OteSharedLinkDTO>> GetSharedLinks(string token, string guid);
    Task<AppResult<IEnumerable<OteSharedLinkDTO>>> GetSharedLinks(int activityId, int dateId);
    Task<AppResult<OteSharedLinkDTO>> UpdateSharedLinkStatus(int id, bool status);
    Task<AppResult<int>> CountBookedTickets(int activityId);
    Task<AppResult<IEnumerable<BookedCustomerDTO>>> BookedCustomers(int activityId, int? dateId, int? limit, int? offset);
    Task<AppResult<IEnumerable<OteTicketDTO>>> GetAllTicketPurchased(int activityId);
}