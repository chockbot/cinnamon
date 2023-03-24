namespace Cinnamon.Framework.ApiCommand.ApiCore.DTO.PurchaseOrder;

public class RequestedRefundDTO
{
    public int Id { get; set; }
    public string ReferenceNo {get; set;}
    public string ActivityTitle {get; set;}
    public int Status {get; set;}
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Reason { get; set; }
    public decimal? OverAllTotal { get; set; }
    public decimal? RefundAmountGiven { get; set; }
}