using Cinnamon.Framework.ApiCommand.ApiCore;

namespace Cinnamon.Api.Core.Services.AccountService.Interactors.Results
{
    public class GetAllCustomerResult
    {
        public ErrorInfo? ErrorInfo { get; set; }
        public Pagination? Pagination { get; set; }
        public IEnumerable<Customer> Customers { get; set; }

        public class Customer
        {
            public int Id { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public DateTime Birthdate { get; set; }
            public string? About { get; set; }
            public string? ProfileImg { get; set; }
            public bool IsMaker { get; set; }
            public int IsVerified { get; set; }
            public DateTime IsVerifiedDate { get; set; }
            public bool ExternalLogin { get; set; }
            public DateTime DateJoined { get; set; }
            public string Handler { get; set; }
            public string FrontIdImagePath { get; set; }
            public string BackIdImagePath { get; set; }
            public bool IsOG { get; set; }
            public DateTime IsOGDate { get; set; }
            public bool IsOF { get; set; }
            public DateTime IsOFDate { get; set; }
            public CustomerPricing CustomerPricing {get; set;}
            public bool IsAccountBan { get; set; }
        }

        public class  CustomerPricing 
        {
            public decimal Rate {get; set;}
            public bool IsManualPayment {get; set;}
        }
    }
}
