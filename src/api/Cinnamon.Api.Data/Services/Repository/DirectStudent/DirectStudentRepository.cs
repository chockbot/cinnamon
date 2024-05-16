using System.Linq.Expressions;
using AutoMapper;
using Cinnamon.Api.Data.Repository.Interfaces;
using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.DirectStudent;
using Cinnamon.Framework.Common;
using Entities = Cinnamon.Api.Data.Repository.Entities;

namespace Cinnamon.Api.Data.Services.Repository.DirectStudent;

public class DirectStudentRepository : IDirectStudentRepository
{
    private readonly IDataStore dataStore;
    private readonly IMapper mapper;

    public DirectStudentRepository(IDataStore dataStore, IMapper mapper)
    {
        this.dataStore = dataStore;
        this.mapper = mapper;
    }

    public async Task<AppResult<IEnumerable<DirectStudentDTO>>> CreateDirectStudents(IEnumerable<DirectStudentDTO> directStudents)
    {
        try
        {
            var createDirectStudentRes = await dataStore.DirectStudentInfo.CreateDirectStudents(directStudents);
            if(!createDirectStudentRes.Succeeded || createDirectStudentRes.Result is null)
            {
                return AppResult<IEnumerable<DirectStudentDTO>>.CreateFailed(new ApplicationException(createDirectStudentRes.Message), createDirectStudentRes.Message);
            }

            return AppResult<IEnumerable<DirectStudentDTO>>.CreateSucceeded(createDirectStudentRes.Result, "Successfully create direct students.");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<DirectStudentDTO>>.CreateFailed(ex, "An error occured when creating direct students.");
        }
    }

    public async Task<AppResult<IEnumerable<DirectStudentSessionDTO>>> GetDirectStudents(int? count, int? skip, int? activityId, int? scheduleId, string? status)
    {
        try
        {
            Expression<Func<Entities.DirectStudentSession, bool>> filter = 
				s => (scheduleId.HasValue ? s.ScheduleId == scheduleId.Value : true) &&
					(activityId.HasValue ? s.ActivityId == activityId.Value : true) &&
					(status != null ? s.Status == status : true);
            
            var result = await dataStore.DirectStudentSession.FindAsync(filter, count, skip);
            if(!result.Succeeded || result.Result is null)
            {
                return AppResult<IEnumerable<DirectStudentSessionDTO>>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }

            var dtos = mapper.Map<IEnumerable<DirectStudentSessionDTO>>(result.Result);

            return AppResult<IEnumerable<DirectStudentSessionDTO>>.CreateSucceeded(dtos, "Successfully get direct students.");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<DirectStudentSessionDTO>>.CreateFailed(ex, "An error occured when getting direct students.");
        }
    }
}