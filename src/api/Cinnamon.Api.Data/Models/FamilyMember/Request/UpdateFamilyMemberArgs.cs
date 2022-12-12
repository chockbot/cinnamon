using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Api.Data.Models.FamilyMember.Request;

public class UpdateFamilyMemberArgs 
{
    [Required]
    public int FamilyMemberId {get; set;}
    public string? Name {get; set;}
    public string? Gender {get; set;}
    public string? BirthMonth {get; set;}
    public string? BirthYear {get; set;}
}