using Cinnamon.Framework.ApiCommand.ApiData.DTO.ActivityImage;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.Customer;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.OteSchedule;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.OteTicket;

namespace Cinnamon.Framework.ApiCommand.ApiData.DTO.Activity;

public class OteActivityDTO 
{
    public int Id {get; set;}
    public string EventName {get; set;}
    public string Description {get; set;}
    public int ExperienceTypeId {get; set;}
    public int CategoryId {get; set;}
    public string Price {get; set;}
    public string HouseNo {get; set;}
    public string BarangayCode {get; set;}
    public string BarangayName {get; set;}
    public string CityNumber {get; set;}
    public string CityName {get; set;}
    public string RegionCode {get; set;}
    public string RegionName {get; set;}
    public string PostalCode {get; set;}
    public string PinnedLocation {get; set;}
    public bool IsPublished {get; set;}
    public string Handler {get; set;}
    public DateTime ScheduleFrom {get; set;}
    public DateTime ScheduleTo {get; set;}
    public string Recurrences {get; set;}
    public int ProviderId {get; set;}
    public bool IsComingSoon {get; set;}
    public string EventImage { get; set; }
    public int Slots { get; set; }
    public int Sold { get; set; }
    public int Available { get; set; }
    public bool ForceDisable {get; set;}

    public IEnumerable<OteSchedulePricingDTO> Pricings {get; set;}
    public IEnumerable<ActivityImageDTO> Images {get; set;}
    public CustomerDTO Owner {get; set;}
    public IEnumerable<OteScheduleDateDTO> OteDates {get; set;}
    public IEnumerable<OteTicketDTO> Tickets { get; set; }
    public IEnumerable<OteOnlineEventsDTO> OteOnlineEvent { get; set;}
    public OteScheduleDTO OteSchedule {get; set;}
}