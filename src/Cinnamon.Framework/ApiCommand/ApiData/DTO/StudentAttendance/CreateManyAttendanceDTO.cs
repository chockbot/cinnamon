namespace Cinnamon.Framework.ApiCommand.ApiData.DTO.StudentAttendance;

public class CreateManyAttendanceDTO 
{
    public int StudentId {get; set;}
    public bool IsPresent {get; set;}
    public DateTime Date {get; set;}
}