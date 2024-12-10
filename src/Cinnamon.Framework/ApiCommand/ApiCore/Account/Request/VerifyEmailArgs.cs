using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiCore.Account.Request;

public class VerifyEmailArgs
{
    [Required]
    public string Email { get; set; }
}
