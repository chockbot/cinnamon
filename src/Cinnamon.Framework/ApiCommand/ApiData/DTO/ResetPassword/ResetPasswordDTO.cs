namespace Cinnamon.Framework.ApiCommand.ApiData.DTO.ResetPassword;

public class ResetPasswordDTO
{
    public int Id { get; set; }
    public string Email { get; set; }
    public bool IsUsed {get; set;}
}