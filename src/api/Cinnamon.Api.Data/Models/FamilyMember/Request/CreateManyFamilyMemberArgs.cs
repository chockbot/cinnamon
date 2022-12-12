using System.ComponentModel.DataAnnotations;
using Cinnamon.Api.Data.Services.Repository.FamilyMember.DTO;

namespace Cinnamon.Api.Data.Models.FamilyMember.Request;

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
        public string BirthYear {get; set;}
    }
}