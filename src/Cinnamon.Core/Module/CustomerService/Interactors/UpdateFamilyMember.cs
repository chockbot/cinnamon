using Cinnamon.Core.Models;
using Cinnamon.Core.Interactor;

namespace Cinnamon.Core.Module.CustomerService.Interactors;

public class UpdateFamilyMember : IInteractor
{
    public int CustomerId { get; set; }
    public IList<FamilyMemberModel> FamilyMembers { get; set; }
}