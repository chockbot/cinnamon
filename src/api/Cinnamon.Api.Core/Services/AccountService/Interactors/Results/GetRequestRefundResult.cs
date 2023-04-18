using Cinnamon.Framework.ApiCommand.ApiCore;

namespace Cinnamon.Api.Core.Services.AccountService.Interactors.Results;

public class GetRequestRefundResult 
{
    public Pagination? Pagination { get; set; }
    public IEnumerable<RequestedRefund> RequestedRefunds {get; set;}
    
    public class RequestedRefund
    {
        public int Id { get; set; }
        public string ReferenceNumber {get; set;}
        public string ExperienceTitle {get; set;}
        public int Status {get; set;}
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Reason { get; set; }
        public decimal? OverAllTotal { get; set; }
        public decimal RefundAmountGiven { get; set; }
    }
}