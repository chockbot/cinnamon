using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Repository.Interfaces;
namespace Cinnamon.Api.Data.Repository.DbSets;
public class OteWaitlistEntity : GenericEntity<OteWaitlist>, IOteWaitlist
{
    public OteWaitlistEntity(ApplicationContext applicationContext) : base(applicationContext)
    {
    }
}
