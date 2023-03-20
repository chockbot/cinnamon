using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Web.Models.Forms;

public class PaymentVerifyPayoutCallback 
{
    [Required]
    public Data data {get; set;}
    
    public class Data 
    {
        [Required]
        public string reference_id {get; set;}
        [Required]
        public string status {get; set;}
        public string? failure_code {get; set;}
    }
}