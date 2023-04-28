namespace Cinnamon.Api.Core.Services.AccountService.Interactors.Results;

public class ExternalLoginResult 
{
    public int Id {get; set;}
    public string FirstName {get; set;}
    public string LastName {get; set;}
    public string Email {get; set;}
    public string PhoneNumber { get; set; }
    public bool ExternalLogin {get; set;}
    public bool IsMaker {get; set;}
    public string GeneratedToken {get; set;}

    // for new external logins
    public bool IsNew {get; set;}
    public string GeneratedNewToken {get; set;}
    public string GeneratedNewGuid {get; set;}
}