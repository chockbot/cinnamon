using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.AccountService.Interactors;

public class SubmitRegisterArgs : IInteractor
{
    public string FirstName {get; set;}
    public string LastName {get; set;}
    public string Email {get; set;}
    public string Password {get; set;}
    public DateTime Birthdate {get; set;}
    public string PhoneNumber { get; set; }
    public string ProfilePath {get; set;}
    public bool ExternalLogin {get; set;}
    public bool HasAcceptedTerms { get; set; }
    public bool IsGuest { get; set; }
}