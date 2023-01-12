using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Repository.Interfaces;

namespace Cinnamon.Api.Data.Repository.DbSets;

public class OngoingActivityEntity : GenericEntity<OngoingActivity>, IOngoingActivity
{
    public OngoingActivityEntity(ApplicationContext applicationContext)
        : base(applicationContext)
    {
    }
}