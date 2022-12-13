namespace Cinnamon.Framework.ApiCommand.ApiData.Customer.Request;

public class GetAllCustomersArgs
{
    public bool? IsVerified { get; set; }
    public int? PageIndex { get; set; }
    public int? CountPerPage { get; set; }
}