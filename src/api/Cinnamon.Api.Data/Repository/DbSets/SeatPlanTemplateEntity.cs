using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Repository.Interfaces;

namespace Cinnamon.Api.Data.Repository.DbSets;

public class SeatPlanTemplateEntity : GenericEntity<SeatPlanTemplate>, ISeatPlanTemplate
{
    private readonly ApplicationContext applicationContext;

    public SeatPlanTemplateEntity(ApplicationContext applicationContext)
        : base(applicationContext)
    {
        this.applicationContext = applicationContext;
    }
}