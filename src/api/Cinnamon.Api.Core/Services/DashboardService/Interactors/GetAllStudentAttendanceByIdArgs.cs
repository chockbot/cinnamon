using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.DashboardService.Interactors;

public class GetAllStudentAttendanceByIdArgs : IInteractor
{
    public int StudentId { get; set; }
    public int ActivityId { get; set; }
    public int ScheduleId { get; set; }
}
