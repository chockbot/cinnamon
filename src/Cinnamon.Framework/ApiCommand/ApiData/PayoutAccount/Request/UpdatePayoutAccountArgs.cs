using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.PayoutAccount.Request;

public class UpdatePayoutAccountArgs 
{
    [Required]
    public int Id {get; set;}
    public string? AccountNumber {get; set;}
    public string? AccountHolder {get; set;}
    public string? Payload {get; set;}
}