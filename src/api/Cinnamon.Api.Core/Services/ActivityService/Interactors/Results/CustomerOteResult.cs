namespace Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;

public class CustomerOteResult
{
    public IEnumerable<CustomerOte> CustomerOtes { get; set; }
    
    public class CustomerOte 
    {
        public int ActivityId { get; set; }
        public int CustomerId { get; set; }
        public int PurchaseOrderId { get; set; }
        public string CustomerEmail { get; set; }
        public string EventTitle { get; set; }
        public string ImageSrc { get; set; }
    }
}