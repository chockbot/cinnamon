using Cinnamon.Framework.ApiCommand.ApiCore.DTO.Customer;

namespace Cinnamon.Framework.ApiCommand.ApiCore.DTO.Activity;

public class TopBookedCustomersDTO 
{
    public IEnumerable<BasicProfileDTO> TopBooked {get; set;}
    public int TotalBooked {get; set;}
}