using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.ActivityService.Interactors;

public class OteCreateArgs : IInteractor
{
    public OteActivity Activity {get; set;}

    public IEnumerable<OtePricing> Pricings {get; set;}

    public class OteActivity 
    {
        public string EventName {get; set;}

        public string Description {get; set;}

        public int ExperienceTypeId {get; set;}

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

        public int ExperienceCreationTypeId {get; set;}

        public DateTime ScheduleFrom {get; set;}

        public DateTime ScheduleTo {get; set;}

        public string Recurrence {get; set;}
    }

    public class OtePricing 
    {
        public string Description {get; set;}

        public bool IsAbsorbFees {get; set;}

        public int MaxSlots {get; set;}

        public decimal Price {get; set;}
    }
}