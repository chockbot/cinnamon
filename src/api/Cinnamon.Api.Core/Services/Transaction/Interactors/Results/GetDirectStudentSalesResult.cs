namespace Cinnamon.Api.Core.Services.TransactionService.Interactors.Results;
public class GetDirectStudentSalesResult
{
    public IEnumerable<DirectStudentSale> DirectStudentSales { get; set; }
    public class DirectStudentSale
    {
        public decimal Amount { get; set; }
        public DateTime PurchaseDate { get; set; }
    }
}
