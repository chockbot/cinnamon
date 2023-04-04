namespace Cinnamon.Api.Core.Services.AccountService.Interactors.Results;

public class GetProfileResult 
{
    public int Id {get; set;}
    public string FirstName {get; set;}
    public string LastName {get; set;}
    public DateTime Birthdate {get; set;}
    public DateTime DateJoined { get; set; }
    public string Email {get; set;}
    public string About {get; set;}
    public bool IsMaker {get; set;}
    public int IsVerified {get; set;}
    public string ProfileImagePath {get; set;}
    public string Handler {get; set;}
}