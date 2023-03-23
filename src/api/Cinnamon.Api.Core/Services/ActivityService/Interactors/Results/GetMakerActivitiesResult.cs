namespace Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;
public class GetMakerActivitiesResult
{
    public IEnumerable<Activity> Activities { get; set; }

    public class Activity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int OngoingStudents { get; set; }
        public int CompletedStudents { get; set; }
    }
}
