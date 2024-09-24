namespace Cinnamon.Framework.ApiCommand.ApiData.DTO.ProviderCustomQuestion;

public class ProviderCustomQuestionDTO
{
    public int Id {get; set;}
    public int ActivityId {get; set;}
    public int ProviderId {get; set;}
    public string FieldLabel {get; set;}
    public string FieldType {get; set;}   
    public bool Required {get; set;}
    public string Options { get; set; }
}