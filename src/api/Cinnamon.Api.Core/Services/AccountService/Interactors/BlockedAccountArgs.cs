using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.AccountService.Interactors;

public class BlockedAccountArgs : IInteractor
{
    public int Id {get; set;}
    public bool IsBlock { get; set; }
}