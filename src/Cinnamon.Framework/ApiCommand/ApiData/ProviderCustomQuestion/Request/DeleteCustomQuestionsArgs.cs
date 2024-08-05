using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.ProviderCustomQuestion.Request;

public class DeleteCustomQuestionsArgs 
{
    [Required]
    public IEnumerable<int> Ids { get; set; }
}