using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Repository.Interfaces;

namespace Cinnamon.Api.Data.Repository.DbSets;

public class OteScheduleEntity : GenericEntity<OteSchedule>, IOteSchedule
{
    public OteScheduleEntity(ApplicationContext applicationContext)
        : base(applicationContext)
    {
    }
}