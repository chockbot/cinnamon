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
    [Required]
    public string PaymentMethod {get; set;}
    public string? PaymentChannel {get; set;}
    [Required]
    public IEnumerable<Enrollee> Students {get; set;}

    public class Enrollee 
    {
        [Required]
        public int FamilyMemberId {get; set;}
        [Required]
        public string Name {get; set;}
    }
}