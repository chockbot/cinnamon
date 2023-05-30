using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.CustomerPricing.Request;

public class CreateCustomerPricingArgs 
{
    [Required]
    public int CustomerId {get; set;}

    [Required]
    [EmailAddress]
    public string Email {get; set;}

    [Required]
    public decimal Rate {get; set;}
}