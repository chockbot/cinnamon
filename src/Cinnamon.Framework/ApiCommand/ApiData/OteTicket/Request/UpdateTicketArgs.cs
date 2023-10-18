using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.OteTicket.Request;

public class UpdateTicketArgs
{
    [Required]
    public int Id {get; set;}

    [Required]
    public string Status {get; set;}
}