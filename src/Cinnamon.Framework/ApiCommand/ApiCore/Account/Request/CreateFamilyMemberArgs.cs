using Cinnamon.Framework.ApiCommand.ApiCore.DTO.FamilyMember;

namespace Cinnamon.Framework.ApiCommand.ApiCore.Account.Request;

public class CreateFamilyMemberArgs 
{
    public IEnumerable<FamilyMemberDTO> FamilyMembers {get; set;}
}