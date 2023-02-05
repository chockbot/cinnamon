namespace Cinnamon.Framework.ApiCommand.ApiCore.DTO.Student;

public class StudentAttendanceDTO : StudentDTO
{
    public int StudentId {get; set;}
    public DateTime Date {get; set;}
    public bool IsPresent {get; set;}
}
