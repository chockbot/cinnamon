using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Repository.Interfaces;

namespace Cinnamon.Api.Data.Repository.DbSets
{
    public class AdminUserEntity : GenericEntity<AdminUser>, IAdminUser
    {
        public AdminUserEntity(ApplicationContext applicationContext):base(applicationContext)
        {

        }
    }
}
