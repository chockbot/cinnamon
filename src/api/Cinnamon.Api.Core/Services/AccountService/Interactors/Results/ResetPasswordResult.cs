namespace Cinnamon.Api.Core.Services.AccountService.Interactors.Results;

public class ResetPasswordResult 
{
    public int Id {get; set;}
    public string Email {get; set;}
    public string VerificationLink {get; set;}
}