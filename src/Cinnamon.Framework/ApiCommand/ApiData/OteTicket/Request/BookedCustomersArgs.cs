using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.OteTicket.Request;

public class BookedCustomersArgs
{
    [Required]
    public int ActivityId {get; set;}

    public int? DateId {get; set;}

    public int? Limit {get; set;}

    public int? Offset {get; set;}
}