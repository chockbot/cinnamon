using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Repository.Interfaces;

namespace Cinnamon.Api.Data.Repository.DbSets;

public class ActivityScheduleEntity : GenericEntity<ActivitySchedule>, IActivitySchedule
{
    public ActivityScheduleEntity(ApplicationContext applicationContext)
        : base(applicationContext)
    {
    }
}