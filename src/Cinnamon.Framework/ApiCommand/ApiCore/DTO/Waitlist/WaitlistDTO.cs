namespace Cinnamon.Framework.ApiCommand.ApiCore.DTO.Waitlist;

public class WaitlistDTO
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string Guid { get; set; }
    public string Token { get; set; }
    public bool IsVerified { get; set; }
}