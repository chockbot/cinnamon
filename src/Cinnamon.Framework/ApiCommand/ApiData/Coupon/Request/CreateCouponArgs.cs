using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.Coupon.Request;

public class CreateCouponArgs
{
    [Required]
    public int ActivityId {get; set;}

    [Required]
    public bool IsAdmin {get; set;}

    [Required]
    public int CustomerId {get; set;}

    [Required]
    public string Name {get; set;}

    [Required]
    public string Code {get; set;}

    [Required]
    [Range(0,1)]
    public int DiscountType {get; set;}

    [Required]
    public decimal Amount {get; set;}

    [Required]
    public decimal MaximumSpend {get; set;}

    [Required]
    public DateTime From {get; set;}

    [Required]
    public DateTime To {get; set;}

    [Required]
    [Range(0,1)]
    public int Status {get; set;}
}
