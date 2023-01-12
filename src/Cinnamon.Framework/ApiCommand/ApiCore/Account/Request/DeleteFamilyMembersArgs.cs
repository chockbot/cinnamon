using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiCore.Account.Request;

public class DeleteFamilyMembersArgs 
{
    [Required]
    public IEnumerable<int> Ids {get; set;}
}