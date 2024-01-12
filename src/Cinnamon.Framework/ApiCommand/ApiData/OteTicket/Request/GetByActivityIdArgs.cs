using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.OteTicket.Request;

public class GetByActivityIdArgs 
{
    public int? ActivityId { get; set; }
    public string? SearchValue { get; set; }
    [Required]
    public int DateId {get; set;}
    public int? PageIndex { get; set; }
    public int? CountPerPage { get; set; }
    public bool? IncludeCustomer {get; set;}
    public bool? IncludeImageAsResult {get; set;}
}