using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.Coupon.Request;

public class GetCouponByCodeArgs
{
    [Required]
    public string Code {get; set;}
}
