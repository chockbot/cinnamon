using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Web.Models.Account;

public class ExternaRegisterAutoLogin 
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

    [Required]
    public string Guid {get; set;}

    [Required]
    public string Token {get; set;}
}