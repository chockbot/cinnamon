using Cinnamon.Core.Interactor;

namespace Cinnamon.Core.Module.CustomerService.Interactors;

public class UpdateLegalName : IInteractor 
{
    public int CustomerId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}