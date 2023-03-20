using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiCore.Payment.Request;

public class VerifyPayoutCallbackArgs 
{
    [Required]
    public string ReferenceId {get; set;}
    [Required]
    public string CallbackToken {get; set;}
    [Required]
    public string Status {get; set;}
    public string? FailureCode {get; set;}
}