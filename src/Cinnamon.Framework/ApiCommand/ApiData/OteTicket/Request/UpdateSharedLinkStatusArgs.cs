using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.OteTicket.Request;

public class UpdateSharedLinkStatusArgs
{
    [Required]
    public int Id {get; set;}

    [Required]
    public bool Status {get; set;}
}