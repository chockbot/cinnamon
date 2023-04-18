namespace Cinnamon.Api.Core.Services.ActivityService.Interactors.Results
{
    public class GetAllRegionsResult
    {
        public IEnumerable<Region> Regions { get; set; }

        public class Region
        {
            public string Code { get; set; }
            public string Name { get; set; }
            public string RegionName { get; set; }
        }
    }
}
