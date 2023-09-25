namespace Cinnamon.Web.Models.Entities;

public class FamilyMember 
{
    public int Id { get; set; }
    public string Name {get; set;}
    public int BirthYear {get; set;}
    public string BirthMonth {get; set;}
    public string Gender {get; set;}

    public bool IsNew { get; set; } = false;

    // extra properties
    public bool IsSelected {get; set;}
}