using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Repository.Interfaces;
namespace Cinnamon.Api.Data.Repository.DbSets;
public class OteOnlineEventEntity : GenericEntity<OteOnlineEvent>, IOteOnlineEvent
{
    public OteOnlineEventEntity(ApplicationContext applicationContext)
        :base(applicationContext)
    { 
    }
}
