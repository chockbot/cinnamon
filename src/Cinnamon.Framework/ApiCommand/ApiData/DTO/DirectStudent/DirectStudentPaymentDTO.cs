namespace Cinnamon.Framework.ApiCommand.ApiData.DTO.DirectStudent;

public class DirectStudentPaymentDTO 
{
    public int Id {get; set;}
    public int DirectStudentSessionId {get; set;}
    public decimal Amount {get; set;}
    public DateTime CreatedOn { get; set;}
}