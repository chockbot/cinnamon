using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.TransactionService.Interactors;

public class GetGrossSalesByProviderArgs : IInteractor
{
    public int Id { get; set; }
    public DateTime DateFrom { get; set; }
}
