namespace Cinnamon.Api.Core.Services.AccountService.Interactors.Results;

public class ExternalRegisterResult 
{
    public int Id {get; set;}
    public string FirstName {get; set;}
    public string LastName {get; set;}
    public string Email {get; set;}
    public DateTime Birthdate {get; set;}
    public string ProfileImg {get; set;}
    public bool ExternalLogin {get; set;}
    public string Handler {get; set;}
}