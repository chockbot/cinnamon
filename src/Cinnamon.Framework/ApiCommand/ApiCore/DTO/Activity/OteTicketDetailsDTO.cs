namespace Cinnamon.Framework.ApiCommand.ApiCore.DTO.Activity;

public class OteTicketDetailsDTO 
{
    public string EventName {get; set;}
    public string EventLocation {get; set;}
    public DateTime EventDate {get; set;}
    public string ImageSrc {get; set;}
    public IEnumerable<Ticket> Tickets {get; set;}
    
    
    public class Ticket 
    {
        public string Name {get; set;}
        public string QRCodeData {get; set;}
    }
}