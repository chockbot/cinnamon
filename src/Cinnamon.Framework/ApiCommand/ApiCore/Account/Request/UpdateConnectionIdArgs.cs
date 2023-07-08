using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiCore.Account.Request;

public class UpdateConnectionIdArgs
{
    public int CustomerId { get; set; }
    public string? ConnectionId { get; set; }
}