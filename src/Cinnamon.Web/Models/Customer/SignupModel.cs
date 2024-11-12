using System.ComponentModel.DataAnnotations;
using Blazorise;
using Cinnamon.Framework.ValidationAttributes;
using Cinnamon.Web.Models.Entities;

namespace Cinnamon.Web.Models.Customer;

public class SignupModel
{
    public bool IsClickButton {get; set;}
    public string ModalClass {get; set;}
    public string ModalDisplay {get; set;}
    public bool ShowBackdrop {get; set;}
    public Validations FormValidation {get; set;}
    
    public bool IsSubmitting {get; set;}
    public bool IsShowErrorMessage {get; set;}
    public bool IsShowSuccessMessage {get; set;}
    public string Message {get; set;}

    public bool IsRevealPassword {get; set;}
    public bool IsRevealConfirmPassword {get; set;}

    [Required(ErrorMessage = "Required first name field.")]
    public string FirstName {get; set;}
    [Required(ErrorMessage = "Required last name field.")]
    public string LastName {get; set;}
    [Required]
    [DateAgeRange(MinAge = 13, MaxAge = 120, ErrorMessage = "Please provide valid birthdate. Age must between 18 to 120 yrs old")]
    public DateTime Birthdate {get; set;} = DateTime.Now.AddYears(-13);
    [Required(ErrorMessage = "Required mobile number field.")]
    [RegularExpression("^(09|\\+639)\\d{9}$", ErrorMessage = "Please provide valid phone number.")]
    public string PhoneNumber { get; set; }
    [Required]
    public string Email {get; set;}
    [Required(ErrorMessage = "Password required")]
    public string Password {get; set;}
    [Required(ErrorMessage = "Confirm Password required")]
    [Compare("Password", ErrorMessage = "Password and Confirm Password do not match")]
    public string ConfirmPassword {get; set;}
}