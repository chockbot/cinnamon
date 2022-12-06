using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Repository.Interfaces;

namespace Cinnamon.Api.Data.Repository.DbSets;

public class WaitListEntity : GenericEntity<WaitList>, IWaitList
{
    public WaitListEntity(ApplicationContext applicationContext)
        : base(applicationContext)
    {
    }
}