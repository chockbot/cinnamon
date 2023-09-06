using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.OnGoingActivityService.Interactors;

public class GetAllStudentsByIdArgs : IInteractor
{
    public int CustomerId { get; set; }
    public int? PageIndex { get; set; }
    public int? CountPerPage { get; set; }
}
