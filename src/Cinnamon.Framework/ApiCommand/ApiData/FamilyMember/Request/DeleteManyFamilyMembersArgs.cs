using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.FamilyMember.Request;

public class DeleteManyFamilyMembersArgs 
{
    [Required]
    public IEnumerable<int> Ids {get; set;}
}