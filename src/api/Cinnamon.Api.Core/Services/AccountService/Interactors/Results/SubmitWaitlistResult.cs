namespace Cinnamon.Api.Core.Services.AccountService.Interactors.Results;

public class SubmitWaitlistResult 
{
    public int Id {get; set;}
    public string Email {get; set;}
    public string Guid {get; set;}
    public string Token {get; set;}
    public string VerificationLink {get; set;}
}