using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiCore.Activity.Request;

public class VerifySharedEventLinkArgs
{
    [Required]
    public string Guid {get; set;}

    [Required]
    public string Token {get; set;}

    [Required]
    public string QrCode {get; set;}
}
