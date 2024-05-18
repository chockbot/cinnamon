namespace Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;

public class OteScheduleDatesResult
{
    public IEnumerable<OteDateSchedule> OteDateSchedules {get; set;}

    public class OteDateSchedule 
    {
        public int Id {get; set;}
        public int ScheduleId {get; set;}
        public DateTime Date {get; set;}
        public DateTime DateStart {get; set;}
        public DateTime DateEnd {get; set;}
    }
}