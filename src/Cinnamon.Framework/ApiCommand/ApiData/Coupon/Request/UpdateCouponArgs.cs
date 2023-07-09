using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.Coupon.Request;

public class UpdateCouponArgs
{
    [Required]
    public int Id {get; set;}
    
    public int? ActivityId {get; set;}

    public bool? IsAdmin {get; set;}

    public int? CustomerId {get; set;}

    public string? Name {get; set;}

    public string? Code {get; set;}

    [Range(0,1)]
    public int? DiscountType {get; set;}

    public decimal? Amount {get; set;}

    public decimal? MaximumSpend {get; set;}

    public DateTime? From {get; set;}

    public DateTime? To {get; set;}

    [Range(0,1)]
    public int? Status {get; set;}
}
