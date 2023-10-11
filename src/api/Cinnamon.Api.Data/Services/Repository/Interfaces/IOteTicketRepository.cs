using Cinnamon.Framework.ApiCommand.ApiData.DTO.OteTicket;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.OteSchedule;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Data.Services.Repository.Interfaces;

public interface IOteTicketRepository 
{
    Task<AppResult<OteTicketDTO>> GetByCode(string code);
    Task<AppResult<IEnumerable<OteTicketDTO>>> GetByActivityId(int activityId, int? count, int? skip, bool includeCustomer = false, bool includeImageAsResult = false);
    Task<AppResult<IEnumerable<OteTicketDTO>>> CreateMany(IEnumerable<OteTicketDTO> tickets, bool includeImageAsResult = false);
    Task<AppResult<OteTicketDTO>> Update(OteTicketDTO ticket);
    Task<AppResult<IEnumerable<OteScheduleDTO>>> GetTicketDetails(int activityId);
    Task<AppResult<IEnumerable<OteTicketDTO>>> GetByPurchaseOrderId(int purchaseOrderId, bool includeCustomer = false, bool includeImageAsResult = false);
}