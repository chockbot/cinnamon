namespace Cinnamon.Api.Core.Modules.DataAccess.Interactors.CustomerData;

public class Customer 
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public DateTime Birthdate { get; set; }
    public string? About { get; set; }
    public string? ProfileImg { get; set; }
    public bool IsMaker { get; set; }
    public bool IsVerified { get; set; }
    public bool ExternalLogin { get; set; }
    public DateTime DateJoined { get; set; }
}