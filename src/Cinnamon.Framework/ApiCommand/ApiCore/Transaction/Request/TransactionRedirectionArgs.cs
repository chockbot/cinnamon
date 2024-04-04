using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiCore.Transaction.Request;

public class TransactionRedirectionArgs 
{
    [Required]
    public string Guid {get; set;}

    [Required]
    public string Token {get; set;}
}