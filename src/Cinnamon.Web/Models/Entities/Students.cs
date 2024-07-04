using Cinnamon.Framework.Enums;

namespace Cinnamon.Web.Models.Entities;

public class Students
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public int ActivityId { get; set; }
    public int ScheduleId { get; set; }
    public string Name { get; set; }
    public string StudentNo { get; set; }
    public int NumberOfSessions { get; set; }
    public int SessionsAttended { get; set; }
    public int NumberOfBacktracking { get; set; }
    public string Remarks { get; set; }
    public string Status { get; set; }
    public string Title { get; set; }
    public DateTime PurchaseDate { get; set; }
    public DateTime ExpirationDateStart { get; set; }
    public DateTime ExpirationDateEnd { get;set; }
    public bool HasReview { get; set; }
    public string ActivityName { get; set; }
    public int Age { get; set; }
    public string Gender { get; set; }
    public string Email { get; set; }
    public int FamilyMemberId { get; set; }
    public StudentAttendance studentAttendance { get; set; }
    public ActivitySchedule activitySchedule { get; set; }
    public string ActivityType { get; set; }
    public StudentType StudentType { get; set; } = StudentType.Manual;
}
