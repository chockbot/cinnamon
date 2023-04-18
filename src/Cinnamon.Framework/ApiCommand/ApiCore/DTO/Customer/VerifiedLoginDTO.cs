namespace Cinnamon.Framework.ApiCommand.ApiCore.DTO.Customer;

public class VerifiedLoginDTO
{
    public int Id {get; set;}
    public string FirstName {get; set;}
    public string LastName {get; set;}
    public string Email {get; set;}
    public bool ExternalLogin {get; set;}
    public bool IsMaker {get; set;}
    public string GeneratedToken {get; set;}

    // for new external logins
    public bool IsNew {get; set;}
    public string GeneratedNewToken {get; set;}
    public string GeneratedNewUid {get; set;}
}