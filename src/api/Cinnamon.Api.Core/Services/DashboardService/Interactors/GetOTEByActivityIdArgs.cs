using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.DashboardService.Interactors;
public class GetOTEByActivityIdArgs : IInteractor
{
    public int ActivityId { get; set; }
    public int DateId {get; set;}
    public int? PageIndex { get; set; }
    public int? CountPerPage { get; set; }
}
