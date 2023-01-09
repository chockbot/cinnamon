using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.AccountService.Interactors;
public class GetWaitListByGuidArgs: IInteractor
{
    public string Guid { get; set; }
}
