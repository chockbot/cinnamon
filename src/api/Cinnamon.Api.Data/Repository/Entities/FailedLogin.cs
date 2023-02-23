namespace Cinnamon.Api.Data.Repository.Entities;

public class FailedLogin : BaseEntity
{
    public string Email {get; set;}
    public string Metadata {get; set;}
    public DateTime LoginDate {get; set;}
}