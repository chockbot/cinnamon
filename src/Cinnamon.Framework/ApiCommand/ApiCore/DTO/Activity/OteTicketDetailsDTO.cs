namespace Cinnamon.Framework.ApiCommand.ApiCore.DTO.Activity;

public class OteTicketDetailsDTO 
{
    public string EventName {get; set;}
    public string EventLocation {get; set;}
    public DateTime EventDate {get; set;}
    public string ImageSrc {get; set;}
    public string EventDescription { get; set; }
    public int ProviderId { get; set; }
    public IEnumerable<TicketDetails> Tickets {get; set;}
    
    
    public class TicketDetails 
    {
        public int Id {get; set;}
        public string Name {get; set;}
        public string QRCodeData {get; set;}
        public string VideoLink { get; set; }
        public string LinkTitle { get; set; }
        public string SeatNumber { get; set; }
    }
}