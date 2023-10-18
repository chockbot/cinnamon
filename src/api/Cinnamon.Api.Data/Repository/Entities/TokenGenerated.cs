namespace Cinnamon.Api.Data.Repository.Entities;

public class TokenGenerated : BaseEntity
{
    public string TokenType {get; set;}
    public string Guid {get; set;}
    public string Token {get; set;}
    public string Payload {get; set;}
}