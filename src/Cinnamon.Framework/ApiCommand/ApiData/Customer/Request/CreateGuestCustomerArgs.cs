using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.Customer.Request;

public class CreateGuestCustomerArgs
{
    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    [Required]
    public string Email { get; set; }

    public DateTime? Birthdate { get; set; }

    public string? PhoneNumber { get; set; }

    public string? About { get; set; }

    public string? ProfilePath { get; set; }

    public string? Handler { get; set; }
    
    public bool? HasAcceptedTerms { get; set; }
}