using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Api.Data.Models.ActivityImage.Request;

public class CreateActivityImageArgs
{
    [Required]
    public int ActivityId {get; set;}
    [Required]
    public string ImageName {get; set;}
    [Required]
    public string ImagePath {get; set;}
}