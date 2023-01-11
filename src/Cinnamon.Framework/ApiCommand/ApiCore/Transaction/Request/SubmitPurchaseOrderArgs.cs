using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiCore.Transaction.Request;

public class SubmitPurchaseOrderArgs 
{
    [Required]
    public int ActivityId {get; set;}
    [Required]
    public int ScheduleId {get; set;}
    [Required]
    public int NumberOfHeads {get; set;}
    public string? CouponCode {get; set;}
}