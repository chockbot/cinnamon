namespace Cinnamon.Api.Data.Repository.Entities;

public class RequestRefund : BaseEntity
{
    public int CustomerId {get; set;}
    public int PurchaseOrderId {get; set;}
    public string ExperienceTitle {get; set;}
    // 0 = pending, 1 = approved, 2 = disapproved
    public int Status {get; set;}
    public string Reason {get; set;}

    public PurchaseOrder PurchaseOrder {get; set;}
    public Customer Customer {get; set;}
    public decimal RefundAmountGiven { get; set;}
}