namespace Cinnamon.Framework.ApiCommand.ApiData.OteTicket.Request;

public class GetByActivityIdArgs 
{
    public int? ActivityId { get; set; }
    public string? SearchValue { get; set; }
    public int? SearchBy { get; set; }
    public int? PageIndex { get; set; }
    public int? CountPerPage { get; set; }
    public bool? IncludeCustomer {get; set;}
    public bool? IncludeImageAsResult {get; set;}
}