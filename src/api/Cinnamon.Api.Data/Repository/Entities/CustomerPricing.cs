namespace Cinnamon.Api.Data.Repository.Entities;

public class CustomerPricing : BaseEntity
{
    public int CustomerId {get; set;}
    public string Email {get; set;}
    public decimal Rate {get; set;}
    public bool IsManualPayment {get; set;}
    public bool InclusivePricing {get; set;}

    public virtual Customer Customer {get; set;}
}