namespace Cinnamon.Framework.ApiCommand.ApiCore.DTO.DynamicContent;

public class DynamicEmailTemplateDTO
{
    public int Id {get; set;}
    public int ActivityId {get; set;}
    public int ProviderId {get; set;}
    public string Subject {get; set;}
    public string Body {get; set;}
}