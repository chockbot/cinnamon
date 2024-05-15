using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.DashboardService.Interactors;
public class GetEnrolledStudentsByProviderArgs : IInteractor
{
    public int? PageIndex { get; set; }
    public int? CountPerPage { get; set; }
    public int ProviderId { get; set; }
    public string SearchValue { get; set; }
    public int SearchBy { get; set; }
}
