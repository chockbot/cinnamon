using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Repository.Interfaces;
namespace Cinnamon.Api.Data.Repository.DbSets;

public class BadgeListEntity: GenericEntity<BadgeList>, IBadgeList
{
    public BadgeListEntity(ApplicationContext applicationContext)
        : base(applicationContext)
    {
    }
}
