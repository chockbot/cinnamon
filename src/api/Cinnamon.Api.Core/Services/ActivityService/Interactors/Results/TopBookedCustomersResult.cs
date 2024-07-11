namespace Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;

public class TopBookedCustomersResult 
{
    public IEnumerable<BookedCustomer> TopBooked {get; set;}
    public int TotalBooked {get; set;}

    public class BookedCustomer 
    {
        public string FirstName {get; set;}
        public string LastName {get; set;}
        public string Email {get; set;}
        public string ProfileImage {get; set;}
    }
}