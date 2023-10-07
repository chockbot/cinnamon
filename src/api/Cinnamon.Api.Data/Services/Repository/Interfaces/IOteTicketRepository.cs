using Cinnamon.Framework.ApiCommand.ApiData.DTO.OteTicket;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Data.Services.Repository.Interfaces;

public interface IOteTicketRepository 
{
    Task<AppResult<OteTicketDTO>> GetByCode(string code);
    Task<AppResult<IEnumerable<OteTicketDTO>>> GetByActivityId(int activityId, bool includeCustomer = false, bool includeImageAsResult = false);
    Task<AppResult<IEnumerable<OteTicketDTO>>> CreateMany(IEnumerable<OteTicketDTO> tickets, bool includeImageAsResult = false);
    Task<AppResult<OteTicketDTO>> Update(OteTicketDTO ticket);
}