using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.DashboardService.Interactors;

public class UpdateAttendanceArgs : IInteractor
{
    public int Id { get; set; }
    public bool IsPresent { get; set; }
    public int StudentId { get; set; }
    public int ActivityId { get; set; }
    public DateTime Date { get; set; }
}
