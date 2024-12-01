namespace Cinnamon.Framework.ApiCommand.ApiData.DTO.Customer;

public class CustomerDTO
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime Birthdate { get; set; }
    public string? About { get; set; }
    public string? ProfileImg { get; set; }
    public bool IsMaker { get; set; }
    public int IsVerified { get; set; }
    public DateTime IsVerifiedObtainedDate { get; set; }
    public bool IsOG { get; set; }
    public DateTime IsOGObtainedDate { get; set; }
    public bool IsOfficial { get; set; }
    public DateTime IsOfficialObtainedDate { get; set; }
    public bool ExternalLogin { get; set; }
    public DateTime DateJoined { get; set; }
    public string Handler {get; set;}
    public string FrontIdImagePath {get; set; }
    public string BackIdImagePath { get; set; }
    public decimal TotalCredits {get; set;}
    public Pricing CustomerPricing {get; set;}
    public string? ConnectionId { get; set; }

    public class Pricing 
    {
        public decimal Rate {get; set;}
        public bool IsManualPayment {get; set;}
        public bool InclusivePricing {get; set;}
    }
    public bool IsAccountBan { get; set; }
    public bool IsGuest { get; set; }
}