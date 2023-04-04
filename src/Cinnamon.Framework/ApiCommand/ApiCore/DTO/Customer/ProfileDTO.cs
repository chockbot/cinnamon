namespace Cinnamon.Framework.ApiCommand.ApiCore.DTO.Customer;

public class ProfileDTO
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public DateTime Birthdate { get; set; }
    public string About { get; set; }
    public string ProfileImg { get; set; }
    public bool IsMaker { get; set; }
    public int IsVerified { get; set; }
    public string Handler {get; set;}
    public decimal TotalCredits {get; set;}
}