namespace Cinnamon.Framework.ApiCommand.ApiData.Customer.Request;

public class GetAllCustomersArgs
{
    public string? SearchValue { get; set; }
    public bool? IsVerified { get; set; }
    public int? PageIndex { get; set; }
    public int? CountPerPage { get; set; }
    public string? HandlerLike {get; set;}
    public bool? IsOfficialPartner {get; set;}
    public bool? HasVerification {get; set;}
}