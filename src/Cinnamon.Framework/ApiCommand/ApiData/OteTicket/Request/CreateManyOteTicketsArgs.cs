using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.OteTicket.Request;

public class CreateManyOteTicketsArgs 
{
    [Required]
    public IEnumerable<CreateOteTicketArgs> Tickets {get; set;}

    public bool IncludeImageAsResult {get; set;}
}