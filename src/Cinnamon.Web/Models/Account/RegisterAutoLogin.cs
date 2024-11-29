using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Web.Models.Account;

public class RegisterAutoLogin 
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
    public string PhoneNumber { get; set; }

    [Required]
    public string Password { get; set; }

    public bool AcceptFlag { get; set; }
    public bool HasAcceptedTerms { get; set; }
    public bool IsGuest { get; set; }
}