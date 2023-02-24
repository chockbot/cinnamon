namespace Cinnamon.Api.Core.Services.ActivityService.Interactors.Results
{
    public class GetAllBarangaysResult
    {
        public IEnumerable<Barangay> Barangays { get; set; }

        public class Barangay
        {
            public string Code { get; set; }
            public string Name { get; set; }
            public string CityCode { get; set; }
        }
    }
}
