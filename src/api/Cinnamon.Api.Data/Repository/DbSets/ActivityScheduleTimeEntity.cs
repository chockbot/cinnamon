using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Repository.Interfaces;

namespace Cinnamon.Api.Data.Repository.DbSets
{
    public class ActivityScheduleTimeEntity : GenericEntity<ActivityScheduleTime>, IActivityScheduleTime
    {
        private readonly ApplicationContext applicationContext;

        public ActivityScheduleTimeEntity(ApplicationContext applicationContext) : base(applicationContext)
        {
            this.applicationContext = applicationContext;
        }
    }
}
