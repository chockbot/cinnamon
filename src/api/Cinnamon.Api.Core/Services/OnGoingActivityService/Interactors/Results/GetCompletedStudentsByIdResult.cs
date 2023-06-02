namespace Cinnamon.Api.Core.Services.OnGoingActivityService.Interactors.Results;
public class GetCompletedStudentsByIdResult
{
    public IEnumerable<Students> Student { get; set; }
    public class Students
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int ScheduleId { get; set; }
        public string StudentNo { get; set; }
        public string StudentName { get; set; }
        public int NumberOfSessions { get; set; }
        public int SessionsAttended { get; set; }
        public string Status { get; set; }
        public int ActivityId { get; set; }
        public string ActivityTitle { get; set; }
        public string ActivityDescription { get; set; }
    }

}
