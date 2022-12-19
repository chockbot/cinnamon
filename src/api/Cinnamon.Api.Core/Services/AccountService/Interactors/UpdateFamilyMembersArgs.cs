using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.AccountService.Interactors;

public class UpdateFamilyMembersArgs : IInteractor
{
    public IEnumerable<FamilyMember> FamilyMembers {get; set;}

    public class FamilyMember 
    {
        public int Id {get; set;}
        public string Name {get; set;}
        public string BirthMonth {get; set;}
        public string BirthYear {get; set;}
        public string Gender {get; set;}
    }
}