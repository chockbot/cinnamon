using System.ComponentModel.DataAnnotations;
using Cinnamon.Framework.ValidationAttributes;

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
    public CardDetails? CardInformation {get; set;}

    public class Enrollee 
    {
        [Required]
        public int FamilyMemberId {get; set;}
        [Required]
        public string Name {get; set;}
    }

    public class CardDetails 
    {
        [CreditCard(ErrorMessage = "Provide valid card number")]
        [Required]
        public string CardNumber {get; set;}
        [Required]
        public string AccountHolder {get; set;}
        [Required]
        [StringLength(4, MinimumLength = 3, ErrorMessage = "Provide valid CVV")]
        public string CVV {get; set;}
        [Required]
        [CardExpiration(ErrorMessage = "Provide valid card expiration format. Eg: 03/23")]
        public string ExpireMonthYear {get; set;}
    }
}