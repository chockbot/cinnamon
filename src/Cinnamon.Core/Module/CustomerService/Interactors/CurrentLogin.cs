using System.Security.Claims;
using Cinnamon.Core.Interactor;

namespace Cinnamon.Core.Module.CustomerService.Interactors;

public class CurrentLogin : IInteractor 
{
    public ClaimsPrincipal User { get; set; }
}