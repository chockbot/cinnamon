namespace Cinnamon.Framework.ApiCommand.ApiData.DTO.CustomerPricing;

public class CustomerPricingDTO 
{
    public int Id {get; set;}
    public string FirstName {get; set;}
    public string LastName {get; set;}
    public string Email {get; set;}
    public decimal Rate {get; set;}
    public bool IsManualPayment {get; set;}
}