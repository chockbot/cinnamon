using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.Disbursement.Request;

public class GetDisbursementByProviderArgs
{
    public int ProviderId { get; set; } 
    public string? FilterBy { get; set; }
    public string? FilterValue { get; set; }
    public int? PageIndex { get; set; }
    public int? CountPerPage { get; set; }
    
    [Required]
    public string PayoutString {get; set;}
}
