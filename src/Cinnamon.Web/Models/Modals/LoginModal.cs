using System.ComponentModel.DataAnnotations;
using Blazorise;

namespace Cinnamon.Web.Models.Modals;

public class LoginModal 
{
    public Modal EmailModal {get; set;}
    public Modal PasswordModal {get; set;}
    public Modal VerificationModal {get; set;}
    public Validations EmailModelValidation {get; set;}
    public Validations PasswordModelValidation {get; set;}

    public LoginEmailModel EmailModel {get; set;} = new();
    public LoginPasswordModel PasswordModel {get; set;} = new();

    public class LoginEmailModel 
    {
        [Required(ErrorMessage = "Email address is required")]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string Email {get; set;}

        public bool IsEmailSubmitting {get; set;}
        public bool IsShowErrorMessage {get; set;}
    }

    public class LoginPasswordModel 
    {
        [Required(ErrorMessage = "Password is required")]
        [StringLength(30, ErrorMessage = "Must be between 6 and 30 characters", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password {get; set;}

        public bool IsPasswordSubmitting {get; set;}
        public bool IShowErrorMessage {get; set;}
        public string ErrorMessage {get; set;}
    }
}