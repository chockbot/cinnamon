using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Repository.Interfaces;

namespace Cinnamon.Api.Data.Repository.DbSets;

public class ActivityDescriptionEntity : GenericEntity<ActivityDescription>, IActivityDescription
{
    public ActivityDescriptionEntity(ApplicationContext applicationContext)
        : base(applicationContext)
    {
    }
}