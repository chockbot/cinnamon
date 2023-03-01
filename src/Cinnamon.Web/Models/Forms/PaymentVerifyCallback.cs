using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Web.Models.Forms;

public class PaymentVerifyCallback 
{
    [Required]
    public Data data {get; set;}

    public class Data 
    {
        [Required]
        public string reference_id {get; set;}
        [Required]
        public string status {get; set;}
    }
}