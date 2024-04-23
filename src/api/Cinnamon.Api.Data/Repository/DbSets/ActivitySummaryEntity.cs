using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Repository.Interfaces;

namespace Cinnamon.Api.Data.Repository.DbSets;

public class ActivitySummaryEntity : GenericEntity<ActivitySummary>, IActivitySummary
{
    public ActivitySummaryEntity(ApplicationContext applicationContext)
        : base(applicationContext)
    {
    }
}