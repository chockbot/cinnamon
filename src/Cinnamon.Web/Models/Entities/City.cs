namespace Cinnamon.Web.Models.Entities
{
    public class City
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string RegionCode { get; set; }
        public bool IsCity { get; set; }
        public bool IsMunicipality { get; set; }
    }
}
