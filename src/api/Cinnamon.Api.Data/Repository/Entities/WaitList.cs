namespace Cinnamon.Api.Data.Repository.Entities;

public class WaitList : BaseEntity 
{
    public string Email {get; set;}
    public string Guid {get; set;}
    public string Token {get; set;}
    public bool IsVerified {get; set;}
}