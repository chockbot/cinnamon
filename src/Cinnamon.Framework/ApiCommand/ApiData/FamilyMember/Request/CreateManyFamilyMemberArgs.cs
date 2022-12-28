using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.FamilyMember.Request;

public class CreateManyFamilyMemberArgs 
{
    [Required]
    public int CustomerId {get; set;}
    [Required]
    public IEnumerable<CreateFamilyMember> FamilyMembers {get; set;}

    public class CreateFamilyMember 
    {
        [Required]
        public string Name {get; set;}
        [Required]
        public string Gender {get; set;}
        [Required]
        public string BirthMonth {get; set;}
        [Required]
        public int BirthYear {get; set;}
    }
}