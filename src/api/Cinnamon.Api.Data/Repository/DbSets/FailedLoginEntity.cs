using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Repository.Interfaces;

namespace Cinnamon.Api.Data.Repository.DbSets;

public class FailedLoginEntity : GenericEntity<FailedLogin>, IFailedLogin
{
    public FailedLoginEntity(ApplicationContext applicationContext)
        : base(applicationContext)
    {
    }
}