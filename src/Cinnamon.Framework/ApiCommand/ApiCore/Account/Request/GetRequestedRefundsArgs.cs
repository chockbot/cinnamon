using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiCore.Account.Request;

public class GetRequestedRefundsArgs
{
    [Required]
    public int Status {get; set;}
    public int? PageIndex { get; set; }
    public int? CountPerPage { get; set; }
    public int? CustomerId { get; set; }
    public bool? IncludeCustomer { get; set; }
    public bool? IncludePurchaseOrder { get; set; }
    public bool? IsAdmin { get; set; }
}