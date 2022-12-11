namespace Cinnamon.Api.Data.Services.Repository.Waitlist.DTO;

public class WaitListDTO
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string Guid { get; set; }
    public string Token { get; set; }
    public bool IsVerified { get; set; }
}