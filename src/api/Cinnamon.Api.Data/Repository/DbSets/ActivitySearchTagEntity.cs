using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Repository.Interfaces;

namespace Cinnamon.Api.Data.Repository.DbSets;

public class ActivitySearchTagEntity : GenericEntity<ActivitySearchTag>, IActivitySearchTag
{
    public ActivitySearchTagEntity(ApplicationContext applicationContext)
        : base(applicationContext)
    {
    }
}