using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Cinnamon.Framework.ApiCommand.ApiCore.Account.Request;

public class UploadGovernmentIdsArgs 
{
    public IFormFile? FrontImageId {get; set;}
    public IFormFile? BackImageId {get; set;}
}