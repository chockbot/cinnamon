namespace Cinnamon.Api.Data.Repository.Entities;

public class PayoutAccount : BaseEntity
{
    public int CustomerId {get; set;}
    public string AccountNumber {get; set;}
    public string AccountHolder {get; set;}
    public string Payloads {get; set;}
}