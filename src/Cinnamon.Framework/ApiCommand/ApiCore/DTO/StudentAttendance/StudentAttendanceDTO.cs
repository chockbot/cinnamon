namespace Cinnamon.Framework.ApiCommand.ApiCore.DTO.StudentAttendance;

public class StudentAttendanceDTOs
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public DateTime Date { get; set; }
    public bool IsPresent { get; set; }
}
