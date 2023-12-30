using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.CustomerPricing.Request;

public class UpdateCustomerPricingArgs
{
    [Required]
    public int Id {get; set;}

    public decimal? Rate {get; set;}

    public bool? IsManualPayment {get; set;}

    public bool? InclusivePricing {get; set;}
}