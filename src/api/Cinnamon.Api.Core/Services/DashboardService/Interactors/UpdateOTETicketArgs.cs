using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.DashboardService.Interactors;
public class UpdateOTETicketArgs : IInteractor
{
    public int Id { get; set; }
    public string Status { get; set; }
}
