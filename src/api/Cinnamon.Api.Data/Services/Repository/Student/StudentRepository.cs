using System.Linq.Expressions;
using Cinnamon.Api.Data.Repository.Interfaces;
using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.Student;
using Cinnamon.Framework.Common;
using Entities = Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Extensions;

namespace Cinnamon.Api.Data.Services.Repository.Student;

public class StudentRepository: IStudentRepository
{
	private readonly IDataStore dataStore;

	public StudentRepository(IDataStore dataStore)
	{
		this.dataStore = dataStore;
	}

	public async Task<AppResult<StudentDTO>> Create(int customerId, int familyMemberId, int activityId, int scheduleId, 
		string name, string studentNo, int numberOfSessions, int sessionsAttended, int numberOfBacktracking, DateTime ExpirationStartDate, DateTime ExpirationEndDate, 
		int ongoingActivityId, string remarks = "", string status = "ACTIVE", bool isDisbursement = false, bool hasReview = false)
	{
		try
		{
			ExpirationStartDate = ExpirationStartDate.SetKindUtc();
			ExpirationEndDate = ExpirationEndDate.SetKindUtc();
			
			var student = new Entities.Student {
				CustomerId = customerId,
				FamilyMemberId = familyMemberId,
				ActivityId = activityId,
				ScheduleId = scheduleId,
				Name = name,
				StudentNo = studentNo,
				NumberOfSessions = numberOfSessions,
				SessionsAttended = sessionsAttended,
				NumberOfBacktracking = numberOfBacktracking,
				Remarks = remarks,
				Status = status,
				ExpirationDateStart = ExpirationStartDate,
				ExpirationDateEnd = ExpirationEndDate,
				OngoingActivityId = ongoingActivityId,
				IsDisbursement = isDisbursement,
				HasReview = hasReview
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
				NumberOfBackTracking = newStudent.NumberOfBacktracking,
				Status = newStudent.Status,
				StudentNo = newStudent.StudentNo,
				ExpirationStartDate = newStudent.ExpirationDateStart,
				ExpirationEndDate = newStudent.ExpirationDateEnd,
				IsDisbursement = newStudent.IsDisbursement,
				HasReview = newStudent.HasReview,
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
					NumberOfBackTracking = s.NumberOfBacktracking,
					Remarks = s.Remarks,
					ScheduleId = s.ScheduleId,
					SessionsAttended = s.SessionsAttended,
					Status = s.Status,
					StudentNo = s.StudentNo,
					ExpirationStartDate = s.ExpirationDateStart,
					ExpirationEndDate = s.ExpirationDateEnd,
					IsDisbursement = s.IsDisbursement,
					HasReview = s.HasReview,
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
					NumberOfBackTracking = s.NumberOfBacktracking,
					Status = s.Status,
					StudentNo = s.StudentNo,
					ExpirationStartDate = s.ExpirationDateStart,
					ExpirationEndDate = s.ExpirationDateEnd,
					IsDisbursement = s.IsDisbursement,
					HasReview = s.HasReview
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
				NumberOfBackTracking = student.NumberOfBacktracking,
				Status = student.Status,
				StudentNo = student.StudentNo,
				ExpirationStartDate = student.ExpirationDateStart,
				ExpirationEndDate = student.ExpirationDateEnd,
				IsDisbursement = student.IsDisbursement,
				HasReview = student.HasReview,  
			};

			return AppResult<StudentDTO>.CreateSucceeded(studentDto, "Successfully get student");
		}
		catch (Exception ex)
		{
			return AppResult<StudentDTO>.CreateFailed(ex, "An error occured when getting student");
		}
	}

	public async Task<AppResult<StudentDTO>> Update(int studendId, string? name, string? studentNo, int? numberOfSessions, 
		int? sessionsAttended, int? numberOfBacktracking, string? remarks, string? status, DateTime? ExpirationStartDate, DateTime? ExpirationEndDate, bool? hasReview)
	{
		try
		{
			var studentRes = await dataStore.Student.GetByIdAsync(studendId);
			if(!studentRes.Succeeded || studentRes.Result == null)
			{
				return AppResult<StudentDTO>.CreateFailed(studentRes.Error.Exception, studentRes.Message);
			}
			var student = studentRes.Result;

			ExpirationStartDate = ExpirationStartDate.SetKindUtc();
			ExpirationEndDate = ExpirationEndDate.SetKindUtc();

			student.Name = name ?? student.Name;
			student.StudentNo = studentNo ?? student.StudentNo;
			student.NumberOfSessions = numberOfSessions ?? student.NumberOfSessions;
			student.SessionsAttended = sessionsAttended ?? student.SessionsAttended;
			student.NumberOfBacktracking = numberOfBacktracking ?? student.NumberOfBacktracking;
			student.Remarks = remarks ?? student.Remarks;
			student.Status = status ?? student.Status;
			student.ExpirationDateStart = ExpirationStartDate ?? student.ExpirationDateStart;
			student.ExpirationDateEnd = ExpirationEndDate ?? student.ExpirationDateEnd;
			student.HasReview = hasReview ?? student.HasReview;

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
				NumberOfBackTracking = updatedStudent.NumberOfBacktracking,
				Status = updatedStudent.Status,
				StudentNo = updatedStudent.StudentNo,
				ExpirationStartDate = updatedStudent.ExpirationDateStart,
				ExpirationEndDate = updatedStudent.ExpirationDateEnd,
				HasReview = updatedStudent.HasReview,
			};

			return AppResult<StudentDTO>.CreateSucceeded(studentDto, "Successfully updated student information");
		}
		catch (Exception ex)
		{
			return AppResult<StudentDTO>.CreateFailed(ex, "An error occured when updating student");
		}
	}

	public async Task<AppResult<IEnumerable<StudentDTO>>> Create(int customerId, int activityId, int scheduleId,int numberOfSessions, 
		int sessionsAttended, int numberOfBacktracking, DateTime ExpirationStartDate, DateTime ExpirationEndDate, IEnumerable<CreateManyStudentDTO> familyMembers, 
		int ongoingActivityId, string remarks = "", string status = "ACTIVE", bool isDisbursement = false, bool hasReview = false)
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
			
			ExpirationStartDate = ExpirationStartDate.SetKindUtc();
			ExpirationEndDate = ExpirationEndDate.SetKindUtc();

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
					NumberOfBacktracking = numberOfBacktracking,
					StudentNo  = f.StudentNo,
					Status = status,
					ExpirationDateStart = ExpirationStartDate,
					ExpirationDateEnd = ExpirationEndDate,
					OngoingActivityId = ongoingActivityId,
					IsDisbursement = isDisbursement,
					HasReview = hasReview,
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
					NumberOfBackTracking = s.NumberOfBacktracking,
					Status = s.Status,
					StudentNo = s.StudentNo,
					ExpirationStartDate = s.ExpirationDateStart,
					ExpirationEndDate = s.ExpirationDateEnd,
					IsDisbursement = s.IsDisbursement,
					HasReview = s.HasReview
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
					NumberOfBackTracking = s.NumberOfBacktracking,
					Status = s.Status,
					StudentNo = s.StudentNo,
					ExpirationStartDate = s.ExpirationDateStart,
					ExpirationEndDate = s.ExpirationDateEnd,
					IsDisbursement = s.IsDisbursement,
					HasReview = s.HasReview
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

	public async Task<AppResult<IEnumerable<DisburseStudentDTO>>> GetStudentsToDisburse(bool isInclusive, bool isExpired = false)
	{
		try
		{
			var result = isExpired ? await dataStore.Student.GetAllExpiredStudentsToDisburse() : 
				( 
					isInclusive ? await dataStore.Student.GetAllInclusiveStudentsToDisburse() : await dataStore.Student.GetAllStudentsToDisburse()
				);
			if(!result.Succeeded || result.Result == null)
			{
				return AppResult<IEnumerable<DisburseStudentDTO>>.CreateFailed(result.Error.Exception, result.Message);
			}

			return AppResult<IEnumerable<DisburseStudentDTO>>.CreateSucceeded(result.Result, "Successfully get all students need to disburse");
		}
		catch (Exception ex)
		{
			return AppResult<IEnumerable<DisburseStudentDTO>>.CreateFailed(ex, "An error occured when getting students to disburse");
		}
	}

	public async Task<AppResult<IEnumerable<StudentDTO>>> UpdateStudentsDisbursementStatus(IEnumerable<int> ids, bool isDisbursement)
	{
		try
		{
			var entities = ids.Select(i => {
				return new Entities.Student {
					Id = i,
					IsDisbursement = isDisbursement
				};
			});

			var updateStatusResult = await dataStore.Student.UpdateStudentsDisbursementStatus(entities);
			if(!updateStatusResult.Succeeded || updateStatusResult.Result == null)
			{
				return AppResult<IEnumerable<StudentDTO>>.CreateFailed(new ApplicationException(updateStatusResult.Message), updateStatusResult.Message);
			}

			return AppResult<IEnumerable<StudentDTO>>.CreateSucceeded(updateStatusResult.Result.Select(p => {
				return new StudentDTO {
					Id = p.Id,
					IsDisbursement = p.IsDisbursement
				};
			}), "Successfully update students disbursement status");
		}
		catch (Exception ex)
		{
			return AppResult<IEnumerable<StudentDTO>>.CreateFailed(ex, "An error occured when updating students disbursement status");
		}
	}

	public async Task<AppResult<IEnumerable<StudentDTO>>> GetCompletedStudentsById(int customerId, int? count, int? skip)
	{
		try
		{
			var result = await dataStore.Student.GetCompletedStudentById(customerId);
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
					NumberOfBackTracking = s.NumberOfBackTracking,
					Remarks = s.Remarks,
					ScheduleId = s.ScheduleId,
					SessionsAttended = s.SessionsAttended,
					Status = s.Status,
					StudentNo = s.StudentNo,
					ExpirationStartDate = s.ExpirationStartDate,
					ExpirationEndDate = s.ExpirationEndDate,
					IsDisbursement = s.IsDisbursement,
					HasReview = s.HasReview,
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

	public async Task<AppResult<IEnumerable<StudentDTO>>> GetAllStudentsById(int customerId, int? count, int? skip)
	{
		try
		{
			var result = await dataStore.Student.GetAllStudentById(customerId);
			if (!result.Succeeded || result.Result == null)
			{
				return AppResult<IEnumerable<StudentDTO>>.CreateFailed(result.Error.Exception, result.Message);
			}
			var students = result.Result.Select(s => {
				var studentDto = new StudentDTO
				{
					ActivityId           = s.ActivityId,
					CustomerId           = s.CustomerId,
					Id                   = s.Id,
					Name                 = s.Name,
					NumberOfSessions     = s.NumberOfSessions,
					NumberOfBackTracking = s.NumberOfBackTracking,
					Remarks              = s.Remarks,
					ScheduleId           = s.ScheduleId,
					SessionsAttended     = s.SessionsAttended,
					Status               = s.Status,
					ExpirationStartDate  = s.ExpirationStartDate,
					ExpirationEndDate    = s.ExpirationEndDate,
					HasReview            = s.HasReview,
					PurchaseDate		 = s.PurchaseDate,
					Title				 = s.Title,
					activitySchedule = new Framework.ApiCommand.ApiData.DTO.ActivitySchedule.ActivityScheduleDTO
					{
						Id            = s.activitySchedule.Id,
						HasExpiration = s.activitySchedule.HasExpiration,
						IsSetSession  = s.activitySchedule.IsSetSession,	
					},
					studentAttendance = new Framework.ApiCommand.ApiData.DTO.StudentAttendance.StudentAttendanceDTO
					{
						Id        = s.studentAttendance.Id,
						Date      = s.studentAttendance.Date,
						IsPresent = s.studentAttendance.IsPresent,	
						StudentId = s.studentAttendance.StudentId
					}
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

	public async Task<AppResult<IEnumerable<ExpiredStudentDTO>>> ExpiringStudents()
	{
		try
		{
			var result = await dataStore.Student.ExpiringStudents();
			if(!result.Succeeded || result.Result == null)
			{
				return AppResult<IEnumerable<ExpiredStudentDTO>>.CreateFailed(result.Error.Exception, result.Message);
			}

			return AppResult<IEnumerable<ExpiredStudentDTO>>.CreateSucceeded(result.Result, "Successfully get all students need to disburse");
		}
		catch (Exception ex)
		{
			return AppResult<IEnumerable<ExpiredStudentDTO>>.CreateFailed(ex, "An error occured when getting students to disburse");
		}
	}

	public async Task<AppResult<IEnumerable<StudentDTO>>> GetEnrolleeMasterList(int providerId, int? count, int? skip)
	{
		try
		{
			var result = await dataStore.Student.GetEnrolleeMasterList(providerId, count, skip);
			if (!result.Succeeded || result.Result == null)
			{
				return AppResult<IEnumerable<StudentDTO>>.CreateFailed(result.Error.Exception, result.Message);
			}
			var students = result.Result.Select(s => {
				var studentDto = new StudentDTO
				{
					Id               = s.Id,
					ActivityId       = s.ActivityId,
					CustomerId       = s.CustomerId,
					ScheduleId       = s.ScheduleId,
					Name             = s.Name,
					Age              = s.Age,
					Gender           = s.Gender,
					NumberOfSessions = s.NumberOfSessions,
					SessionsAttended = s.SessionsAttended,
					ActivityTitle    = s.ActivityTitle,
					Email            = s.Email,
					StudentNo        = s.StudentNo,
					Remarks          = s.Remarks,
					FamilyMemberId   = s.FamilyMemberId
				};
				return studentDto;
			});

			return AppResult<IEnumerable<StudentDTO>>.CreateSucceeded(students, "Successfully get enrollee master list");
		}
		catch (Exception ex)
		{
			return AppResult<IEnumerable<StudentDTO>>.CreateFailed(ex, "An error occured when getting enrollee master list");
		}
	}

	public async Task<AppResult<IEnumerable<StudentDTO>>> GetEnrolledStudentsByProvider(int? providerId, string searchValue, int searchBy, int? count, int? skip)
	{
		try
		{
			var result = await dataStore.Student.GetEnrolledStudents(providerId, searchValue, searchBy, count, skip);
			if (!result.Succeeded || result.Result == null)
			{
				return AppResult<IEnumerable<StudentDTO>>.CreateFailed(result.Error.Exception, result.Message);
			}
			var students = result.Result.Select(s => {
				var studentDto = new StudentDTO
				{
					Id                  = s.Id,
					ActivityId          = s.ActivityId,
					ScheduleId          = s.ScheduleId,
					Name                = s.Name,
					NumberOfSessions    = s.NumberOfSessions,
					SessionsAttended    = s.SessionsAttended,
					ActivityTitle       = s.ActivityTitle,
					StudentNo           = s.StudentNo,
					Remarks             = s.Remarks,
					ExpirationEndDate   = s.ExpirationEndDate,
					ExpirationStartDate = s.ExpirationStartDate,
					HasExpiration       = s.HasExpiration
				};
				return studentDto;
			});

			return AppResult<IEnumerable<StudentDTO>>.CreateSucceeded(students, "Successfully get enrolled students");
		}
		catch (Exception ex)
		{
			return AppResult<IEnumerable<StudentDTO>>.CreateFailed(ex, "An error occured when getting enrolled students list");
		}
	}
}