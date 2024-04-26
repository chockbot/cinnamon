namespace Cinnamon.Framework.ApiCommand.ApiCore.DTO.Activity;

public class OteAlreadyBookedDTO
{
    public int ActivityId {get; set;}
    public int OteDateId {get; set;}
    public DateTime Date {get; set;}
    public DateTime DateStart {get; set;}
    public DateTime DateEnd {get; set;}
}