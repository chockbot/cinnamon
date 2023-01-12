namespace Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;

public class GetAddressResult
{
    public IEnumerable<Address> Addresses { get; set; }

    public class Address
    {
        public int Id { get; set; }
        public int ActivityId { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string District { get; set; }
        public string City { get; set; }
    }
}
