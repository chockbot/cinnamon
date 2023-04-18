using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.ResetPassword.Request;

public class UpdateResetPasswordArgs
{
    [Required]
    public int Id {get; set;}
    [Required]
    public bool IsUsed {get; set;}
}