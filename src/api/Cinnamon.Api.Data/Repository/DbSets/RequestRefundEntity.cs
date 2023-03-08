using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Repository.Interfaces;

namespace Cinnamon.Api.Data.Repository.DbSets;

public class RequestRefundEntity : GenericEntity<RequestRefund>, IRequestRefund 
{
    public RequestRefundEntity(ApplicationContext applicationContext)
        : base(applicationContext)
    {
    }
}