using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiCore.Payment.Request;

public class VerifyCallbackArgs 
{
    [Required]
    public string TransactionId {get; set;}
    [Required]
    public string CallbackToken {get; set;}
    [Required]
    public string Status {get; set;}
    [Required]
    public object Payload {get; set;}
}