using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiCore.Account.Request;

public class SubmitExternalRegisterArgs
{
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    public string PhoneNumber { get; set; }
    [Required]
    public string Password {get; set;}
    [Required]
    public DateTime Birthdate { get; set; }
    [Required]
    public string ProfilePath { get; set; }
    [Required]
    public string Guid {get; set;}
    [Required]
    public string Token {get; set;}
    public bool HasAcceptedTerms { get; set; }
}