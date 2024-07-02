using Cinnamon.Framework.ApiCommand.ApiData.DTO.StudentAttendance;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.ActivitySchedule;
using Cinnamon.Framework.Enums;

namespace Cinnamon.Framework.ApiCommand.ApiData.DTO.Student;

public class StudentDTO 
{
    public int Id {get; set;}
    public int CustomerId {get; set;}
    public int ActivityId {get; set;}
    public int ScheduleId {get; set;}
    public string Name {get; set;}
    public string StudentNo {get; set;}
    public int NumberOfSessions {get; set;}
    public int SessionsAttended { get; set; }
    public int NumberOfBackTracking { get; set; }
    public string Remarks {get; set;}
    public string Status {get; set;}
    public string Title { get; set; }
    public DateTime PurchaseDate { get; set; }
    public DateTime ExpirationStartDate { get; set;}
    public DateTime ExpirationEndDate { get; set;}
    public bool IsDisbursement {get; set;}
    public bool HasReview { get; set; }
    public int HasExpiration { get; set; }
    public int Age { get; set;}
    public string Gender { get; set;}
    public string ActivityTitle { get; set;}
    public string Email { get; set;}
    public int FamilyMemberId { get; set; }
    public StudentType StudentType {get; set;} = StudentType.Cinnamon;
    public DateTime LastAttendance {get; set;}
    public StudentAttendanceDTO studentAttendance { get; set;}
    public ActivityScheduleDTO activitySchedule { get; set;}
}