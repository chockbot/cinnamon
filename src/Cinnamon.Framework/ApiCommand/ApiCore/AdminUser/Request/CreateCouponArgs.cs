using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiCore.AdminUser.Request;

public class AdminCreateCouponArgs
{
    [Required]
    public int ActivityId {get; set;}

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
    public DateTime FromDate {get; set;}

    [Required]
    public DateTime ToDate {get; set;}
}