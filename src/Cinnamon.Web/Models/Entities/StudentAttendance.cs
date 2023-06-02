using Blazorise;

namespace Cinnamon.Web.Models.Entities;

public class StudentAttendance
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int StudentId { get; set; }
    public int ScheduleId { get; set; }
    public bool IsPresent { get; set; }
    public int ActivityId { get; set; }
    public string Name { get; set; }
    public string ActivityName { get; set; }
    public int NumberOfSessions { get; set; }
    public int SessionsAttended { get; set; }

    // Extra properties
    public Modal ModalRef { get; set; }
}
