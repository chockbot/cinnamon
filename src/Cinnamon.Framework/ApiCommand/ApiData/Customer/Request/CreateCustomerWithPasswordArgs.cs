using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.Customer.Request;

public class CreateCustomerWithPasswordArgs
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
    public string PhoneNumber { get; set; }
    public string? About { get; set; }
    [Required]
    public string ProfilePath { get; set; }
    public bool IsMaker { get; set; } = false;
    public bool ExternalLogin { get; set; } = false;
    [Required]
    public string Handler {get; set;}
    public bool HasAcceptedTerms { get; set; }
}