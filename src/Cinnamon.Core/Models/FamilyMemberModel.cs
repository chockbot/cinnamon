namespace Cinnamon.Core.Models;

public class FamilyMemberModel : BaseModel
{
    public int Id { get; set;}
    public int CustomerId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Gender { get; set; }
    public string BirthMonth { get; set; }
    public string BirthYear { get; set; }

    public virtual CustomerModel Customer { get; set; }
}