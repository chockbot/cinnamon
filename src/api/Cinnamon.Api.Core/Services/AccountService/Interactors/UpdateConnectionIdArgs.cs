using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.AccountService.Interactors;

public class UpdateConnectionIdArgs : IInteractor
{
    public int CustomerId { get; set; }
    public string? ConnectionId { get; set; }
}