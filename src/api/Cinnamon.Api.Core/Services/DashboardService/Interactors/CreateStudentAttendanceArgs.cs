using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.DashboardService.Interactors;
public class CreateStudentAttendanceArgs : IInteractor
{
    public int StudentId { get; set; }
    public bool IsPresent { get; set; }
    public DateTime AttendanceDate { get; set; }
}
