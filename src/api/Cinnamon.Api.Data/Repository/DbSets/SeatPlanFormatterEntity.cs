using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Repository.Interfaces;

namespace Cinnamon.Api.Data.Repository.DbSets;

public class SeatPlanFormatterEntity : GenericEntity<SeatPlanFormatter>, ISeatPlanFormatter
{
    private readonly ApplicationContext applicationContext;

    public SeatPlanFormatterEntity(ApplicationContext applicationContext)
        : base(applicationContext)
    {
        this.applicationContext = applicationContext;
    }
}