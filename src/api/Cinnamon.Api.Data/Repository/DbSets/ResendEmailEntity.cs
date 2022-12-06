using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Repository.Interfaces;

namespace Cinnamon.Api.Data.Repository.DbSets;

public class ResendEmailEntity : GenericEntity<ResendEmail>, IResendEmail
{
    public ResendEmailEntity(ApplicationContext applicationContext)
        : base(applicationContext)
    {
    }
}