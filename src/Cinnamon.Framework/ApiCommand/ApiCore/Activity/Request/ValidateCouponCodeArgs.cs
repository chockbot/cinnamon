using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiCore.Activity.Request;

public class ValidateCouponCodeArgs
{
    [Required]
    public int ActivityId {get; set;}

    [Required]
    public decimal Amount {get; set;}

    [Required]
    public string CouponCode {get; set;}
}