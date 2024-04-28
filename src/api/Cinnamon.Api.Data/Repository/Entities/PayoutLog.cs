namespace Cinnamon.Api.Data.Repository.Entities;

public class PayoutLog : BaseEntity 
{
    public int PurchaseOrderId {get; set;}
    public int CustomerId {get; set;}
    public decimal Amount {get; set;}
    public int Status {get; set;}
    public string Remarks {get; set;}
    public string Payload {get; set;}
}