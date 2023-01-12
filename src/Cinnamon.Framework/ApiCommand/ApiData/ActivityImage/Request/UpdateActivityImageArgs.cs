using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.ActivityImage.Request;

public class UpdateActivityImageArgs
{
    [Required]
    public int ActivityImageId {get; set;}
    public string? ImageName {get; set;}
    public string? ImagePath {get; set;}
}