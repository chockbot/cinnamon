using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.BadgeList.Request;

public class UpdateBadgeArgs
{
    [Required]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int NumberOfStudent { get; set; }
    public int NumberOfCompleted { get; set; }
    public int NumberOfReviews { get; set; }
    public string ImgScr { get; set; }
}
