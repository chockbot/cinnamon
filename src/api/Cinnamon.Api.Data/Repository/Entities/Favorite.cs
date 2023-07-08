namespace Cinnamon.Api.Data.Repository.Entities
{
    public class Favorite : BaseEntity
    {
        public int CustomerId { get; set; }
        public int ActivityId { get; set; }
    }
}
