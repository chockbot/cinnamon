using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiCore.Account.Request;

public class UploadProfilePictureArgs
{
    [Required]
    public IFormFile ProfileImage { get; set; }
}
