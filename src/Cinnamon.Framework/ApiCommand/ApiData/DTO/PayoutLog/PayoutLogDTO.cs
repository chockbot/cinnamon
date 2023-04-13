namespace Cinnamon.Framework.ApiCommand.ApiData.DTO.PayoutLog;

public class PayoutLogDTO 
{
    public int Id {get; set;}
    public int PurchaseOrderId {get; set;}
    public int CustomerId {get; set;}
    public decimal Amount {get; set;}
    public int Status {get; set;}
    public string Remarks {get; set;}
    public string Payload {get; set;}
}