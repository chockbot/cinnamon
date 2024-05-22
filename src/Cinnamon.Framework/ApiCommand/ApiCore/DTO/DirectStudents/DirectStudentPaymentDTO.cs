namespace Cinnamon.Framework.ApiCommand.ApiCore.DTO.DirectStudents;

public class DirectStudentPaymentDTO 
{
    public int Id {get; set;}
    public int DirectStudentSessionId {get; set;}
    public decimal Amount {get; set;}
    public DateTime CreatedOn { get; set; }
}