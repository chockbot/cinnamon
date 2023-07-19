using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.OnGoingActivityService.Interactors;

public class GetStudentLastAttendanceArgs : IInteractor
{
    public int Id { get; set; }
    public int ActivityId { get; set; }
    public int ScheduleId { get; set; }
}
