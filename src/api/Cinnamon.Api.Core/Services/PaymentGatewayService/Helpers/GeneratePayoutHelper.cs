namespace Cinnamon.Api.Core.Services.PaymentGatewayService.Helpers;

public class GeneratePayoutHelper 
{
    private readonly IDictionary<int,CustomerPayoutSummary> customerPayoutSummaries;

    public GeneratePayoutHelper()
    {
        this.customerPayoutSummaries = new Dictionary<int,CustomerPayoutSummary>();
    }

    public void AddCustomerSummary(int customerId, decimal amount, int puchaseOrderId, 
        string bankChannel, string accountHolder, string accountNumber)
    {
        if(this.customerPayoutSummaries.ContainsKey(customerId))
        {
            var customerSummary = this.customerPayoutSummaries[customerId];
            customerSummary.TotalAmount += amount;
            customerSummary.PurchaseOrderIds.Add(puchaseOrderId);
        }
        else
        {
            var newSummary = new CustomerPayoutSummary { CustomerId = customerId, TotalAmount = amount, 
                BankChannel = bankChannel, AccountHolder = accountHolder, AccountNumber = accountNumber };
            newSummary.PurchaseOrderIds.Add(puchaseOrderId);
            this.customerPayoutSummaries.Add(customerId, newSummary);
        }
    }

    public IList<CustomerPayoutSummary> GetCustomerPayoutSummaries 
    {
        get 
        {
            return this.customerPayoutSummaries.Select(s => s.Value).ToList();
        }
    }

    public class CustomerPayoutSummary
    {
        public int CustomerId {get; set;}
        public decimal TotalAmount {get; set;}
        public string BankChannel {get; set;}
        public string AccountHolder {get; set;}
        public string AccountNumber {get; set;}
        public IList<int> PurchaseOrderIds {get; set;} = new List<int>();
    }
}