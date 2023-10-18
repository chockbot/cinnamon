using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiCore.Dashboard.Request;
public class UpdateOTETicketArgs
{
    [Required]
    public int Id { get; set; }
    [Required]
    public string Status { get; set; }
}
