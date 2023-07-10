namespace Cinnamon.Api.Core.Services.AccountService.Interactors.Results;

public class SubmitUpdateProfileResult 
{
    public int Id {get; set;}
    public string FirstName {get; set;}
    public string LastName {get; set;}
    public string PhoneNumber { get; set; }
    public DateTime Birthdate {get; set;}
    public string About {get; set;}
}