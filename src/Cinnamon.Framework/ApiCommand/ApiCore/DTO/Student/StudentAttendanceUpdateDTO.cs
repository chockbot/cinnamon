namespace Cinnamon.Framework.ApiCommand.ApiCore.DTO.Student;

public class StudentAttendanceUpdateDTO
{
    public int ActivityId {get; set;}
    public int ScheduleId {get; set;}
    public int StudentId {get; set;}
    public bool IsPresent {get; set;}
}
