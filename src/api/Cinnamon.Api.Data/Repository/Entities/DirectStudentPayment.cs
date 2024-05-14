namespace Cinnamon.Api.Data.Repository.Entities;

public class DirectStudentPayment : BaseEntity
{
    public int DirectStudentSessionId {get; set;}
    public decimal Amount {get; set;}

    public virtual DirectStudentSession DirectStudentSession {get; set;}
}