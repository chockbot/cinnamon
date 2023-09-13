using Cinnamon.Framework.ApiCommand.ApiCore;
namespace Cinnamon.Api.Core.Services.OnGoingActivityService.Interactors.Results;

public class GetEnrolleeMasterListResult
{
    public ErrorInfo? ErrorInfo { get; set; }
    public Pagination? Pagination { get; set; }
    public IEnumerable<EnrolleeMasterList> EnrolleeMasterLists { get; set; }
    public class EnrolleeMasterList
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
        public int Age { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string ActivityName { get; set; }
    }
}
