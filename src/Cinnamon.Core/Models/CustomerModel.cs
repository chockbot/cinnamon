using Microsoft.AspNetCore.Identity;

namespace Cinnamon.Core.Models;

public class CustomerModel : BaseModel
{
    public int Id { get; set;}
    public string UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public bool IsMaker { get; set; }
    public string Birthdate { get; set; }
    public bool AcceptFlag { get; set; }
    public bool ExternalLogin { get; set; }
    public string ProfilePath { get; set; }
    public bool IsVerified { get; set; }
    public string DateJoined { get; set; }
    public string About { get; set; }
}