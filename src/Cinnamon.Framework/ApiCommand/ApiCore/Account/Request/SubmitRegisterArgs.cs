using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiCore.Account.Request;

public class SubmitRegisterArgs
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    public string Password {get; set;}
    [Required]
    public DateTime Birthdate { get; set; }
    public string PhoneNumber { get; set; }
    [Required]
    public string ProfilePath { get; set; }
    public bool IsMaker { get; set; } = false;
    public bool ExternalLogin { get; set; } = false;
    public bool HasAcceptedTerms { get; set; }
    public bool IsGuest { get; set; } = false;
}