using System.Linq.Expressions;
using AutoMapper;
using Cinnamon.Api.Data.Extensions;
using Cinnamon.Api.Data.Repository.Interfaces;
using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.DirectStudent;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.StudentAttendance;
using Cinnamon.Framework.Common;
using Entities = Cinnamon.Api.Data.Repository.Entities;

namespace Cinnamon.Api.Data.Services.Repository.DirectStudent;

public class DirectStudentAttendanceRepository : IDirectStudentAttendanceRepository
{
    private readonly IDataStore dataStore;
    private readonly IMapper mapper;

    public DirectStudentAttendanceRepository(IDataStore dataStore, IMapper mapper)
    {
        this.dataStore = dataStore;
        this.mapper = mapper;
    }

    public async Task<AppResult<IEnumerable<DirectStudentAttendanceDTO>>> CreateDirectStudentAttendances(IEnumerable<DirectStudentAttendanceDTO> attendances)
    {
        try
        {
            var entities = mapper.Map<IEnumerable<Entities.DirectStudentAttendance>>(attendances);

            // set kind utc of the date
            foreach (var item in entities)
            {
                item.Date = item.Date.Date.SetKindUtc();
            }

            var result = await dataStore.DirectStudentAttendance.AddRange(entities);
            if(!result.Succeeded || result.Result is null)
            {
                return AppResult<IEnumerable<DirectStudentAttendanceDTO>>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }

            var dtos = mapper.Map<IEnumerable<DirectStudentAttendanceDTO>>(result.Result);
            return AppResult<IEnumerable<DirectStudentAttendanceDTO>>.CreateSucceeded(dtos, "Successfully create student attendance.");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<DirectStudentAttendanceDTO>>.CreateFailed(ex, "An error occured when creating student attendances.");
        }
    }

    public async Task<AppResult<IEnumerable<StudentAttendanceDTO>>> GetAllAsync(int? count, int? skip, DateTime? date = null, 
        bool? includeStudent = false, IEnumerable<int>? activityIds = null, IEnumerable<int>? scheduleIds = null)
    {
        try
        {
            var includes = new List<Expression<Func<Entities.DirectStudentAttendance, object>>>();
            if (includeStudent.HasValue && includeStudent.Value)
            {
                includes.Add(s => s.DirectStudentSession);
            }

            Expression<Func<Entities.DirectStudentAttendance, bool>> filter = 
                s => (date.HasValue ? s.Date == date.Value.Date.SetKindUtc() : true) &&
                    (activityIds != null ? activityIds.Contains(s.DirectStudentSession.ActivityId) : true) &&
                    (scheduleIds != null ? scheduleIds.Contains(s.DirectStudentSession.ScheduleId) : true);
            
            var result = await dataStore.DirectStudentAttendance.FindAsync(filter, count, skip, includes);
            if(!result.Succeeded || result.Result is null)
            {
                return AppResult<IEnumerable<StudentAttendanceDTO>>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }
            var studentAttendances = result.Result;

            return AppResult<IEnumerable<StudentAttendanceDTO>>.CreateSucceeded(studentAttendances.Select(s => {
                var studentAttendance = new StudentAttendanceDTO {
                    Date = s.Date,
                    Id = s.Id,
                    IsPresent = s.IsPresent,
                    StudentId = s.DirectStudentSessionId
                };

                if(includeStudent.HasValue && includeStudent.Value)
                {
                    var studentSession = s.DirectStudentSession;
                    studentAttendance.Student = new Framework.ApiCommand.ApiData.DTO.Student.StudentDTO {
                        ActivityId = studentSession.ActivityId,
                        Id = studentSession.Id,
                        Name = studentSession.Name,
                        NumberOfSessions = studentSession.NumberOfSessions,
                        Remarks = studentSession.Remarks,
                        ScheduleId = studentSession.ScheduleId,
                        SessionsAttended = studentSession.SessionsAttended,
                        Status = studentSession.Status,
                        StudentNo = studentSession.StudentNo,
                    };
                }

                return studentAttendance;
            }), "Sucessfully get direct students attendance.");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<StudentAttendanceDTO>>.CreateFailed(ex, "An error occured when getting all direct student attendance.");
        }   
    }

    public async Task<AppResult<IEnumerable<StudentAttendanceDTO>>> GetStudentById(int? studentId, int? count, int? skip, bool? includeStudent = false, IEnumerable<int>? activityIds = null, IEnumerable<int>? scheduleIds = null)
    {
        try
        {
            var includes = new List<Expression<Func<Entities.DirectStudentAttendance, object>>>();
            if (includeStudent.HasValue && includeStudent.Value)
            {
                includes.Add(s => s.DirectStudentSession);
            }
            Expression<Func<Entities.DirectStudentAttendance, bool>> filter =
                s => (studentId.HasValue ? s.DirectStudentSessionId == studentId.Value : true) &&
                    (activityIds != null ? activityIds.Contains(s.DirectStudentSession.ActivityId) : true) &&
                    (scheduleIds != null ? scheduleIds.Contains(s.DirectStudentSession.ScheduleId) : true) && 
                    (s.IsPresent == true);

            var result = await dataStore.DirectStudentAttendance.FindAsync(filter, count, skip, includes);
            if (!result.Succeeded || result.Result is null)
            {
                return AppResult<IEnumerable<StudentAttendanceDTO>>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }
            var studentAttendances = result.Result;

            return AppResult<IEnumerable<StudentAttendanceDTO>>.CreateSucceeded(studentAttendances.Select(s => {
                var studentAttendance = new StudentAttendanceDTO
                {
                    Date = s.Date,
                    Id = s.Id,
                    IsPresent = s.IsPresent,
                    StudentId = s.DirectStudentSessionId
                };

                if (includeStudent.HasValue && includeStudent.Value)
                {
                    var studentSession = s.DirectStudentSession;
                    studentAttendance.Student = new Framework.ApiCommand.ApiData.DTO.Student.StudentDTO
                    {
                        ActivityId       = studentSession.ActivityId,
                        Id               = studentSession.Id,
                        Name             = studentSession.Name,
                        NumberOfSessions = studentSession.NumberOfSessions,
                        Remarks          = studentSession.Remarks,
                        ScheduleId       = studentSession.ScheduleId,
                        SessionsAttended = studentSession.SessionsAttended,
                        Status           = studentSession.Status,
                        StudentNo        = studentSession.StudentNo,
                    };
                }

                return studentAttendance;
            }), "Sucessfully get direct students attendance.");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<StudentAttendanceDTO>>.CreateFailed(ex, "An error occured when direct student attendance.");
        }
    }

    public async Task<AppResult<IEnumerable<DirectStudentAttendanceDTO>>> UpdateStudentAttendances(IEnumerable<DirectStudentAttendanceDTO> attendances, DateTime date)
    {
        try
        {
            var entities = mapper.Map<IEnumerable<Entities.DirectStudentAttendance>>(attendances);

            var result = await dataStore.DirectStudentAttendance.UpdateStudentAttendance(entities, date.SetKindUtc());
            if(!result.Succeeded || result.Result is null)
            {
                return AppResult<IEnumerable<DirectStudentAttendanceDTO>>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }

            return AppResult<IEnumerable<DirectStudentAttendanceDTO>>.CreateSucceeded(attendances, "Successfully update direct student attendance.");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<DirectStudentAttendanceDTO>>.CreateFailed(ex, "An error occured when updating direct student attendance.");
        }
    }
}