namespace Cinnamon.Framework.ApiCommand.ApiData.DynamicContent.Request;

public class GetEmailTemplatesArgs
{
    public int? ActivityId {get; set;}

    public int? ProviderId {get; set;}

    public string? TemplateType {get; set;}
}