namespace Cinnamon.Framework.ApiCommand.ApiData.DTO.OteSchedule;

public class OteSharedLinkDTO
{
    public int Id {get; set;}
    public int ActivityId {get; set;}
    public int OteDateId {get; set;}
    public string Guid {get; set;}
    public string Token {get; set;}
    public bool Enable {get; set;}
}