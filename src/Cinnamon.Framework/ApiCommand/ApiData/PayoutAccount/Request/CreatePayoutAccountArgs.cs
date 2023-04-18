using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.PayoutAccount.Request;

public class CreatePayoutAccountArgs 
{
    [Required]
    public int CustomerId {get; set;}
    [Required]
    public string AccountNumber {get; set;}
    [Required]
    public string AccountHolder {get; set;}
    public string? Payload {get; set;}
    [Required]
    public string BankChannel {get; set;}
}