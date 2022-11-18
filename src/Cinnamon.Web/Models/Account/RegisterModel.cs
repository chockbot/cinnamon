using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Web.Models.Account;

public class RegisterModel 
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}