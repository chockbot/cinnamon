namespace Cinnamon.Api.Core.Services.Disbursement.Interactors.Results;

public class GetDisbursementsResult 
{
    public IEnumerable<Disbursement> Disbursements {get; set;}

    public class Disbursement 
    {
        public int Id {get; set;}
        public string ProviderEmail {get; set;}
        public string ProviderFirstName {get; set;}
        public string ProviderLastName {get; set;}
        public string Label {get; set;}
        public decimal Amount {get; set;}
        public string Status {get; set;}
        public string Remarks {get; set;}
        public bool InclusivePayment {get; set;}
    }
}