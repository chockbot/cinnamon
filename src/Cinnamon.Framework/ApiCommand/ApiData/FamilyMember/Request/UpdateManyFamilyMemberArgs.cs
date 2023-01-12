using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.FamilyMember.Request;

public class UpdateManyFamilyMemberArgs 
{
    [Required]
    public IEnumerable<UpdateFamilyMember> FamilyMembers {get; set;}

    public class UpdateFamilyMember 
    {
        [Required]
        public int Id {get; set;}
        [Required]
        public int CustomerId {get; set;}
        public string? Name {get; set;}
        public string? Gender {get; set;}
        public string? BirthMonth {get; set;}
        public int? BirthYear {get; set;}
    }
}