using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Web.Models.Account;

public class RegisterAutoLogin 
{

    public string FirstName { get; set; }

    public string LastName { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public DateTime Birthdate { get; set; }

    public string PhoneNumber { get; set; }

    public string Password { get; set; }

    public bool AcceptFlag { get; set; }

    public bool HasAcceptedTerms { get; set; }

    public bool IsGuest { get; set; } = false;
}