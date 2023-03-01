namespace Cinnamon.Api.Core.Services.DashboardService.Interactors.Results;

public class GetActivityScheduleResult 
{
    public IEnumerable<ActivitySchedule> Schedules {get; set;}

    public class ActivitySchedule 
    {
        public int ActivityId {get; set;}
        public string Title {get; set;}
        public string Description  {get; set;}
        public int ScheduleId {get; set;}
        public string ScheduleTitle {get; set;}
        public string ScheduleDescription {get; set; }
    }
}