using System.Linq.Expressions;
using AutoMapper;
using Cinnamon.Api.Data.Extensions;
using Cinnamon.Api.Data.Repository.Interfaces;
using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.OteDate;
using Cinnamon.Framework.Common;
using Entities = Cinnamon.Api.Data.Repository.Entities;

namespace Cinnamon.Api.Data.Services.Repository.OteDate;

public class OteDateRepository : IOteDateRepository
{
    private readonly IDataStore dataStore;
    private readonly IMapper mapper;

    public OteDateRepository(IDataStore dataStore, IMapper mapper)
    {
        this.dataStore = dataStore;
        this.mapper = mapper;
    }
    
    public async Task<AppResult<OteDateDTO>> GetOteDateById(int id)
    {
        try
        {
            var result = await dataStore.OteDate.GetByIdAsync(id);
            if(!result.Succeeded || result.Result is null)
            {
                return AppResult<OteDateDTO>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }
            var oteDate = result.Result;

            return AppResult<OteDateDTO>.CreateSucceeded(
                new OteDateDTO {
                    Date = oteDate.Date,
                    DateEnd = oteDate.DateEnd,
                    DateStart = oteDate.DateStart,
                    Id = oteDate.Id,
                    ScheduleId = oteDate.OteScheduleId
                },
                "Successfully get ote date by id"
            );
        }
        catch (Exception ex)
        {
            return AppResult<OteDateDTO>.CreateFailed(ex, "An error occured when getting ote dates");
        }
    }

    public async Task<AppResult<IEnumerable<OteDateDTO>>> GetOteDate (int activityId, DateTime? from, DateTime? to)
    {
        try
        {
            Expression<Func<Entities.OteDate, bool>> filter = 
                d => (d.OteSchedule.ActivityId == activityId) &&
                    (from.HasValue && to.HasValue ? from.Value.Date.SetKindUtc() <= d.Date && to.Value.Date.SetKindUtc() >= d.Date : true);

            var includes = new List<Expression<Func<Entities.OteDate, object>>>();
            includes.Add(d => d.OteSchedule);

            var result = await dataStore.OteDate.FindAsync(filter, int.MaxValue, 0, includes);
            if(!result.Succeeded || result.Result is null)
            {
                return AppResult<IEnumerable<OteDateDTO>>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }

            return AppResult<IEnumerable<OteDateDTO>>.CreateSucceeded(result.Result.Select(d => new OteDateDTO {
                Date = d.Date,
                DateEnd = d.DateEnd,
                DateStart = d.DateStart,
                Id = d.Id,
                ScheduleId = d.OteScheduleId
            }), "Successfully get ote dates.");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<OteDateDTO>>.CreateFailed(ex, "An error occured when getting ote dates.");
        }
    }
}