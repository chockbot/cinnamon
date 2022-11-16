using Microsoft.AspNetCore.Identity;

namespace Cinnamon.Core.Models;

public class CustomerModel : IdentityUser
{
    public string UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public bool IsMaker { get; set; }
    public DateTime Birthdate { get; set; }
    public bool AcceptFlag { get; set; }
}