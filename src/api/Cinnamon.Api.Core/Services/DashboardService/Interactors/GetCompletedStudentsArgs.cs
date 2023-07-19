using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.DashboardService.Interactors;

public class GetCompletedStudentsArgs : IInteractor
{
    public IEnumerable<int> ActivityId { get; set; }
}
