using Cinnamon.Framework.Interactor;
namespace Cinnamon.Api.Core.Services.DirectStudentService.Interactors;

public class GetDirectStudentByIdArgs : IInteractor
{
    public int StudentId { get; set; }
    public int ActivityId { get; set; }
    public int ScheduleId { get; set; }
}
