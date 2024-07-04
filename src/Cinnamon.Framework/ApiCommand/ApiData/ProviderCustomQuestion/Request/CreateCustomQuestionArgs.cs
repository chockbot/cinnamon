using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.ProviderCustomQuestion.Request;

public class CreateCustomQuestionArgs 
{
    [Required]
    public int ActivityId {get; set;}

    [Required]
    public int ProviderId {get; set;}

    [Required]
    public string FieldLabel {get; set;}

    [Required]
    public string FieldType {get; set;}

    [Required]
    public bool Required {get; set;}
}