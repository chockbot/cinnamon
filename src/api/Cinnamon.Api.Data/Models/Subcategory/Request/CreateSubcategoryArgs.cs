using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Api.Data.Models.Subcategory.Request;

public class CreateSubcategoryArgs
{
    [Required]
    public int SubcategoryId { get; set; }
    [Required]
    public int CategoryId { get; set; }
    [Required]
    public string SubcategoryName { get; set; } 
}