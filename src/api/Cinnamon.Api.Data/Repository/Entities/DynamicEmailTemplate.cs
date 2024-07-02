namespace Cinnamon.Api.Data.Repository.Entities;

public class DynamicEmailTemplate : BaseEntity
{
    public int ActivityId {get; set;}
    public int ProviderId {get; set;}
    public string TemplateType {get; set;}
    public string Subject {get; set;}
    public string Body {get; set;}
}