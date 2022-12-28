using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.FamilyMember.Request;

public class UpdateFamilyMemberArgs 
{
    [Required]
    public int FamilyMemberId {get; set;}
    public string? Name {get; set;}
    public string? Gender {get; set;}
    public string? BirthMonth {get; set;}
    public int? BirthYear {get; set;}
}