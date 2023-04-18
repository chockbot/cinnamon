using System.ComponentModel.DataAnnotations;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.ActivityImage;

namespace Cinnamon.Framework.ApiCommand.ApiData.ActivityImage.Request;

public class CreateManyActivityImageArgs
{
    [Required]
    public IEnumerable<CreateImage> Images {get; set;}

    public class CreateImage 
    {
        [Required]
        public int Order {get; set;}
        [Required]
        public int ActivityId {get; set;}
        [Required]
        public string ImageSrc {get; set;}
        [Required]
        public string ImageName {get; set;}
    }
}