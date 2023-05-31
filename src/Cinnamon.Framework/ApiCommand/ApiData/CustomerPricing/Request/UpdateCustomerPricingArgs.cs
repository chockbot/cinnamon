using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.CustomerPricing.Request;

public class UpdateCustomerPricingArgs
{
    [Required]
    public int Id {get; set;}

    [Required]
    public decimal Rate {get; set;}
}