using System.ComponentModel.DataAnnotations;
using Blazorise;

namespace Cinnamon.Web.Models.Modals;

public class LoginModal 
{
    public Modal EmailModal {get; set;}
    public Modal PasswordModal {get; set;}

    public Modal ForgotPasswordModal { get; set; }
    public Modal VerificationModal {get; set;}

    public Modal GuestLoginModal { get; set; }

    public Modal OTPModal { get; set; }
    public Validations EmailModelValidation {get; set;}
    public Validations PasswordModelValidation {get; set;}
    public Validations ResetPasswordValidation {get; set;}
    public Validations GuestEmailModelValidation { get; set; }

    public LoginEmailModel EmailModel {get; set;} = new();
    public LoginPasswordModel PasswordModel {get; set;} = new();
    public ForgotPasswordModel ResetPasswordModel {get; set;} = new();
    public LoginGuestEmailModel GuestEmailModel { get; set;} = new();   
    public bool IsResendingVerification {get; set;}
    public bool IsResendVerificationShowError {get; set;}
    public string ResendVrificationErrorMessage {get; set;}

    public bool IsGuestSendingVerification { get; set; }
    public bool IsGuestSendVerificationShowError { get; set; }
    public bool IsGuestResendingVerification { get; set; }
    public bool IsGuestResendVerificationShowError { get; set; }
    public string ResendGuestVerificationErrorMessage { get; set; }

    public string CurrentEmail { get; set; } = string.Empty;

    public string NewEmail { get; set; } = string.Empty;

    public bool IsRevealPassword {get; set;}

    public TextEdit TextEmail {get; set;}
    public TextEdit TextPassword {get; set;}
    public TextEdit TextForgetPassword {get; set;}

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

    public class ForgotPasswordModel 
    {
        [Required(ErrorMessage = "Email address is required")]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string Email {get; set;}

        public bool IsForgotPasswordSubmitting {get; set;}
        public bool IsShowForgotPassworErrordMessage {get; set;}
        public string ForgotPasswordErrorMessage {get; set;}

        public bool IsShowForgorPasswordSuccessMessage {get; set;}
        public string ForgotPasswordSuccessMessage {get; set;}
    }

    public class LoginGuestEmailModel
    {
        [Required(ErrorMessage = "Email address is required")]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public bool IsEmailSubmitting { get; set; }
        public bool IsShowErrorMessage { get; set; }
    }
}