using Cinnamon.Framework.ApiCommand.ApiCore.DTO.Activity;

namespace Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;

public class OteTicketDetailsResult 
{
    public string EventName {get; set;}
    public string EventLocation {get; set;}
    public DateTime EventDate {get; set;}
    public string ImageSrc {get; set;}
    public string EventDescription { get; set;}
    public int ProviderId { get; set; }
    public IEnumerable<Ticket> Tickets {get; set;}
    
    
    public class Ticket 
    {
        public int Id {get; set;}
        public string Name {get; set;}
        public string QRCodeData {get; set;}
        public string VideoLink { get; set;}
        public string LinkTitle { get; set; }
        public string SeatNumber { get; set; }
    }
}