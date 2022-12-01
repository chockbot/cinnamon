namespace Cinnamon.Core.Models;

public class ResendEmailModel : BaseModel 
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string DateResend { get; set; }
    public int Count { get; set; }

    protected virtual DateTime ToDateResend()
    {
        return DateTime.Parse(DateResend);
    }
}