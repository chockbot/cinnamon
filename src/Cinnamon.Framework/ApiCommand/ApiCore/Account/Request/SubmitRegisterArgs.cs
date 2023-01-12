using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiCore.Account.Request;

public class SubmitRegisterArgs
{
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    public string Password {get; set;}
    [Required]
    public DateTime Birthdate { get; set; }
    [Required]
    public string ProfilePath { get; set; }
    public bool IsMaker { get; set; } = false;
    public bool ExternalLogin { get; set; } = false;
}