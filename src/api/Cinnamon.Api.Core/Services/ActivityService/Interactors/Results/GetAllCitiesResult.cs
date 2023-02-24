namespace Cinnamon.Api.Core.Services.ActivityService.Interactors.Results
{
    public class GetAllCitiesResult
    {
        public IEnumerable<City> Cities { get; set; }

        public class City
        {
            public string Code { get; set; }
            public string Name { get; set; }
            public string RegionCode { get; set; }
        }
    }
}
