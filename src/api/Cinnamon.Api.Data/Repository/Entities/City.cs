namespace Cinnamon.Api.Data.Repository.Entities
{
    public class City: BaseEntity
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string RegionCode { get; set; }
    }
}
