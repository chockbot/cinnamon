namespace Cinnamon.Api.Core.Services.AccountService.Interactors.Results;

public class UpdateCustomerProfileResult
{
    public int Id {get; set;}
    public string FirstName {get; set;}
    public string LastName {get; set;}
    public int VerifiedBadge {get; set;}
    public bool IsOG { get; set; }
    public bool IsOF { get; set; }
}