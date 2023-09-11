namespace Cinnamon.Api.Core.Services.TransactionService.Interactors.Results;
public class GetPayoutsByProviderResult
{
    public IEnumerable<PayoutLog> PayoutsLog { get; set; }  
    public class PayoutLog
    {
        public int Id { get; set; }
        public int PurchaseOrderId { get; set; }
        public int CustomerId { get; set; }
        public decimal Amount { get; set; }
        public int Status { get; set; }
        public DateTime PayoutDate { get; set; }
    }
}
