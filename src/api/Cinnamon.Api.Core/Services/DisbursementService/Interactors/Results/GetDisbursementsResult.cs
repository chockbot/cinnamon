namespace Cinnamon.Api.Core.Services.Disbursement.Interactors.Results;

public class GetDisbursementsResult 
{
    public class Disbursement 
    {
        public int PurchaseOrderId {get; set;}
        public int CustomerId {get; set;}
        public string Label {get; set;}
        public decimal Amount {get; set;}
        public string Status {get; set;}
        public string Remarks {get; set;}
        public bool InclusivePayment {get; set;}
    }
}