using Cinnamon.Framework.ApiCommand.ApiCore.DTO.Activity;

namespace Cinnamon.Api.Core.Services.DashboardService.Interactors.Results;
public class GetTicketDetailsResult
{
    public IEnumerable<OTETicket> OTETickets { get; set; }
    public class OTETicket
    {
        public int ActivityId { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public string Recurrences { get; set; }
        public OtePricingDTO OtePricingDTO { get; set; }
    }
}
