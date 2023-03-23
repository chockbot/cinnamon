using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Web.Models.Forms;

public class CardPaymentModel 
{
    [Required(ErrorMessage = "Required card number field")]
    public string CardNumber {get; set;}
    [Required(ErrorMessage = "Required card holder name field")]
    public string CardHolder {get; set;}
    [Required(ErrorMessage = "Required card expiration. Eg: 03/23")]
    public string Expiration {get; set;}
    [Required(ErrorMessage = "Required card cvv field.")]
    public string CVV {get; set;}
}