namespace Cinnamon.Core.Models;

public class ResendEmailModel : BaseModel 
{
    public int Id { get; set; }
    public string Email { get; set; }
    public DateTime DateResend { get; set; }
    public int Count { get; set; }
}