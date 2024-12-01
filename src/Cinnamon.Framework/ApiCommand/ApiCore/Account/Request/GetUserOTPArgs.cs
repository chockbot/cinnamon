using System.ComponentModel.DataAnnotations;
namespace Cinnamon.Framework.ApiCommand.ApiCore.Account.Request;

public class GetUserOTPArgs
{
    [Required]
    public string Email { get; set; }
}
