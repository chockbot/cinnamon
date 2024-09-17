using System.ComponentModel.DataAnnotations;
using Blazorise;

namespace Cinnamon.Web.Models.Account;

public class ResetPasswordModel 
{
    public ResetModel Model {get; set;} = new();

    public bool IsSubmittingResetPassword {get; set;}
    public string Message {get; set;}
    public bool IsShowErrorMessage {get; set;}
    public bool IsShowSuccessMessage {get; set;}

    public Validations FormValidation {get; set;}

    public TextEdit TextNewPassword {get; set;}

    public TextEdit TextConfirmPassword {get; set;}

    public bool IsRevealNewPassword {get; set;}
    public bool IsRevealConfirmPassword {get; set;}

    public class ResetModel 
    {
        [Required(ErrorMessage = "New Password field is required.")]
        [StringLength(255, ErrorMessage = "Minimum of six characters", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string NewPassword {get; set;}
        [Required(ErrorMessage = "Confirm Password field is required.")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Must equal to New Password")]
        public string ConfirmPassword {get; set;}
    }
}