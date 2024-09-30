namespace Cinnamon.Framework.ApiCommand.ApiCore.DTO.Activity;

public class OteDateDTO
{
    public int Id {get; set;}
    public int OteScheduleId {get; set;}
    public DateTime Date {get; set;}
    public DateTime DateStart {get; set;}
    public DateTime DateEnd {get; set;}
    public string? SeatPlanPayload { get; set; }
}
