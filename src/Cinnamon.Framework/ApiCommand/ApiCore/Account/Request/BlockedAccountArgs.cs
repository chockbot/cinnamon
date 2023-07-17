using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiCore.Account.Request;

public class BlockedAccountArgs 
{
    [Required]
    public int CustomerId { get; set; }

    [Required]
    public bool IsBlock { get; set; }
}