using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Repository.Interfaces;

namespace Cinnamon.Api.Data.Repository.DbSets;

public class ProviderCustomQuestionEntity : GenericEntity<ProviderCustomQuestion>, IProviderCustomQuestion
{
    public ProviderCustomQuestionEntity(ApplicationContext applicationContext) : base(applicationContext)
    {
    }
}
