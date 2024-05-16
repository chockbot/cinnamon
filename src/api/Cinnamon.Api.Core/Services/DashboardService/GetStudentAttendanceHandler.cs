using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.DashboardService.Handlers;
using Cinnamon.Api.Core.Services.DashboardService.Interactors;
using Cinnamon.Api.Core.Services.DashboardService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.DashboardService;

public class GetStudentAttendanceHandler : IGetStudentAttendanceHandler
{
    private readonly IStudentAttendanceData studentAttendanceData;
    private readonly IGetOwnedActivityHandler getOwnedActivityHandler;
    private readonly IDirectStudentData directStudentData;
    private readonly IStudentData studentData;

    public GetStudentAttendanceHandler(IStudentAttendanceData studentAttendanceData, IGetOwnedActivityHandler getOwnedActivityHandler,
        IStudentData studentData, IDirectStudentData directStudentData)
    {
        this.studentAttendanceData = studentAttendanceData;
        this.getOwnedActivityHandler = getOwnedActivityHandler;
        this.studentData = studentData;
        this.directStudentData = directStudentData;
    }

    public AppResult<GetStudentAttendanceResult> Execute(GetStudentAttendanceArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<GetStudentAttendanceResult>.CreateFailed(ex, "An error occured in GetStudentAttendanceHandler");
        }
    }

    public async Task<AppResult<GetStudentAttendanceResult>> ExecuteAsync(GetStudentAttendanceArgs args)
    {
        try
        {
            var ownedActivityResult = await getOwnedActivityHandler.ExecuteAsync(new ActivityService.Interactors.GetOwnedActivityArgs {
                ActivityId = args.ActivityId,
                IncludeAtivitySchedules = true
            });
            if(!ownedActivityResult.Succeeded || ownedActivityResult.Result == null)
            {
                return AppResult<GetStudentAttendanceResult>.CreateFailed(ownedActivityResult.Error.Exception, ownedActivityResult.Message);
            }
            var activity = ownedActivityResult.Result;

            // check schedule if associated to activity
            if(!activity.ActivitySchedules.Any(s => s.Id == args.ScheduleId))
            {
                return AppResult<GetStudentAttendanceResult>.CreateFailed(
                    new ApplicationException("Unable to determine activity and schedule"), "Unable to determine activity and schedule");
            }
            var schedule = activity.ActivitySchedules.First(s => s.Id == args.ScheduleId);

            // check if already have attendance schedule in the date provided
            // if not yet then create and insert to db.
            var attendanceRes = await studentAttendanceData.GetAllStudentAttendance(new Framework.ApiCommand.ApiData.StudentAttendance.Request.GetAllStudentAttendanceArgs {
                ActivityIds = new int[] {args.ActivityId},
                IsIncludeStudent = true,
                ScheduleIds = new int[] {args.ScheduleId},
                Date = args.Date.ToString("yyyyMMdd")
            });
            if(!attendanceRes.Succeeded || attendanceRes.Result == null || !attendanceRes.Result.IsSuccess)
            {
                return AppResult<GetStudentAttendanceResult>.CreateFailed(new ApplicationException(attendanceRes.Result?.ErrorInfo?.Message), attendanceRes.Message);
            }
            var attendances = attendanceRes.Result.Result;

            // for direct student attendance
            var directStudentAttendanceRes = await directStudentData.StudentAttendance(new Framework.ApiCommand.ApiData.DirectStudent.Request.StudentAttendanceArgs {
                ActivityIds = new int[] {args.ActivityId},
                IncludeStudent = true,
                ScheduleIds = new int[] {args.ScheduleId},
                Date = args.Date.ToString("yyyyMMdd")
            });
            if(!directStudentAttendanceRes.Succeeded || directStudentAttendanceRes.Result is null || !directStudentAttendanceRes.Result.IsSuccess)
            {
                return AppResult<GetStudentAttendanceResult>.CreateFailed(
                    new ApplicationException(directStudentAttendanceRes.Result?.ErrorInfo?.Message), directStudentAttendanceRes.Message);
            }
            var directStudentAttendance = directStudentAttendanceRes.Result.Result;

            // get enrolled students ativity schedule
            var studentRes = await studentData.GetAllStudents(new Framework.ApiCommand.ApiData.Student.Request.GetAllStudentArgs {
                ActivityId = args.ActivityId,
                ScheduleId = args.ScheduleId,
                Status = "ACTIVE"
            });
            if(!studentRes.Succeeded || studentRes.Result == null || !studentRes.Result.IsSuccess)
            {
                return AppResult<GetStudentAttendanceResult>.CreateFailed(
                    new ApplicationException(studentRes.Result?.ErrorInfo?.Message), studentRes.Message);
            }
            var students = studentRes.Result.Result;

            // for direct students
            var directStudentRes = await directStudentData.GetDirectStudents(new Framework.ApiCommand.ApiData.DirectStudent.Request.GetDirectStudentsArgs {
                ActivityId = args.ActivityId,
                ScheduleId = args.ScheduleId,
                Status = "ACTIVE"
            });
            if(!directStudentRes.Succeeded || directStudentRes.Result is null || !directStudentRes.Result.IsSuccess)
            {
                return AppResult<GetStudentAttendanceResult>.CreateFailed(
                    new ApplicationException(directStudentRes.Result?.ErrorInfo?.Message), directStudentRes.Message);
            }
            var directStudents = directStudentRes.Result.Result;

            // get student enrolled that don't have yet attendance
            var studentsDontHaveAttendance = students.Where(s => !attendances.Any(at => at.StudentId == s.Id));
            var directStudentsDontHaveAttendance = directStudents.Where(s => !directStudentAttendance.Any(at => at.StudentId == s.Id));

            bool createStudentsDontHaveAttendance = studentsDontHaveAttendance.Count() > 0 && directStudentsDontHaveAttendance.Count() > 0 && args.ForceCreate;

            // don't have entries yet, then need to create attendance
            if(createStudentsDontHaveAttendance)
            {
                // create student attendance
                DateTime date = DateTime.Now.Date;
                var studentsToCreate = studentsDontHaveAttendance.Select(s => {
                    return new Cinnamon.Framework.ApiCommand.ApiData.StudentAttendance.Request.CreateManyStudentAttendanceArgs.StudentAttendaceDetails {
                        Date = date,
                        IsPresent = false,
                        StudentId = s.Id
                    };
                });

                var createStudentAttendance = await studentAttendanceData.CreateManyStudentAttendance(new Framework.ApiCommand.ApiData.StudentAttendance.Request.CreateManyStudentAttendanceArgs {
                    StudentAttendaces = studentsToCreate
                });
                if(!createStudentAttendance.Succeeded || createStudentAttendance.Result == null || !createStudentAttendance.Result.IsSuccess)
                {
                    return AppResult<GetStudentAttendanceResult>.CreateFailed(
                        new ApplicationException(createStudentAttendance.Result?.ErrorInfo?.Message), createStudentAttendance.Message);
                }

                var directStudentsCreateAttendanceRes = await directStudentData.CreateStudentAttendance(new Framework.ApiCommand.ApiData.DirectStudent.Request.CreateStudentAttendanceArgs {
                    CreateStudentAttendances = directStudentsDontHaveAttendance.Select(s => new Framework.ApiCommand.ApiData.DirectStudent.Request.CreateStudentAttendanceArgs.CreateStudentAttendance {
                        Date = date,
                        IsPresent = false,
                        DirectStudentSessionId = s.Id
                    })
                });
                if(!directStudentsCreateAttendanceRes.Succeeded || directStudentsCreateAttendanceRes.Result is null || 
                    !directStudentsCreateAttendanceRes.Result.IsSuccess)
                {
                    return AppResult<GetStudentAttendanceResult>.CreateFailed(
                        new ApplicationException(directStudentsCreateAttendanceRes.Result?.ErrorInfo?.Message), directStudentsCreateAttendanceRes.Message);
                }

                // fetch again student attendance
                var attendanceResReLoad = await studentAttendanceData.GetAllStudentAttendance(new Framework.ApiCommand.ApiData.StudentAttendance.Request.GetAllStudentAttendanceArgs {
                    ActivityIds = new int[] {args.ActivityId},
                    IsIncludeStudent = true,
                    ScheduleIds = new int[] {args.ScheduleId},
                    Date = args.Date.ToString("yyyyMMdd")
                });
                if(!attendanceResReLoad.Succeeded || attendanceResReLoad.Result == null || !attendanceResReLoad.Result.IsSuccess)
                {
                    return AppResult<GetStudentAttendanceResult>.CreateFailed(new ApplicationException(attendanceResReLoad.Result?.ErrorInfo?.Message), attendanceResReLoad.Message);
                }

                var directStudentAttendanceReload = await directStudentData.StudentAttendance(new Framework.ApiCommand.ApiData.DirectStudent.Request.StudentAttendanceArgs {
                    ActivityIds = new int[] {args.ActivityId},
                    IncludeStudent = true,
                    ScheduleIds = new int[] {args.ScheduleId},
                    Date = args.Date.ToString("yyyyMMdd")
                });
                if(!directStudentAttendanceReload.Succeeded || directStudentAttendanceReload.Result is null || !directStudentAttendanceReload.Result.IsSuccess)
                {
                    return AppResult<GetStudentAttendanceResult>.CreateFailed(
                        new ApplicationException(directStudentAttendanceReload.Result?.ErrorInfo?.Message), directStudentAttendanceReload.Message);
                }

                List<GetStudentAttendanceResult.StudentAttendace> grpStudentAttendances = new();

                grpStudentAttendances.AddRange(attendanceResReLoad.Result.Result.Select(s => new GetStudentAttendanceResult.StudentAttendace {
                    ActivityDescription = activity.Description,
                    ActivityId = activity.Id,
                    ActivityTitle = activity.Title,
                    IsPresent = s.IsPresent,
                    ScheduleDescription = schedule.Name,
                    ScheduleId = schedule.Id,
                    ScheduleTitle = schedule.DateTime,
                    StudentId = s.StudentId,
                    NumberOfSessions = s.Student.NumberOfSessions,
                    SessionsAttended = s.Student.SessionsAttended,
                    Status = s.Student.Status,
                    StudentName = s.Student.Name,
                    StudentNo = s.Student.StudentNo,
                    AttendanceDate = s.Date,
                    Id = s.Id,
                    Remarks = s.Student.Remarks,
                    ExpirationDateStart = s.Student.ExpirationStartDate,
                    ExpirationDateEnd = s.Student.ExpirationEndDate,
                    StudentType = Framework.Enums.StudentType.Cinnamon
                }));

                grpStudentAttendances.AddRange(directStudentAttendanceReload.Result.Result.Select(s => new GetStudentAttendanceResult.StudentAttendace {
                    ActivityDescription = activity.Description,
                    ActivityId = activity.Id,
                    ActivityTitle = activity.Title,
                    IsPresent = s.IsPresent,
                    ScheduleDescription = schedule.Name,
                    ScheduleId = schedule.Id,
                    ScheduleTitle = schedule.DateTime,
                    StudentId = s.StudentId,
                    NumberOfSessions = s.Student.NumberOfSessions,
                    SessionsAttended = s.Student.SessionsAttended,
                    Status = s.Student.Status,
                    StudentName = s.Student.Name,
                    StudentNo = s.Student.StudentNo,
                    AttendanceDate = s.Date,
                    Id = s.Id,
                    Remarks = s.Student.Remarks,
                    StudentType = Framework.Enums.StudentType.Manual
                }));

                return AppResult<GetStudentAttendanceResult>.CreateSucceeded(new GetStudentAttendanceResult {
                    StudentAttendaces = grpStudentAttendances
                }, "Successfullt get student attendance");
            }

            List<GetStudentAttendanceResult.StudentAttendace> grpStudentAttendancesNoCreate = new();

            grpStudentAttendancesNoCreate.AddRange(attendances.Select(s => new GetStudentAttendanceResult.StudentAttendace {
                ActivityDescription = activity.Description,
                ActivityId = activity.Id,
                ActivityTitle = activity.Title,
                IsPresent = s.IsPresent,
                ScheduleDescription = schedule.Name,
                ScheduleId = schedule.Id,
                ScheduleTitle = schedule.DateTime,
                StudentId = s.StudentId,
                NumberOfSessions = s.Student.NumberOfSessions,
                SessionsAttended = s.Student.SessionsAttended,
                Status = s.Student.Status,
                StudentName = s.Student.Name,
                StudentNo = s.Student.StudentNo,
                AttendanceDate = s.Date,
                Id = s.Id,
                Remarks = s.Student.Remarks,
                ExpirationDateStart = s.Student.ExpirationStartDate,
                ExpirationDateEnd = s.Student.ExpirationEndDate,
                StudentType = Framework.Enums.StudentType.Cinnamon
            }));

            grpStudentAttendancesNoCreate.AddRange(directStudentAttendance.Select(s => new GetStudentAttendanceResult.StudentAttendace {
                ActivityDescription = activity.Description,
                ActivityId = activity.Id,
                ActivityTitle = activity.Title,
                IsPresent = s.IsPresent,
                ScheduleDescription = schedule.Name,
                ScheduleId = schedule.Id,
                ScheduleTitle = schedule.DateTime,
                StudentId = s.StudentId,
                NumberOfSessions = s.Student.NumberOfSessions,
                SessionsAttended = s.Student.SessionsAttended,
                Status = s.Student.Status,
                StudentName = s.Student.Name,
                StudentNo = s.Student.StudentNo,
                AttendanceDate = s.Date,
                Id = s.Id,
                Remarks = s.Student.Remarks,
                StudentType = Framework.Enums.StudentType.Manual
            }));

            return AppResult<GetStudentAttendanceResult>.CreateSucceeded(new GetStudentAttendanceResult {
                StudentAttendaces = grpStudentAttendancesNoCreate
            }, "Successfullt get student attendance");
        }
        catch (Exception ex)
        {
            return AppResult<GetStudentAttendanceResult>.CreateFailed(ex, "An error occured in GetStudentAttendanceHandler");
        }
    }
}