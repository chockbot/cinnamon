using Cinnamon.Framework.ApiCommand.ApiCore;
namespace Cinnamon.Api.Core.Services.DashboardService.Interactors.Results;
public class GetEnrolledStudentsByProviderResult
{
    public ErrorInfo? ErrorInfo { get; set; }
    public Pagination? Pagination { get; set; }
    public IEnumerable<EnrolledStudents> EnrolledStudentsList { get; set; } 

    public class EnrolledStudents
    {
        public int Id { get; set; }
        public int ActivityId { get; set; }
        public int ScheduleId { get; set; }
        public string Name { get; set; }
        public string StudentNo { get; set; }
        public int NumberOfSessions { get; set; }
        public int SessionsAttended { get; set; }
        public string Remarks { get; set; }
        public string Title { get; set; }
        public DateTime ExpirationStartDate { get; set; }
        public DateTime ExpirationEndDate { get; set; }
        public int HasExpiration { get; set; }
        public string ActivityTitle { get; set; }
    }
}
