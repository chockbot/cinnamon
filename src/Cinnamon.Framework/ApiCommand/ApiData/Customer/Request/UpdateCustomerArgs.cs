using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.Customer.Request;

public class UpdateCustomerArgs
{
    [Required]
    public int CustomerId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    [EmailAddress]
    public string? Email { get; set; }
    public DateTime? Birthdate { get; set; }
    public string? About { get; set; }
    public string? ProfilePath { get; set; }
    public bool? IsMaker { get; set; }
    public bool? ExternalLogin { get; set; }
    public bool? IsVerified { get; set; }
    public string? FrontIdImagePath { get; set; }
    public string? BackIdImagePath { get; set; }
}