using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Repository.Interfaces;

namespace Cinnamon.Api.Data.Repository.DbSets
{
    public class OngoingActivityScheduleTimeEntity : GenericEntity<OngoingActivityScheduleTime>, IOngoingActivityScheduleTime
    {
        private readonly ApplicationContext applicationContext;

        public OngoingActivityScheduleTimeEntity(ApplicationContext applicationContext) : base(applicationContext)
        {
            this.applicationContext = applicationContext;
        }
    }
}
