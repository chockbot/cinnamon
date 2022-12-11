using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Api.Data.Models.ExperienceCategory.Request;

public class CreateCategoryArgs
{
    public string Category { get; set; }
    public string IconPath { get; set; }
}