using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Cinnamon.Framework.ApiCommand.ApiCore.Account.Request;

public class UploadGovernmentIdsArgs 
{
    [Required]
    public IFormFile FrontImageId {get; set;}
    [Required]
    public IFormFile BackImageId {get; set;}
}