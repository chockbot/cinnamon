namespace Cinnamon.Framework.ApiCommand.ApiData.DTO.PayoutAccount;

public class PayoutAccountDTO
{
    public int Id {get; set;}
    public int CustomerId {get; set;}
    public string AccountNumber {get; set;}
    public string AccountHolder {get; set;}
    public string Payload {get; set;}
    public string BankChannel {get; set;}
}