using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Repository.Interfaces;

namespace Cinnamon.Api.Data.Repository.DbSets;

public class OteReminderFlagEntity : GenericEntity<OteReminderFlag>, IOteReminderFlag
{
    public OteReminderFlagEntity(ApplicationContext applicationContext)
        :base(applicationContext)
    { 
    }
}
