using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Web.Models.Account;

public class CreateGuestCustomerModel
{
    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    [EmailAddress]
    public string Email { get; set; }

    public DateTime? Birthdate { get; set; }

    public string? PhoneNumber { get; set; }

    public string? About { get; set; }

    public string? Handler { get; set; }
    
    public bool? HasAcceptedTerms { get; set; }
}