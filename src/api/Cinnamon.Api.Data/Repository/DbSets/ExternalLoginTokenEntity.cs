using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Repository.Interfaces;

namespace Cinnamon.Api.Data.Repository.DbSets;

public class ExternalLoginTokenEntity : GenericEntity<ExternalLoginToken>, IExternalLoginToken
{
    public ExternalLoginTokenEntity(ApplicationContext applicationContext)
        : base(applicationContext)
    {
    }
}