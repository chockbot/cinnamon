using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.OnlineEvent.Request;

public class DeleteOnlineEventArgs
{
    [Required]
    public int Id { get; set; }
}
