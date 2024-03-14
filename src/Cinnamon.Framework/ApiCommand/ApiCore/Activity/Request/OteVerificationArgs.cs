using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiCore.Activity.Request;

public class OteVerificationArgs
{
    [Required]
    public string Handler {get; set;}

    [Required]
    public string QrCode {get; set;}

    [Required]
    public int DateId {get; set;}
}
