namespace Cinnamon.Api.Core.Services.AccountService.Interactors.Results;

public class GetCustomerByHandlerResult 
{
    public int Id {get; set;}
    public string FirstName {get; set;}
    public string LastName {get; set;}
    public bool IsMaker {get; set;}
    public int IsVerified { get; set; }
    public bool IsOG { get; set; }
    public bool IsOfficial { get; set; }
    public string ProfileImg {get; set;}
    public string About {get; set;}
    public DateTime DateJoined { get; set; }
    public string Email { get; set; }
}