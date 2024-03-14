namespace Cinnamon.Api.Data.Repository.Entities;

public class OteSharedLink : BaseEntity 
{
    public int ActivityId {get; set;}
    public int OteDateId {get; set;}
    public string Guid {get; set;}
    public string Token {get; set;}
}