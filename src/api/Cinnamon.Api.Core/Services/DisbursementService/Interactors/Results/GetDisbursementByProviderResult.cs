using Cinnamon.Framework.ApiCommand.ApiCore;
namespace Cinnamon.Api.Core.Services.Disbursement.Interactors.Results;

public class GetDisbursementByProviderResult
{
    public ErrorInfo? ErrorInfo { get; set; }
    public Pagination? Pagination { get; set; }
    public IEnumerable<DisbursementByProviderId> DisbursementInformation{ get; set; }
    public class DisbursementByProviderId
    {
        public int Id { get; set; }
        public int ProviderId { get; set; }
        public string ProviderEmail { get; set; }
        public string ProviderFirstName { get; set; }
        public string ProviderLastName { get; set; }
        public string CustomerName { get; set; }
        public string Label { get; set; }
        public decimal Amount { get; set; }
        public string Payload { get; set; }
        public DateTime PayoutDate { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
        public bool InclusivePayment { get; set; }
    }
}
