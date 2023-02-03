using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.DashboardService.Interactors;

public class GetStudentAttendanceArgs : IInteractor
{
    public DateTime Date {get; set;}
    public int ActivityId {get; set;}
    public int ScheduleId {get; set;}
    public bool ForceCreate {get; set;}
}