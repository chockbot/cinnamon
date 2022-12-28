using System.ComponentModel.DataAnnotations.Schema;

namespace Cinnamon.Api.Data.Repository.Entities;

public class FamilyMember : BaseEntity 
{
    public int CustomerId {get; set;}
    public string Name {get; set;}
    public string Gender {get; set;}
    public string BirthMonth {get; set;}
    public int BirthYear {get; set;}

    public virtual Customer Customer {get; set;}
}