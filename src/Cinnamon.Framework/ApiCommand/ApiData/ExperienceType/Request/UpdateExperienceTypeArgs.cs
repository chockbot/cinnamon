using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.ExperienceType.Request;
public class UpdateExperienceTypeArgs
{
    [Required]
    public int Id { get; set; }
    public string Name { get; set; }
}
