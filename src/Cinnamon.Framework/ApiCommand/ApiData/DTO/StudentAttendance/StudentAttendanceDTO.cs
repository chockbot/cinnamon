using Cinnamon.Framework.ApiCommand.ApiData.DTO.Student;

namespace Cinnamon.Framework.ApiCommand.ApiData.DTO.StudentAttendance;

public class StudentAttendanceDTO 
{
    public int Id {get; set;}
    public int StudentId {get; set;}
    public bool IsPresent {get; set;}
    public DateOnly Date {get; set;}
    public StudentDTO Student {get; set;}
}