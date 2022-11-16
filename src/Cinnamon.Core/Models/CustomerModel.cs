namespace Cinnamon.Core.Models;

public class CustomerModel : BaseModel
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public bool IsMaker { get; set; }
    public DateTime Birthdate { get; set; }
    public bool AcceptFlag { get; set; }
}