using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.DashboardService.Interactors;

public class GetCurrentDateAttendanceArgs : IInteractor
{
    public int ActivityId {get; set;}
    public int ScheduleId {get; set;}
}