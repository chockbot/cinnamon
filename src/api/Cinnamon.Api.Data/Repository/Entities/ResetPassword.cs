namespace Cinnamon.Api.Data.Repository.Entities;

public class ResetPassword : BaseEntity
{
    public string Email {get; set;}
    public string Guid {get; set;}
    public string Token {get; set;}
    public bool IsUsed {get; set;}
    public string GeneratedToken {get; set;}
}