namespace Cinnamon.Api.Data.Repository.Entities
{
    public class AdminUser: BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
    }
}
