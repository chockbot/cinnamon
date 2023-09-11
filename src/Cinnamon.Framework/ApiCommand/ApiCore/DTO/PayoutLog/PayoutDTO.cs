namespace Cinnamon.Framework.ApiCommand.ApiCore.DTO.PayoutLog;

public class PayoutDTO
{
    public int Id { get; set; }
    public int PurchaseOrderId { get; set; }
    public int CustomerId { get; set; }
    public decimal Amount { get; set; }
    public int Status { get; set; }
    public DateTime PayoutDate { get; set; }
}
