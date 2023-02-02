namespace Cinnamon.Api.Data.Repository.Entities;

public class StudentAttendance : BaseEntity
{
    public int StudentId {get; set;}
    public bool IsPresent {get; set;}
    public DateTime Date {get; set;}

    public Student Student {get; set;}
}