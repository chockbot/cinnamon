using System.ComponentModel.DataAnnotations;
using Cinnamon.Framework.ValidationAttributes;
using Blazorise;

namespace Cinnamon.Web.Models.Account;

public class ExternalRegisterModel 
{
    public RegisterModel Model {get; set;} = new();
    public Validations FormValidation {get; set;}
    
    public bool IsSubmitting {get; set;}
    public string ErrorMessage {get; set;}
    public bool IsShowErrorMessage {get; set;}
    public bool IsRevealPassword {get; set;}

    public class RegisterModel 
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DateAgeRange(MinAge = 18, MaxAge = 120, ErrorMessage = "Please provide valid birthdate. Age must between 18 to 120 yrs old")]
        public DateTime Birthdate { get; set; }

        [Required]
        [RegularExpression("^(09|\\+639)\\d{9}$", ErrorMessage = "Please provide valid phone number.")]
        public string PhoneNumber { get; set; }

        [Required]
        public string Password { get; set; }

        public bool IsEmptyUsername {get; set;}
    }
}