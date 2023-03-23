namespace Cinnamon.Web.Models.Entities;

public class AdminUser
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string EmailAddress { get; set; }
    public bool IsAdmin { get; set; }
}