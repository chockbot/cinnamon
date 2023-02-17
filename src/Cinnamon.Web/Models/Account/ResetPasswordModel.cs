using System.ComponentModel.DataAnnotations;
using Blazorise;

namespace Cinnamon.Web.Models.Account;

public class ResetPasswordModel 
{
    public ResetModel Model {get; set;} = new();

    public bool IsSubmittingResetPassword {get; set;}
    public string ErrorMessage {get; set;}
    public bool IsShowErrorMessage {get; set;}

    public Validations FormValidation {get; set;}

    public class ResetModel 
    {
        [Required]
        [StringLength(255, ErrorMessage = "Minimum of six characters", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string NewPassword {get; set;}
        [Required]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Must equal to New Password")]
        public string ConfirmPassword {get; set;}
    }
}