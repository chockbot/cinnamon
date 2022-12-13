using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.ExperienceCategory.Request;

public class UpdateCategoryArgs
{
    [Required]
    public int Id { get; set; }
    public string Category { get; set; }
    public string IconPath { get; set; }
}