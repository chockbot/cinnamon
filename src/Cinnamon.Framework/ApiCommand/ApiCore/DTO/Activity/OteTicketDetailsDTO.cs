namespace Cinnamon.Framework.ApiCommand.ApiCore.DTO.Activity;

public class OteTicketDetailsDTO 
{
    public string EventName {get; set;}
    public string EventLocation {get; set;}
    public DateTime EventDate {get; set;}
    public string ImageSrc {get; set;}
    public IEnumerable<TicketDetails> Tickets {get; set;}
    
    
    public class TicketDetails 
    {
        public int Id {get; set;}
        public string Name {get; set;}
        public string QRCodeData {get; set;}
    }
}