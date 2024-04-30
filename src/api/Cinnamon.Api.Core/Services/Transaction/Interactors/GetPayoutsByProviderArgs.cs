using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.TransactionService.Interactors;

public class GetPayoutsByProviderArgs : IInteractor
{
    public int Id { get; set; }
    public DateTime DateFrom { get; set; }
    public int Status { get; set; }
}
