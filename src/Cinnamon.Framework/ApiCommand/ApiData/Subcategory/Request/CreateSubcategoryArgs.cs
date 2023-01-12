using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.Subcategory.Request;

public class CreateSubcategoryArgs
{
    [Required]
    public int SubcategoryId { get; set; }
    [Required]
    public int CategoryId { get; set; }
    [Required]
    public string SubcategoryName { get; set; } 
}