using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.AdminService.Interactors;

public class UpdateCustomerPricingArgs : IInteractor
{
    public int CustomerId {get; set;}
    public decimal Rate {get; set;}
    public bool IsManualPayment {get; set;}
    public bool InclusivePricing {get; set;}
}