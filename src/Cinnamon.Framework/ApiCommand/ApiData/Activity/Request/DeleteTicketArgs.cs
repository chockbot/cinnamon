using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.Activity.Request;

public class DeleteTicketArgs
{
    [Required]
    public int Id { get; set; }
}
