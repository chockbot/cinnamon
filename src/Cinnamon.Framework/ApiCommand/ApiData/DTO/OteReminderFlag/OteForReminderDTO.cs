namespace Cinnamon.Framework.ApiCommand.ApiData.DTO.OteReminderFlag;

public class OteForReminderDTO
{
    public int DateId {get; set;}
    public int ActivityId {get; set;}
    public DateTime DateStart {get; set;}
    public string Title {get; set;}
    public string Description {get; set;}
    public string ProviderFirstName {get; set;}
    public string ProviderLastName {get; set;}
    public string ProviderEmail {get; set;}
}