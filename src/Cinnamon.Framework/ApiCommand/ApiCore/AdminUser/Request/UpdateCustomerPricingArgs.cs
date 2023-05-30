using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiCore.AdminUser.Request;

public class UpdateCustomerPricingArgs 
{
    [Required]
    public int CustomerId {get; set;}

    [Required]
    [Range(1, 100)]
    public decimal Rate {get; set;}
}