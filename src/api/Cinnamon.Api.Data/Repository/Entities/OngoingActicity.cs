namespace Cinnamon.Api.Data.Repository.Entities;

public class OngoingActicity : BaseEntity 
{
    public int ActivityId {get; set;}
    public int CustomerId {get; set;}
    public int PurchaseOrderId {get; set;}

    public virtual Activity Activity {get; set;}
    public virtual Customer Customer {get; set;}
}