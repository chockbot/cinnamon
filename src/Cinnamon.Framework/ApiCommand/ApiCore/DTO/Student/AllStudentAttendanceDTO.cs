namespace Cinnamon.Framework.ApiCommand.ApiCore.DTO.Student;

public class AllStudentAttendanceDTO
{
    public int StudentId { get; set; }
    public DateTime Date { get; set; }
    public bool IsPresent { get; set; }
    public StudentDTO studentDTO { get; set; }
}
