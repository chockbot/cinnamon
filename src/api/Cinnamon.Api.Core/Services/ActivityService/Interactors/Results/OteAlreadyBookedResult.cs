namespace Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;

public class OteAlreadyBookedResult 
{
    public IEnumerable<OteAlreadyBooked> OteAlreadyBookedItems {get; set;}

    public class OteAlreadyBooked 
    {
        public int ActivityId {get; set;}
        public int OteDateId {get; set;}
        public DateTime Date {get; set;}
        public DateTime DateStart {get; set;}
        public DateTime DateEnd {get; set;}
        public int BookCount {get; set;}
    }
}