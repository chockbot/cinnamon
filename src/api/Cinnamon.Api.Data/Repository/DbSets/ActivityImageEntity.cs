using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Repository.Interfaces;

namespace Cinnamon.Api.Data.Repository.DbSets;

public class ActivityImageEntity : GenericEntity<ActivityImage>, IActivityImage
{
    public ActivityImageEntity(ApplicationContext applicationContext)
        : base(applicationContext)
    {
    }
}