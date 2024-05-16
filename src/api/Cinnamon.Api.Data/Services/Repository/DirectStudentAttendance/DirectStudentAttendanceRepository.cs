using System.Linq.Expressions;
using Cinnamon.Api.Data.Extensions;
using Cinnamon.Api.Data.Repository.Interfaces;
using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.StudentAttendance;
using Cinnamon.Framework.Common;
using Entities = Cinnamon.Api.Data.Repository.Entities;

namespace Cinnamon.Api.Data.Services.Repository.DirectStudent;

public class DirectStudentAttendanceRepository : IDirectStudentAttendanceRepository
{
    private readonly IDataStore dataStore;

    public DirectStudentAttendanceRepository(IDataStore dataStore)
    {
        this.dataStore = dataStore;
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
}