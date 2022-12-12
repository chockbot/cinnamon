using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Api.Data.Models.ActivityImage.Request;

public class UpdateActivityImageArgs
{
    [Required]
    public int ActivityImageId {get; set;}
    public string? ImageName {get; set;}
    public string? ImagePath {get; set;}
}