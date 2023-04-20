using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.ExternalLoginToken.Request;

public class GetLoginTokenArgs
{
    [Required]
    public string Token {get; set;}
    [Required]
    public string Guid {get; set;}
}