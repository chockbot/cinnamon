using System.ComponentModel.DataAnnotations;
using Blazorise;

namespace Cinnamon.Web.Models.Account;

public class ExternalRegisterModel 
{
    public RegisterModel Model {get; set;} = new();
    public Validations FormValidation {get; set;}
    
    public bool IsSubmitting {get; set;}
    public string ErrorMessage {get; set;}
    public bool IsShowErrorMessage {get; set;}

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
        public DateTime Birthdate { get; set; }

        [Required]
        public string Password { get; set; }
    }
}