namespace Cinnamon.Api.Data.Repository.Entities;

public class ProviderCustomQuestion : BaseEntity 
{
    public int ActivityId {get; set;}
    public int ProviderId {get; set;}
    public string FieldLabel {get; set;}
    public string FieldType {get; set;}   
    public bool Required {get; set;}
}