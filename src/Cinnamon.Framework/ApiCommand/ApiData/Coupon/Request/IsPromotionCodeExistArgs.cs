using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.Coupon.Request;

public class IsPromotionCodeExistArgs
{
    [Required]
    public string Code {get; set;}

    [Required]
    public int ActivityId {get; set;}

    [Required]
    public int CustomerId {get; set;}
}
