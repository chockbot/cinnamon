namespace Cinnamon.Framework.ApiCommand.ApiCore.DTO.Activity;

public class CustomerOteDTO
{
    public int ActivityId { get; set; }
    public int CustomerId { get; set; }
    public int PurchaseOrderId { get; set; }
    public string CustomerEmail { get; set; }
    public string EventTitle { get; set; }
    public string ImageSrc { get; set; }
}

