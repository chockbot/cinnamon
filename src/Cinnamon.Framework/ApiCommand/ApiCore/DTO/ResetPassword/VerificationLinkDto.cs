namespace Cinnamon.Framework.ApiCommand.ApiCore.DTO.ResetPassword;

public class ResetVerificationLinkDto
{
    public int Id {get; set;}
    public string VerificationLink {get; set;}
    public string Email {get; set;}
}