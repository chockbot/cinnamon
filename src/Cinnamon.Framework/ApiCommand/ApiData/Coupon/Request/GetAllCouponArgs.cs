namespace Cinnamon.Framework.ApiCommand.ApiData.Coupon.Request;

public class GetAllCouponArgs
{
    public int? PageIndex { get; set; }
    public int? CountPerPage { get; set; }
    public bool? IncludeActivity {get; set;}
    public int? CustomerId {get; set;}
}
