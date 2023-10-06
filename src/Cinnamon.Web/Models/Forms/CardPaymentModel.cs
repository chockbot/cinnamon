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
    private string _expiration;

    [Required(ErrorMessage = "Card expiration is required. Eg: 03/23")]
    [CardExpiration(ErrorMessage = "Provide a valid card expiration format. Eg: 03/23")]
    public string Expiration
    {
        get
        {
            // If _expiration is not empty and has at least 4 characters, format it as MM/YY
            if (!string.IsNullOrEmpty(_expiration) && _expiration.Length >= 4)
            {
                return $"{_expiration.Substring(0, 2)}/{_expiration.Substring(2, 2)}";
            }
            else
            {
                // Return the cleaned input as is
                return _expiration;
            }
        }
        set
        {
            if (!string.IsNullOrEmpty(value))
            {
                // Remove any non-numeric characters from the input
                string cleanedInput = new string(value.Where(char.IsDigit).ToArray());

                // Ensure the cleaned input has at least 4 characters (MMYY)
                if (cleanedInput.Length >= 4)
                {
                    // Extract the first 4 characters (MMYY)
                    _expiration = cleanedInput.Substring(0, 4);
                }
                else
                {
                    _expiration = cleanedInput; // Use the cleaned input as is
                }
            }
            else
            {
                _expiration = value; // Handle null or empty input
            }
        }
    }

    [Required(ErrorMessage = "Required card CVV field.")]
    [StringLength(4, MinimumLength = 3, ErrorMessage = "Provide valid CVV")]
    [DataType(DataType.Password)]
    public string CVV {get; set;}

    public bool IsRevealPassword { get; set; }
}