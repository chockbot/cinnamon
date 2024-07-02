namespace Cinnamon.Framework.ApiCommand.ApiData.DTO.DynamicContent;

public class DynamicEmailTemplateDTO
{
    public int Id {get; set;}
    public int ActivityId {get; set;}
    public int ProviderId {get; set;}
    public string TemplateType {get; set;}
    public string Subject {get; set;}
    public string Body {get; set;}
}