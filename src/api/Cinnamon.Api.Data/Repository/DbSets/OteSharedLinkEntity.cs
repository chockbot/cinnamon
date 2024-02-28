using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Repository.Interfaces;

namespace Cinnamon.Api.Data.Repository.DbSets;

public class OteSharedLinkEntity : GenericEntity<OteSharedLink>, IOteSharedLink
{
    public OteSharedLinkEntity(ApplicationContext applicationContext)
        : base(applicationContext)
    {
    }
}