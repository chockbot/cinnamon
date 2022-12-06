using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Repository.Interfaces;

namespace Cinnamon.Api.Data.Repository.DbSets;

public class ActivityEntity : GenericEntity<Activity>, IActivity
{
    public ActivityEntity(ApplicationContext applicationContext)
        : base(applicationContext)
    {
    }
}