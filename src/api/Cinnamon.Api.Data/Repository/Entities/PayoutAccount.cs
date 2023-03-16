namespace Cinnamon.Api.Data.Repository.Entities;

public class PayoutAccount : BaseEntity
{
    public string AccountNumber {get; set;}
    public string AccountHolder {get; set;}
    public string Payloads {get; set;}
}