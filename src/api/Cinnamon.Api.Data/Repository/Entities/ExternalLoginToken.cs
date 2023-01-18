namespace Cinnamon.Api.Data.Repository.Entities;

public class ExternalLoginToken : BaseEntity
{
    public string Token {get; set;}
    public string Guid {get; set;}
    public bool IsUsed {get; set;}
    public string Email {get; set;}
    public string FirstName {get; set;}
    public string LastName {get; set;}
    public DateTime DateGenerated {get; set;}
}