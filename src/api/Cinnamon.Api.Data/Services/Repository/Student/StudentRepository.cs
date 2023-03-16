using System.Linq.Expressions;
using Cinnamon.Api.Data.Repository.Interfaces;
using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.Student;
using Cinnamon.Framework.Common;
using Entities = Cinnamon.Api.Data.Repository.Entities;

namespace Cinnamon.Api.Data.Services.Repository.Student;

public class StudentRepository: IStudentRepository
{
    private readonly IDataStore dataStore;

	public StudentRepository(IDataStore dataStore)
	{
        this.dataStore = dataStore;
    }

    public async Task<AppResult<StudentDTO>> Create(int customerId, int familyMemberId, int activityId, int scheduleId, 
        string name, string studentNo, int numberOfSessions, int sessionsAttended, DateTime ExpirationStartDate, DateTime ExpirationEndDate, string remarks = "", string status = "ACTIVE")
    {
        try
        {
            var student = new Entities.Student {
                ActivityId = activityId,
                CustomerId = customerId,
                FamilyMemberId = familyMemberId,
                ScheduleId = scheduleId,
                Name = name,
                StudentNo = studentNo,
                NumberOfSessions = numberOfSessions,
                SessionsAttended = sessionsAttended,
                Remarks = remarks,
                Status = status,
                ExpirationDateStart = ExpirationStartDate,
                ExpirationDateEnd = ExpirationEndDate
            };

            var createdStudent = await dataStore.Student.Add(student);
            if(!createdStudent.Succeeded || createdStudent.Result == null)
            {
                return AppResult<StudentDTO>.CreateFailed(new ApplicationException(createdStudent.Message), createdStudent.Message);
            }
            var newStudent = createdStudent.Result;

            return AppResult<StudentDTO>.CreateSucceeded(new StudentDTO {
                ActivityId = newStudent.ActivityId,
                CustomerId = newStudent.CustomerId,
                Id = newStudent.Id,
                Name = newStudent.Name,
                NumberOfSessions = newStudent.NumberOfSessions,
                Remarks = newStudent.Remarks,
                ScheduleId = newStudent.ScheduleId,
                SessionsAttended = newStudent.SessionsAttended,
                Status = newStudent.Status,
                StudentNo = newStudent.StudentNo,
                ExpirationStartDate = newStudent.ExpirationDateStart,
                ExpirationEndDate = newStudent.ExpirationDateEnd,
            }, "Successfully creation student");
        }
        catch (Exception ex)
        {
            return AppResult<StudentDTO>.CreateFailed(ex, "An errored occured when creating student");
        }
    }

    public async Task<AppResult<IEnumerable<StudentDTO>>> GetAllAsync(int? count, int? skip, int? activityId, int? scheduleId, string? status)
    {
        try
        {
            Expression<Func<Entities.Student,bool>> filter = 
                s => (scheduleId.HasValue ? s.ScheduleId == scheduleId.Value : true) &&
                    (activityId.HasValue ? s.ActivityId == activityId.Value : true) &&
                    (status != null ? s.Status == status : true);
            
            var result = await dataStore.Student.FindAsync(filter, count, skip);
            if(!result.Succeeded || result.Result == null)
            {
                return AppResult<IEnumerable<StudentDTO>>.CreateFailed(result.Error.Exception, result.Message);
            }

            var students = result.Result.Select(s => {
                var studentDto = new StudentDTO {
                    ActivityId = s.ActivityId,
                    CustomerId = s.CustomerId,
                    Id = s.Id,
                    Name = s.Name,
                    NumberOfSessions = s.NumberOfSessions,
                    Remarks = s.Remarks,
                    ScheduleId = s.ScheduleId,
                    SessionsAttended = s.SessionsAttended,
                    Status = s.Status,
                    StudentNo = s.StudentNo,
                    ExpirationStartDate = s.ExpirationDateStart,
                    ExpirationEndDate = s.ExpirationDateEnd,
                };

                return studentDto;
            });

            return AppResult<IEnumerable<StudentDTO>>.CreateSucceeded(students, "Successfully get students");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<StudentDTO>>.CreateFailed(ex, "An error occured when getting students");
        }
    }

    public async Task<AppResult<IEnumerable<StudentDTO>>> GetAllAsync()
    {
        try
        {
            var result = await dataStore.Student.GetAllAsync();
            if(!result.Succeeded || result.Result == null)
            {
                return AppResult<IEnumerable<StudentDTO>>.CreateFailed(result.Error.Exception, result.Message);
            }

            var students = result.Result.Select(s => {
                var studentDto = new StudentDTO {
                    ActivityId = s.ActivityId,
                    CustomerId = s.CustomerId,
                    Id = s.Id,
                    Name = s.Name,
                    NumberOfSessions = s.NumberOfSessions,
                    Remarks = s.Remarks,
                    ScheduleId = s.ScheduleId,
                    SessionsAttended = s.SessionsAttended,
                    Status = s.Status,
                    StudentNo = s.StudentNo,
                    ExpirationStartDate = s.ExpirationDateStart,
                    ExpirationEndDate = s.ExpirationDateEnd     
                };

                return studentDto;
            });

            return AppResult<IEnumerable<StudentDTO>>.CreateSucceeded(students, "Successfully get students");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<StudentDTO>>.CreateFailed(ex, "An error occured when getting students");
        }
    }

    public async Task<AppResult<StudentDTO>> GetByIdAsync(int id)
    {
        try
        {
            var result = await dataStore.Student.GetByIdAsync(id);
            if(!result.Succeeded || result.Result == null)
            {
                return AppResult<StudentDTO>.CreateFailed(result.Error.Exception, result.Message);
            }
            var student = result.Result;

            var studentDto = new StudentDTO {
                ActivityId = student.ActivityId,
                CustomerId = student.CustomerId,
                Id = student.Id,
                Name = student.Name,
                NumberOfSessions = student.NumberOfSessions,
                Remarks = student.Remarks,
                ScheduleId = student.ScheduleId,
                SessionsAttended = student.SessionsAttended,
                Status = student.Status,
                StudentNo = student.StudentNo,
                ExpirationStartDate = student.ExpirationDateStart,
                ExpirationEndDate = student.ExpirationDateEnd   
            };

            return AppResult<StudentDTO>.CreateSucceeded(studentDto, "Successfully get student");
        }
        catch (Exception ex)
        {
            return AppResult<StudentDTO>.CreateFailed(ex, "An error occured when getting student");
        }
    }

    public async Task<AppResult<StudentDTO>> Update(int studendId, string? name, string? studentNo, int? numberOfSessions, 
        int? sessionsAttended, string? remarks, string? status, DateTime? ExpirationStartDate, DateTime? ExpirationEndDate)
    {
        try
        {
            var studentRes = await dataStore.Student.GetByIdAsync(studendId);
            if(!studentRes.Succeeded || studentRes.Result == null)
            {
                return AppResult<StudentDTO>.CreateFailed(studentRes.Error.Exception, studentRes.Message);
            }
            var student = studentRes.Result;

            student.Name = name ?? student.Name;
            student.StudentNo = studentNo ?? student.StudentNo;
            student.NumberOfSessions = numberOfSessions ?? student.NumberOfSessions;
            student.SessionsAttended = sessionsAttended ?? student.SessionsAttended;
            student.Remarks = remarks ?? student.Remarks;
            student.Status = status ?? student.Status;
            student.ExpirationDateStart = ExpirationStartDate ?? student.ExpirationDateStart;
            student.ExpirationDateEnd = ExpirationEndDate ?? student.ExpirationDateEnd;

            var updatedStudentRes = await dataStore.Student.Update(student);
            if(!updatedStudentRes.Succeeded || updatedStudentRes.Result == null)
            {
                return AppResult<StudentDTO>.CreateFailed(updatedStudentRes.Error.Exception, updatedStudentRes.Message);
            }
            var updatedStudent = updatedStudentRes.Result;

            var studentDto = new StudentDTO {
                ActivityId = updatedStudent.ActivityId,
                CustomerId = updatedStudent.CustomerId,
                Id = updatedStudent.Id,
                Name = updatedStudent.Name,
                NumberOfSessions = updatedStudent.NumberOfSessions,
                Remarks = updatedStudent.Remarks,
                ScheduleId = updatedStudent.ScheduleId,
                SessionsAttended = updatedStudent.SessionsAttended,
                Status = updatedStudent.Status,
                StudentNo = updatedStudent.StudentNo,
                ExpirationStartDate = updatedStudent.ExpirationDateStart,
                ExpirationEndDate = updatedStudent.ExpirationDateEnd    
            };

            return AppResult<StudentDTO>.CreateSucceeded(studentDto, "Successfully updated student information");
        }
        catch (Exception ex)
        {
            return AppResult<StudentDTO>.CreateFailed(ex, "An error occured when updating student");
        }
    }

    public async Task<AppResult<IEnumerable<StudentDTO>>> Create(int customerId, int activityId, int scheduleId,int numberOfSessions, 
        int sessionsAttended, DateTime ExpirationStartDate, DateTime ExpirationEndDate, IEnumerable<CreateManyStudentDTO> familyMembers, string remarks = "", string status = "ACTIVE")
    {
        try
        {
            // check customer if existed
            var customerRes = await dataStore.Customer.GetByIdAsync(customerId);
            if(!customerRes.Succeeded || customerRes.Result == null)
            {
                return AppResult<IEnumerable<StudentDTO>>.CreateFailed(
                    new ApplicationException("Can't find customer id provided"), "Can't find customer id provided");
            }

            var students = familyMembers.Select(f => {
                return new Entities.Student {
                    ActivityId = activityId,
                    CustomerId = customerId,
                    FamilyMemberId = f.FamilyMemberId,
                    Name = f.Name,
                    NumberOfSessions = numberOfSessions,
                    Remarks = remarks,
                    ScheduleId = scheduleId,
                    SessionsAttended = sessionsAttended,
                    StudentNo  = f.StudentNo,
                    Status = status,
                    ExpirationDateStart = ExpirationStartDate,
                    ExpirationDateEnd = ExpirationEndDate,
                };
            });

            var createdRes = await dataStore.Student.AddRange(students);
            if(!createdRes.Succeeded || createdRes.Result == null)
            {
                return AppResult<IEnumerable<StudentDTO>>.CreateFailed(createdRes.Error.Exception, createdRes.Message);
            }

            var createdStudents = createdRes.Result.Select(s => {
                return new StudentDTO {
                    ActivityId = s.ActivityId,
                    CustomerId = s.CustomerId,
                    Id = s.Id,
                    Name = s.Name,
                    NumberOfSessions = s.NumberOfSessions,
                    Remarks = s.Remarks,
                    ScheduleId = s.ScheduleId,
                    SessionsAttended = s.SessionsAttended,
                    Status = s.Status,
                    StudentNo = s.StudentNo,
                    ExpirationStartDate = s.ExpirationDateStart,
                    ExpirationEndDate = s.ExpirationDateEnd

                };
            });

            return AppResult<IEnumerable<StudentDTO>>.CreateSucceeded(createdStudents, "Successfully created studets");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<StudentDTO>>.CreateFailed(ex, "An error occured when creating many students");
        }
    }

    public async Task<AppResult<IEnumerable<StudentDTO>>> GetEnrolledStudent(int ActivityId)
    {
        try
        {
            Expression<Func<Entities.Student, bool>> filter = a => (a.ActivityId == ActivityId);
            var result= await dataStore.Student.FindAsync(filter);
            if (!result.Succeeded || result.Result == null)
            {
                return AppResult<IEnumerable<StudentDTO>>.CreateFailed(result.Error.Exception, result.Message);
            }
            var students = result.Result.Select(s => {
                var studentDto = new StudentDTO
                {
                    ActivityId = s.ActivityId,
                    CustomerId = s.CustomerId,
                    Id = s.Id,
                    Name = s.Name,
                    NumberOfSessions = s.NumberOfSessions,
                    Remarks = s.Remarks,
                    ScheduleId = s.ScheduleId,
                    SessionsAttended = s.SessionsAttended,
                    Status = s.Status,
                    StudentNo = s.StudentNo,
                    ExpirationStartDate = s.ExpirationDateStart,
                    ExpirationEndDate = s.ExpirationDateEnd
                };

                return studentDto;
            });

            return AppResult<IEnumerable<StudentDTO>>.CreateSucceeded(students, "Successfully get students");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<StudentDTO>>.CreateFailed(ex, "An error occured when getting students");
        }
    }
}