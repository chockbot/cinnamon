using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Repository.Interfaces;

namespace Cinnamon.Api.Data.Repository.DbSets;

public class OteDateOverrideEntity : GenericEntity<OteDateOverride>, IOteDateOverride
{
    public OteDateOverrideEntity(ApplicationContext applicationContext)
        : base(applicationContext)
    {
    }
}