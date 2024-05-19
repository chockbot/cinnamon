namespace Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;

public class OteTicketBookedCountResult 
{
    public int BookedCount {get; set;}

    public FirstScheduleDate FirstOteDate {get; set;}

    public class FirstScheduleDate 
    {
        public int Id {get; set;}
        public int ScheduleId {get; set;}
        public DateTime Date {get; set;}
        public DateTime DateStart {get; set;}
        public DateTime DateEnd {get; set;}
    }
}