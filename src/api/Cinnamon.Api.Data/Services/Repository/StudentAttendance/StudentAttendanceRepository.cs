using System.Linq.Expressions;
using Cinnamon.Api.Data.Repository.Interfaces;
using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.StudentAttendance;
using Cinnamon.Framework.Common;
using Entities = Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Extensions;

namespace Cinnamon.Api.Data.Services.Repository.StudentAttendance;

public class StudentAttendanceRepository: IStudentAttendanceRepository
{
    private readonly IDataStore dataStore;

	public StudentAttendanceRepository (IDataStore dataStore)
	{
        this.dataStore = dataStore;
    }

    public async Task<AppResult<StudentAttendanceDTO>> GetByIdAsync(int id, bool? includeStudent = false)
    {
        try
        {
            var includes = new List<Expression<Func<Entities.StudentAttendance, object>>>();
            if(includeStudent.HasValue && includeStudent.Value)
            {
                includes.Add(s => s.Student);
            }

            var result = await dataStore.StudentAttendance.FindFirstAsync(s => s.Id == id, includes);
            if(!result.Succeeded || result.Result == null)
            {
                return AppResult<StudentAttendanceDTO>.CreateFailed(result.Error.Exception, result.Message);
            }
            var studentAttendance = result.Result;

            var studendDTO = new StudentAttendanceDTO {
                Date = studentAttendance.Date,
                Id = studentAttendance.Id,
                IsPresent = studentAttendance.IsPresent,
                StudentId = studentAttendance.StudentId
            };

            if(includeStudent.HasValue && includeStudent.Value)
            {
                var student = studentAttendance.Student;
                studendDTO.Student = new Framework.ApiCommand.ApiData.DTO.Student.StudentDTO 
                {
                    ActivityId = student.ActivityId,
                    CustomerId = student.CustomerId,
                    Id = student.Id,
                    Name = student.Name,
                    NumberOfSessions = student.NumberOfSessions,
                    Remarks = student.Remarks,
                    ScheduleId = student.ScheduleId,
                    SessionsAttended = student.SessionsAttended,
                    Status = student.Status,
                    StudentNo = student.StudentNo
                };
            }

            return AppResult<StudentAttendanceDTO>.CreateSucceeded(studendDTO, "Successfully get student attendance");
        }
        catch (Exception ex)
        {
            return AppResult<StudentAttendanceDTO>.CreateFailed(ex, "An error occured when getting the student attendance");
        }
    }
    
    public async Task<AppResult<IEnumerable<StudentAttendanceDTO>>> GetAllAsync(int? count, int? skip, 
        DateTime? date = null, bool? includeStudent = false, int? activityId = null, int? scheduleId = null)
    {
        try
        {
            var includes = new List<Expression<Func<Entities.StudentAttendance, object>>>();
            if(includeStudent.HasValue && includeStudent.Value)
            {
                includes.Add(s => s.Student);
            }

            Expression<Func<Entities.StudentAttendance,bool>> filter = 
                a => (date.HasValue ? a.Date == date.Value.Date.SetKindUtc() : true) &&
                    (activityId.HasValue ? a.Student.ActivityId == activityId.Value : true) &&
                    (scheduleId.HasValue ? a.Student.ScheduleId == scheduleId.Value : true);

            var result = await dataStore.StudentAttendance.FindAsync(filter, count, skip, includes);
            if(!result.Succeeded || result.Result == null)
            {
                return AppResult<IEnumerable<StudentAttendanceDTO>>.CreateFailed(result.Error.Exception, result.Message);
            }
            var studentAttendances = result.Result;
            
            return AppResult<IEnumerable<StudentAttendanceDTO>>.CreateSucceeded(studentAttendances.Select(s => 
            {
                var studentAttendance = new StudentAttendanceDTO 
                {
                    Date = s.Date,
                    Id = s.Id,
                    IsPresent = s.IsPresent,
                    StudentId = s.StudentId
                };

                if(includeStudent.HasValue && includeStudent.Value)
                {
                    var student = s.Student;
                    studentAttendance.Student = new Framework.ApiCommand.ApiData.DTO.Student.StudentDTO {
                        ActivityId = student.ActivityId,
                        CustomerId = student.CustomerId,
                        Id = student.Id,
                        Name = student.Name,
                        NumberOfSessions = student.NumberOfSessions,
                        Remarks = student.Remarks,
                        ScheduleId = student.ScheduleId,
                        SessionsAttended = student.SessionsAttended,
                        Status = student.Status,
                        StudentNo = student.StudentNo
                    };
                }

                return studentAttendance;
            }), "Successfully get student attendances");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<StudentAttendanceDTO>>.CreateFailed(ex, "An error occured when getting all student attendance");
        }
    }

    public async Task<AppResult<IEnumerable<StudentAttendanceDTO>>> GetAllAsync()
    {
        try
        {
            var result = await dataStore.StudentAttendance.GetAllAsync();
            if(!result.Succeeded || result.Result == null)
            {
                return AppResult<IEnumerable<StudentAttendanceDTO>>.CreateFailed(result.Error.Exception, result.Message);
            }
            var studentAttendances = result.Result;
            
            return AppResult<IEnumerable<StudentAttendanceDTO>>.CreateSucceeded(studentAttendances.Select(s => 
            {
                return new StudentAttendanceDTO {
                    Date = s.Date,
                    Id = s.Id,
                    IsPresent = s.IsPresent,
                    StudentId = s.StudentId
                };
            }), "Successfully get student attendances");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<StudentAttendanceDTO>>.CreateFailed(ex, "An error occured when getting all student attendance");
        }
    }

    public async Task<AppResult<StudentAttendanceDTO>> Create(int studentId, bool isPresent, DateTime date)
    {
        try
        {
            var studentAttendance = new Entities.StudentAttendance {
                Date = date.Date.SetKindUtc(),
                IsPresent = isPresent,
                StudentId = studentId
            };

            var result = await dataStore.StudentAttendance.Add(studentAttendance);
            if(!result.Succeeded || result.Result == null)
            {
                return AppResult<StudentAttendanceDTO>.CreateFailed(
                    new ApplicationException("An error occured when creating student"), "An error occured when creating student");
            }

            return AppResult<StudentAttendanceDTO>.CreateSucceeded(new StudentAttendanceDTO {
                Date = result.Result.Date,
                Id = result.Result.Id,
                IsPresent = result.Result.IsPresent,
                StudentId = result.Result.StudentId
            }, "Successfully created student attendance");
        }
        catch (Exception ex)
        {
            return  AppResult<StudentAttendanceDTO>.CreateFailed(ex, "An error occured when creating student");
        }
    }

    public async Task<AppResult<IEnumerable<StudentAttendanceDTO>>> Create(IEnumerable<CreateManyAttendanceDTO> students)
    {
        try
        {
            var newStudents = students.Select(s => {
                return new Entities.StudentAttendance {
                    Date = s.Date.Date.SetKindUtc(),
                    IsPresent = s.IsPresent,
                    StudentId = s.StudentId
                };
            });

            var result = await dataStore.StudentAttendance.AddRange(newStudents);
            if(!result.Succeeded || result.Result == null)
            {
                return AppResult<IEnumerable<StudentAttendanceDTO>>.CreateFailed(
                    new ApplicationException("An error occured when creating students"), "An error occured when creating students");
            }

            return AppResult<IEnumerable<StudentAttendanceDTO>>.CreateSucceeded(result.Result.Select(s => {
                return new StudentAttendanceDTO {
                    Date = s.Date,
                    Id = s.Id,
                    IsPresent = s.IsPresent,
                    StudentId = s.StudentId
                };
            }), "Successfully created students");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<StudentAttendanceDTO>>.CreateFailed(ex, "An error occured when creating many student attendance");
        }
    }

    public async Task<AppResult<StudentAttendanceDTO>> Update(int id, bool? isPresent, DateTime? date)
    {
        try
        {
            var studentAttendance = await dataStore.StudentAttendance.GetByIdAsync(id);
            if(!studentAttendance.Succeeded || studentAttendance.Result == null)
            {
                return AppResult<StudentAttendanceDTO>.CreateFailed(
                    new ApplicationException("Can't find student attendance need to update"), "Can't find student attendance need to update");
            }
            
            var studentEntity = new Entities.StudentAttendance {
                Id = studentAttendance.Result.Id,
                Date = date.HasValue ? date.Value.Date.SetKindUtc() : studentAttendance.Result.Date,
                IsPresent = isPresent ?? studentAttendance.Result.IsPresent
            };

            var result = await dataStore.StudentAttendance.Update(studentEntity);
            if(!result.Succeeded || result.Result == null)
            {
                return AppResult<StudentAttendanceDTO>.CreateFailed(
                    new ApplicationException("An error occured when updating student attendance"),"An error occured when updating student attendance");
            }
            var updated = result.Result;

            return AppResult<StudentAttendanceDTO>.CreateSucceeded(new StudentAttendanceDTO {
                Date = updated.Date,
                Id = updated.Id,
                IsPresent = updated.IsPresent,
                StudentId = updated.StudentId
            }, "Successfully update student attendance");
        }
        catch (Exception ex)
        {
            return AppResult<StudentAttendanceDTO>.CreateFailed(ex, "An error occured when updating student attendance");
        }
    }
    
    public async Task<AppResult<IEnumerable<StudentAttendanceDTO>>> Update(IEnumerable<UpdateManyStudentDTO> students)
    {
        try
        {
            var studentEntities = students.Select(s => {
                return new Entities.StudentAttendance {
                    Id = s.Id,
                    Date = s.Date.Date.SetKindUtc(),
                    IsPresent = s.IsPresent
                };
            });

            var result = await dataStore.StudentAttendance.UpdateRange(studentEntities);
            if(!result.Succeeded || result.Result == null)
            {
                return AppResult<IEnumerable<StudentAttendanceDTO>>.CreateFailed(
                    new ApplicationException("An error occured when updating student attendance"), "An error occured when updating student attendance");
            }

            return AppResult<IEnumerable<StudentAttendanceDTO>>.CreateSucceeded(result.Result.Select(s => {
                return new StudentAttendanceDTO {
                    Date = s.Date,
                    Id = s.Id,
                    IsPresent = s.IsPresent,
                    StudentId = s.StudentId
                };
            }), "Successfully update student attendances");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<StudentAttendanceDTO>>.CreateFailed(ex, "An error occured when updating many student attendance");
        }
    }
}