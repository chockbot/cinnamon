using Cinnamon.Framework.ApiCommand.ApiCore;
namespace Cinnamon.Api.Core.Services.OnGoingActivityService.Interactors.Results;
public class GetReviewsByCustomerIdResult
{
    public ErrorInfo? ErrorInfo { get; set; }
    public Pagination? Pagination { get; set; }
    public IEnumerable<Reviews> Review { get; set; }
    public class Reviews
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int MakerId { get; set; }
        public int ActivityId { get; set; }
        public int ScheduleId { get; set; }
        public int StudentId { get; set; }
        public int Rating { get; set; }
        public string Review { get; set; }
        public DateTime ReviewDate { get; set; }
    }
}
