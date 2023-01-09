using System.ComponentModel.DataAnnotations;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.ActivityImage;

namespace Cinnamon.Framework.ApiCommand.ApiData.ActivityImage.Request;

public class UpdateManyActivityImageArgs
{
    [Required]
    public IEnumerable<UpdateImage> Images {get; set;}

    public class UpdateImage 
    {
        [Required]
        public int Id {get; set;}
        [Required]
        public int ActivityId {get; set;}
        [Required]
        public string ImageSrc {get; set;}
        [Required]
        public string ImageName {get; set;}
    }
}