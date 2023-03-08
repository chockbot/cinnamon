namespace Cinnamon.Framework.ApiCommand.ApiData.RequestRefund.Request;

public class GetAllRequestRefundArgs 
{
    public int? PageIndex { get; set; }
    public int? CountPerPage { get; set; }
    public int? CustomerId {get; set;}
    public bool? IncludeCustomer {get; set;}
    public bool? IncludePurchaseOrder {get; set;}
}