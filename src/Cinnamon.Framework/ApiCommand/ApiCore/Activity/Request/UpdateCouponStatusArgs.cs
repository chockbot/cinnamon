using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiCore.Activity.Request;

public class UpdateCouponStatusArgs
{
    [Required]
    public int Id {get; set;}

    [Required]
    [Range(0, 1)]
    public int Status {get; set;}
}