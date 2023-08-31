namespace Cinnamon.Api.Core.Services.AccountService.Interactors.Results;

public class ExtraLoginResult 
{
    public int Id {get; set;}
    public string FirstName {get; set;}
    public string LastName {get; set;}
    public string Email {get; set;}
    public bool ExternalLogin {get; set;}
    public bool IsMaker {get; set;}
    public string GeneratedToken {get; set;}
}