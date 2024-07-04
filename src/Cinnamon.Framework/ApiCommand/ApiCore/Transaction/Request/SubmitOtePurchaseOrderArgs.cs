using System.ComponentModel.DataAnnotations;
using Cinnamon.Framework.ValidationAttributes;

namespace Cinnamon.Framework.ApiCommand.ApiCore.Transaction.Request;

public class SubmitOtePurchaseOrderArgs 
{
    [Required]
    public int ActivityId {get; set;}
    public string? CouponCode {get; set;}
    [Required]
    public string PaymentMethod {get; set;}
    public string? PaymentChannel {get; set;}
    public OteCardDetails? CardInformation {get; set;}
    [Required]
    public bool IsCreditsApplied {get; set;}
    [Required]
    public IEnumerable<OTeTicket> Tickets {get; set;}

    public IEnumerable<ActivityQuestion>? Questions {get; set;}

    public class OteCardDetails 
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

    public class OTeTicket 
    {
        [Required]
        public int Id {get; set;}
        [Required]
        public int Count {get; set;}
    }

    public class ActivityQuestion 
    {
        [Required]
        public int Id {get; set;}

        [Required]
        public string Question {get; set;}

        public string? Answer {get; set;}
    }
}