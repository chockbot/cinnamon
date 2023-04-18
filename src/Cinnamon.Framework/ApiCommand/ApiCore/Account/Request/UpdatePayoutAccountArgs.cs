using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiCore.Account.Request;

public class UpdatePayoutAccountArgs
{
    [Required]
    public string AccountHolder {get; set;}
    [Required]
    public string AccountNumber {get; set;}
    [Required]
    public string BankChannel {get; set;}
}
