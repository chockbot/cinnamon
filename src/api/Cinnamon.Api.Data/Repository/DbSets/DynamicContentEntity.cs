using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Repository.Interfaces;

namespace Cinnamon.Api.Data.Repository.DbSets;

public class DynamicContentEntity : GenericEntity<DynamicContent>, IDynamicContent
{
    public DynamicContentEntity(ApplicationContext applicationContext)
        : base(applicationContext)
    {
    }
}