using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.Subcategory.Request;

public class UpdateSubcategoryArgs
{
    [Required]
    public int SubcategoryId { get; set; }
    [Required]
    public int CategoryId { get; set; }
    public string SubcategoryName { get; set; } 
}