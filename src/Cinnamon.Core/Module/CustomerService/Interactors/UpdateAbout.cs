using Cinnamon.Core.Interactor;

namespace Cinnamon.Core.Module.CustomerService.Interactors;

public class UpdateAbout : IInteractor 
{
    public int CustomerId { get; set; }
    public string About { get; set; }
}