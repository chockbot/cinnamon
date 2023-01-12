using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.AccountService.Interactors;

public class DeleteFamilyMembersArgs : IInteractor
{
    public IEnumerable<int> Ids {get; set;}
}