namespace Cinnamon.Framework.ApiCommand.ApiCore.DTO.Student;

public class StudentAttendanceDTO : StudentDTO
{
    public DateTime Date {get; set;}
    public bool IsPresent {get; set;}
}
