namespace Cinnamon.Framework.ApiCommand.ApiCore.DTO.Schedule;

public class ActivityScheduleDTO 
{
    public int ActivityId {get; set;}
    public string Title {get; set;}
    public string Description {get; set;}
    public int ScheduleId {get; set;}
    public string ScheduleTitle {get; set;}
    public string ScheduleDescription {get; set;}
    public bool IsActiveSchedule { get; set; }
    public bool IsSetSession { get; set; }
    public string SessionName { get; set; }
    public int HasExpiration { get; set; }
    public DateTime? StartDate { get; set; }
    public IList<ActivityScheduleTimeModel> ActivityScheduleTimes { get; set; } = new List<ActivityScheduleTimeModel>();
}

public class ActivityScheduleTimeModel
{
    public int ActivityScheduleTimeId { get; set; }
    public int ActivityScheduleId { get; set; }
    public int DayOfWeek { get; set; }
    public string StartTime { get; set; }
    public string EndTime { get; set; }
    public bool IsAvailable { get; set; }
}