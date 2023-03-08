namespace Cinnamon.Framework.ApiCommand.ApiData.PurchaseOrder.Request;

public class GetAllPurchaseOrderArgs 
{
    public int? PageIndex { get; set; }
    public int? CountPerPage { get; set; }
    public int? CustomerId {get; set;}
    public bool? IncludeActivity {get; set;}
    public bool? IncludeSchedule {get; set;}
    public int? Status {get; set;}
}