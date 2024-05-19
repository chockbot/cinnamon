namespace Cinnamon.Framework.ApiCommand.ApiCore.DTO.Activity;

public class OteTicketBookCountDTO
{
    public int BookedCount {get; set;}
    public BookFirstScheduleDTO FirstSchedule {get; set;}

    public class BookFirstScheduleDTO 
    {
        public int Id {get; set;}
        public int ScheduleId {get; set;}
        public DateTime Date {get; set;}
        public DateTime DateStart {get; set;}
        public DateTime DateEnd {get; set;}
    }
}