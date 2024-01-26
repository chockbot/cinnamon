namespace Cinnamon.Api.Data.Repository.Entities;

public class OteDateOverride : BaseEntity 
{
    public int OteDateId {get; set;}
    public DateTime Date {get; set;}
    public DateTime DateStart {get; set;}
    public DateTime DateEnd {get; set;}

    public virtual OteDate OteDate {get; set;}
}