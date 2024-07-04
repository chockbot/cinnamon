namespace Cinnamon.Api.Data.Repository.Entities;

public class DirectStudentAttendance : BaseEntity 
{
    public int DirectStudentSessionId {get; set;}
    public bool IsPresent {get; set;}
    public DateTime Date {get; set;}

    public virtual DirectStudentSession DirectStudentSession {get; set;}  
}