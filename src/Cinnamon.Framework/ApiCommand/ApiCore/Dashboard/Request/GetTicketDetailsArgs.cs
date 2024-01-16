using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiCore.Dashboard.Request;

public class GetTicketDetailsArgs
{
    [Required]
    public int ActivityId { get; set; }
    [Required]
    public int DateId {get; set;}
}
