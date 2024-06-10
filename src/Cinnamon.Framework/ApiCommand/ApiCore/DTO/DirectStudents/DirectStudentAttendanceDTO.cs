namespace Cinnamon.Framework.ApiCommand.ApiCore.DTO.DirectStudents;

public class DirectStudentAttendanceDTO
{
    public int DirectStudentSessionId { get; set; }
    public bool IsPresent { get; set; }
    public DateTime Date { get; set; }
}
