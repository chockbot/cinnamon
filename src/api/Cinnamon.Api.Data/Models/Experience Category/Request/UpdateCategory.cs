using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Api.Data.Models.Activity.Request;

public class UpdateCategory
{
    [Required]
    public int Id { get; set; }
    public string Category { get; set; }
    public string IconPath { get; set; }
}