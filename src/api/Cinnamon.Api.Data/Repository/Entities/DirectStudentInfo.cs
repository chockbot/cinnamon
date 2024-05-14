namespace Cinnamon.Api.Data.Repository.Entities;

public class DirectStudentInfo : BaseEntity 
{
    public int ProviderId {get; set;}
    public string Name {get; set;}
    public string Gender {get; set;}
    public string BirthMonth {get; set;}
    public int BirthYear {get; set;}
}