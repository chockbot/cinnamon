namespace Cinnamon.Api.Core.Services.AccountService.Interactors.Results;

public class GetCustomerByIdResult
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public DateTime Birthdate { get; set; }
    public string PhoneNumber { get; set; }
    public string? About { get; set; }
    public string? ProfileImg { get; set; }
    public bool IsMaker { get; set; }
    public int IsVerified { get; set; }
    public bool IsOG { get; set; }
    public bool IsOfficial { get; set; }
    public bool ExternalLogin { get; set; }
    public DateTime DateJoined { get; set; }
    public string? ConnectionId { get; set; }
    public string Handler {get; set;}
}
