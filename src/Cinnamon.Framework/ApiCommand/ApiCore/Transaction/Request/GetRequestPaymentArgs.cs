using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiCore.Transaction.Request;

public class GetRequestPaymentArgs 
{
    [Required]
    public string Token {get; set;}

    [Required]
    public string Guid {get; set;}
}