namespace Cinnamon.Api.Data.Repository.Entities
{
    public class ActivityScheduleTime : BaseEntity
    {
        public int ActivityScheduleId { get; set; }
        public int DayOfWeek { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public virtual ActivitySchedule ActivitySchedule { get; set; }
        public virtual IList<OngoingActivityScheduleTime> OngoingActivityScheduleTimes { get; set; }
    }
}
