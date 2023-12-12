using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Repository.Interfaces;

namespace Cinnamon.Api.Data.Repository.DbSets;
public class AddOnsEntity: GenericEntity<AddOns>, IAddOns
{
    public AddOnsEntity(ApplicationContext applicationContext):base(applicationContext)
    {
    }
}
