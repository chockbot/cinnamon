using System.Linq.Expressions;
using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Repository.Interfaces;
using Cinnamon.Framework.Common;
using Microsoft.EntityFrameworkCore;

namespace Cinnamon.Api.Data.Repository.DbSets;

public class SeatPlanTemplateEntity : GenericEntity<SeatPlanTemplate>, ISeatPlanTemplate
{
    private readonly ApplicationContext applicationContext;

    public SeatPlanTemplateEntity(ApplicationContext applicationContext)
        : base(applicationContext)
    {
        this.applicationContext = applicationContext;
    }

    public async Task<AppResult<IEnumerable<SeatPlanTemplate>>> SeatPlanWithoutPayload(Expression<Func<SeatPlanTemplate, bool>> filter, int page, int limit)
    {
        try
        {
            var seatPlanTemplate = await applicationContext.SeatPlanTemplates
                .Where(filter)
                .Skip((page - 1) * limit)
                .Take(limit)
                .Select(spt => new SeatPlanTemplate
                {
                    Name = spt.Name,
                    Address = spt.Address,
                    ImageSrc = spt.ImageSrc,
                    Enabled = spt.Enabled,
                    Id = spt.Id,
                    SeatPlanFormatterId = spt.SeatPlanFormatterId,
                    UploadedBy = spt.UploadedBy,
                    UploadedDate = spt.UploadedDate
                })
                .ToListAsync();

            return AppResult<IEnumerable<SeatPlanTemplate>>.CreateSucceeded(seatPlanTemplate, "Seat plan template retrieved successfully");
        }
        catch (System.Exception ex)
        {
            return AppResult<IEnumerable<SeatPlanTemplate>>.CreateFailed(ex, "An error occured in getting seat plan template");
        }
    }
}