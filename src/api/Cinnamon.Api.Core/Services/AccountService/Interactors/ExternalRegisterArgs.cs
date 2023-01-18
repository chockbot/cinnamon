using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.AccountService.Interactors;

public class ExternalRegisterArgs : IInteractor
{
    public string FirstName {get; set;}
    public string LastName {get; set;}
    public string Email {get; set;}
    public string Password {get; set;}
    public DateTime Birthdate {get; set;}
    public string Guid {get; set;}
    public string Token {get; set;}
    public string ProfilePath {get; set;}
}