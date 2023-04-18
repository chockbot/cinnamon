using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.ExternalLoginToken.Request;

public class UpdateExternalLoginTokenArgs
{
    [Required]
    public int Id {get; set;}
    [Required]
    public bool IsUsed {get; set;}
}