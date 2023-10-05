using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Repository.Interfaces;

namespace Cinnamon.Api.Data.Repository.DbSets;

public class OteTicketEntity : GenericEntity<OteTicket>, IOteTicket 
{
    public OteTicketEntity(ApplicationContext applicationContext)
        : base(applicationContext)
    {
    }
}