using Cinnamon.Framework.ApiCommand.ApiCore.DTO.Schedule;
using Cinnamon.Framework.ApiCommand.ApiCore.DTO.Student;

namespace Cinnamon.Api.Core.Services.OnGoingActivityService.Interactors.Results;

public class GetAllStudentsByIdResult
{
    public IEnumerable<Students> Student { get; set; }

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
        public string Remarks { get; set; }
        public string Status { get; set; }
        public string Title { get; set; }
        public DateTime PurchaseDate { get; set; }
        public DateTime ExpirationStartDate { get; set; }
        public DateTime ExpirationEndDate { get; set; }
        public bool HasReview { get; set; }
        public StudentAttendanceDTO studentAttendanceDTO { get; set; }
        public ActivityScheduleDTO activityScheduleDTO { get; set; }
    }
}
