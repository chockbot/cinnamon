using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.DashboardService.Interactors;
public class GetTicketDetailsArgs : IInteractor
{
    public int ActivityId { get; set; }
    public int DateId {get; set;}
}
