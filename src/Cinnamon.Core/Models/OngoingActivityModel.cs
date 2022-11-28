namespace Cinnamon.Core.Models;

public class OngoingActivityModel : BaseModel 
{
    public int Id { get; set; }
    public int ActivityId { get; set; }
    public int CustomerId { get; set; }
    public int PurchaseOrderId {get; set;}

    public virtual ActivityModel Activity { get; set; }
    public virtual CustomerModel Customer { get; set; }
}