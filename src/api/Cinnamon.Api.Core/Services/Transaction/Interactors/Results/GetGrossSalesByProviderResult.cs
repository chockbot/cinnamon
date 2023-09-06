namespace Cinnamon.Api.Core.Services.TransactionService.Interactors.Results;

public class GetGrossSalesByProviderResult
{
    public IEnumerable<GrossSale> GrossSales { get; set; }
    public class GrossSale
    {
        public int Id { get; set; }
        public int ActivityId { get; set; }
        public int ScheduleId { get; set; }
        public int CustomerId { get; set; }
        public decimal Total { get; set; }
        public DateTime PurchaseDate { get; set; }
        public int Status { get; set; }
        public string Payload { get; set; }
        public int UnitCount { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
