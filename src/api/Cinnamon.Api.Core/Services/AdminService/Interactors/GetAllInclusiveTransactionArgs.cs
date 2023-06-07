using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.AdminService.Interactors;

public class GetAllInclusiveTransactionArgs : IInteractor
{
    public string? Name {get; set;}

    // date format must be yyyyMMddHHmmss
    public string? PurchaseDateFrom {get; set;}

    // date format must be yyyyMMddHHmmss
    public string? PurchaseDateTo {get; set;}

    public string? Email {get; set;}

    public int? Status {get; set;}
}