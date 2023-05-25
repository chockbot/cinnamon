using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.ExternalLoginToken.Request;

public class CreateExterLoginTokenArgs
{
    [Required]
    public string Token {get; set;}
    [Required]
    public string Guid {get; set;}
    [EmailAddress]
    public string? Email {get; set;}
    public string? FirstName {get; set;}
    public string? LastName {get; set;}
    [Required]
    public DateTime DateGenerated {get; set;}
    public bool IsEmptyUsername {get; set;} = false;
}