namespace Cinnamon.Api.Core.Services.AccountService.Interactors.Results;

public class GetRequestRefundResult 
{
    public IEnumerable<RequestedRefund> RequestedRefunds {get; set;}
    
    public class RequestedRefund
    {
        public string ReferenceNumber {get; set;}
        public string ExperienceTitle {get; set;}
        public int Status {get; set;}
    }
}