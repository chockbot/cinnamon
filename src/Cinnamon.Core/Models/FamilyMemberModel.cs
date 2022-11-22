namespace Cinnamon.Core.Models;

public class FamilyMemberModel : BaseModel
{
    public int Id { get; set;}
    public int CustomerId { get; set; }
    public string Name { get; set; }
    public string Gender { get; set; }
    public string BirthMonth { get; set; }
    public int BirthYear { get; set; }

    public virtual CustomerModel Customer { get; set; }
}