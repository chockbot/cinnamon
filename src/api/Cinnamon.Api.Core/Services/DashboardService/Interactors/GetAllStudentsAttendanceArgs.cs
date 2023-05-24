using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.DashboardService.Interactors;

public class GetAllStudentsAttendanceArgs : IInteractor
{
    public int ActivityId { get; set; }
}
