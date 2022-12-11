using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Api.Data.Models.ExperienceCategory.Request;

public class CreateCategoryArgs
{
    [Required]
    public int CategoryId { get; set; }
    [Required]
    public string Category { get; set; }
    [Required]
    public string IconPath { get; set; }
}