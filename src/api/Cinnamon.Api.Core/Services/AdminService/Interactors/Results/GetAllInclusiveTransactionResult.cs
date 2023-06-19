namespace Cinnamon.Api.Core.Services.AdminService.Interactors.Results;

public class GetAllInclusiveTransactionResult 
{
    public IEnumerable<Transaction> Transactions {get; set;}

    public class Transaction 
    {
        public int PurchaseOrderId {get; set;}
        public DateTime PurchaseDate {get; set;}
        public int UnitCount {get; set;}
        public decimal UnitPrice {get; set;}
        public decimal Total {get; set;}
        public decimal ConvinienceFee {get; set;}
        public decimal CreditAmount {get; set;}
        public decimal OverallTotal {get; set;}
        public int Status {get; set;}

        public ProviderDTO Provider {get; set;} = new();

        public class ProviderDTO 
        {
            public string FirstName {get; set;}
            public string LastName {get; set;}
            public string Email {get; set;}
        }
    }
}