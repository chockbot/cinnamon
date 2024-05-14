namespace Cinnamon.Api.Data.Repository.Entities;

public class DirectStudentSession : BaseEntity
{
    public int DirectStudentInfoId {get; set;}
    public int ActivityId {get; set;}
    public int ScheduleId {get; set;}
    public string Name {get; set;}
    public string StudentNo {get; set;}
    public int NumberOfSessions {get; set;}
    public int SessionsAttended {get; set;}
    public string Remarks {get; set;}
    public string Status {get; set;}

    public virtual DirectStudentInfo DirectStudentInfo {get; set;}
}