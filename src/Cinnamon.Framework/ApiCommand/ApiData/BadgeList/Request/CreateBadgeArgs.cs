using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.BadgeList.Request;

public class CreateBadgeArgs
{
    [Required]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    public int NumberOfStudent { get; set; }
    [Required]
    public int NumberOfCompleted { get; set; }
    [Required]
    public int NumberOfReviews { get; set; }
    [Required]
    public string ImgScr { get; set; }
}
