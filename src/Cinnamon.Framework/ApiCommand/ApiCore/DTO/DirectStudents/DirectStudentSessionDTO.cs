namespace Cinnamon.Framework.ApiCommand.ApiCore.DTO.DirectStudents;

public class DirectStudentSessionDTO 
{
    public int Id {get; set;}
    public int DirectStudentInfoId {get; set;}
    public int ActivityId {get; set;}
    public int ScheduleId {get; set;}
    public string Name {get; set;}
    public string StudentNo {get; set;}
    public int NumberOfSessions {get; set;}
    public int SessionsAttended {get; set;}
    public string Remarks {get; set;}
    public string Status {get; set;}
}