using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.TokenGenerated.Request;

public class CreateTokenArgs
{
    [Required]
    public string TokenType {get; set;}

    [Required]
    public string Token {get; set;}

    [Required]
    public string Guid {get; set;}

    [Required]
    public string Payload {get; set;}
}