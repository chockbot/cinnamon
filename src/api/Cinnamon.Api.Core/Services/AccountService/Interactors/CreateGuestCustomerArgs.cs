using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.AccountService.Interactors;

public class CreateGuestCustomerArgs : IInteractor
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string Email { get; set; }
    public DateTime? Birthdate { get; set; }
    public string? PhoneNumber { get; set; }
    public string? About { get; set; }
    public string? ProfilePath { get; set; }
    public string? Handler { get; set; }
    public bool? HasAcceptedTerms { get; set; }
}