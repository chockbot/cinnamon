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

	public async Task<AppResult<IEnumerable<DirectStudentInfoDTO>>> GetDirectStudentsInfo(int? providerId, string? searchValue, int? count, int? skip)
	{
		try
		{
			Expression<Func<Entities.DirectStudentInfo, bool>> filter = s =>
		   (providerId.HasValue ? s.ProviderId == providerId.Value : true) &&
		   (!string.IsNullOrEmpty(searchValue) ? s.Name.ToLower().Contains(searchValue.ToLower()) : true);

			var result = await dataStore.DirectStudentInfo.FindAsync(filter, count, skip);

			if (!result.Succeeded || result.Result is null)
			{
				return AppResult<IEnumerable<DirectStudentInfoDTO>>.CreateFailed(new ApplicationException(result.Message), result.Message);
			}

			var dtos = mapper.Map<IEnumerable<DirectStudentInfoDTO>>(result.Result);
			return AppResult<IEnumerable<DirectStudentInfoDTO>>.CreateSucceeded(dtos, "Successfully got direct students info.");
		}
		catch (Exception ex)
		{
			return AppResult<IEnumerable<DirectStudentInfoDTO>>.CreateFailed(ex, "An error occurred when getting direct students info.");
		}
	}


	public async Task<AppResult<IEnumerable<DirectStudentPaymentDTO>>> GetStudentPaymentByProvider(int? providerId, DateTime? dateFrom)
	{
		try
		{
			var result = await dataStore.DirectStudentPayment.GetStudentPaymentByProvider(providerId, dateFrom);
			if (!result.Succeeded || result.Result is null)
			{
				return AppResult<IEnumerable<DirectStudentPaymentDTO>>.CreateFailed(new ApplicationException(result.Message), result.Message);
			}
			var dtos = mapper.Map<IEnumerable<DirectStudentPaymentDTO>>(result.Result);

			return AppResult<IEnumerable<DirectStudentPaymentDTO>>.CreateSucceeded(dtos, "Successfully get direct students payment.");
		}
		catch (Exception ex)
		{
			return AppResult<IEnumerable<DirectStudentPaymentDTO>>.CreateFailed(ex, "An error occured when getting direct students payment.");
		}
	}

	public async Task<AppResult<DirectStudentDTO>> UpdateDirectStudent(DirectStudentDTO directStudent)
	{
		try
		{
			var student = mapper.Map<Entities.DirectStudentInfo>(directStudent.DirectStudentInfo);
			var studentSessions = mapper.Map<Entities.DirectStudentSession>(directStudent.DirectStudentSession);
			var studentPayment = mapper.Map<Entities.DirectStudentPayment>(directStudent.DirectStudentPayment);

			var result = await dataStore.DirectStudentInfo.UpdateDirectStudent(student, studentSessions, studentPayment);
			if(!result.Succeeded || result.Result is null)
			{
				return AppResult<DirectStudentDTO>.CreateFailed(new ApplicationException(result.Message), result.Message);
			}

			return AppResult<DirectStudentDTO>.CreateSucceeded(directStudent, "Successfully update direct student.");
		}
		catch (Exception ex)
		{
			return AppResult<DirectStudentDTO>.CreateFailed(ex, "An error occured when updating direct student.");
		}
	}

	public async Task<AppResult<DirectStudentDTO>> DirectStudentInfo(int studentId)
	{
		try
		{
			var result = await dataStore.DirectStudentInfo.DirecStudentInfo(studentId);
			if(!result.Succeeded || result.Result is null)
			{
				return AppResult<DirectStudentDTO>.CreateFailed(new ApplicationException(result.Message), result.Message);
			}

			return AppResult<DirectStudentDTO>.CreateSucceeded(result.Result, "Successfully get direct student info.");
		}
		catch (Exception ex)
		{
			return AppResult<DirectStudentDTO>.CreateFailed(ex, "An error occured when getting direct student info.");
		}
	}
}