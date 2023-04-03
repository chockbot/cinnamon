using System.ComponentModel.DataAnnotations;
using Cinnamon.Framework.ValidationAttributes;

namespace Cinnamon.Web.Models.Forms;

public class CardPaymentModel 
{
    [CreditCard(ErrorMessage = "Provide valid card number")]
    [Required(ErrorMessage = "Required card number field")]
    public string CardNumber {get; set;}
    [Required(ErrorMessage = "Required card holder name field")]
    public string CardHolder {get; set;}
    [Required(ErrorMessage = "Required card expiration. Eg: 03/23")]
    [CardExpiration(ErrorMessage = "Provide valid card expiration format. Eg: 03/23")]
    public string Expiration {get; set;}
    [Required(ErrorMessage = "Required card CVV field.")]
    [StringLength(4, MinimumLength = 3, ErrorMessage = "Provide valid CVV")]
    public string CVV {get; set;}
}