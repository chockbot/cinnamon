namespace Cinnamon.Api.Core.Services.ActivityService.Interactors.Results
{
    public class GetActivityScheduleTimesResult
    {
        public IEnumerable<ActivityScheduleTime> ActivityScheduleTimes { get; set; }

    }

    public class ActivityScheduleTime
    {
        public int ActivityScheduleTimeId { get; set; }
        public int ActivityScheduleId { get; set; }
        public int DayOfWeek { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public bool IsAvailable { get; set; }
    }
}
