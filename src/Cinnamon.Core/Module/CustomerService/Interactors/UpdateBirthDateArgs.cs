using Cinnamon.Core.Interactor;

namespace Cinnamon.Core.Module.CustomerService.Interactors;

public class UpdateBirthDateArgs : IInteractor 
{
    public int CustomerId { get; set; }
    public string BirthDate { get; set; }
}