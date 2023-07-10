using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiCore.Activity.Request;

public class UpdateCouponArgs
{
    [Required]
    public int Id {get; set;}

    [Required]
    public string Name {get; set;}

    [Required]
    public DateTime DateFrom {get; set;}

    [Required]
    public DateTime DateTo {get; set;}
}