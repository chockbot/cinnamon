using Cinnamon.Framework.ApiCommand.ApiCore.DTO.Schedule;
using Cinnamon.Framework.Enums;

namespace Cinnamon.Framework.ApiCommand.ApiCore.DTO.Student;

public class StudentDTO
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
    public DateTime ExpirationStartDate { get; set; }
    public DateTime ExpirationEndDate { get; set; }
    public StudentType StudentType {get; set;} = StudentType.Cinnamon;
    public bool HasReview { get; set; }
    public int HasExpiration { get; set; }
    public string ActivityName { get; set; }
    public int Age { get; set; }
    public string Gender { get; set; }
    public string Email { get; set; }
    public int FamilyMemberId { get; set; }
    public StudentAttendanceDTO studentAttendanceDTO { get; set; }
    public ActivityScheduleDTO activityScheduleDTO { get; set; }
}
