using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.AccountService.Interactors;

public class SubmitUpdateProfileArgs : IInteractor
{
    public string? FirstName {get; set;}
    public string? LastName {get; set;}
    public DateTime? Birthdate {get; set;}
    public string? About {get; set;}
    public int? VerifiedBadge { get; set; }
    public string? PhoneNumber { get; set; }
}