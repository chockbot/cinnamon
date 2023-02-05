namespace Cinnamon.Web.Models.Entities;

public class StudentAttendance
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int StudentId { get; set; }
    public int ScheduleId { get; set; }
    public bool IsPresent { get; set; }
}
