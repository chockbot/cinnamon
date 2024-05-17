using Cinnamon.Framework.ApiCommand.ApiData.OteTicket.Request;
using Cinnamon.Framework.ApiCommand.ApiData.OteTicket.Response;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Modules.DataAccess.Handlers;

public interface IOteTicketData
{
    Task<AppResult<CreateManyOteTicketsResult>> CreateTickets(CreateManyOteTicketsArgs args);
    Task<AppResult<GetByActivityIdResult>> GetByActivityId(int activityId, GetByActivityIdArgs args);
    Task<AppResult<GetByCodeResult>> GetByCode(string code);
    Task<AppResult<UpdateTicketResult>> UpdateTicket(UpdateTicketArgs args);
    Task<AppResult<GetTicketDetailsResult>> GetTicketDetails(GetTicketDetailsArgs args);
    Task<AppResult<GetByPurchaseOrderIdResult>> GetByPurchaseOrderId(int purchaseOrderId, GetByPurchaseOrderIdArgs args);
    Task<AppResult<CreateSharedLinkResult>> CreateSharedLink(CreateSharedLinkArgs args);
    Task<AppResult<GetSharedLinkResult>> GetSharedLink(GetSharedLinkArgs args);
    Task<AppResult<UpdateSharedLinkStatusResult>> UpdateSharedLinkStatus(UpdateSharedLinkStatusArgs args);
    Task<AppResult<CountBookedTicketsResult>> CountBookedTickets(CountBookedTicketsArgs args);
}
