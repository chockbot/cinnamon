using AutoMapper;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.DashboardService.Handlers;
using Cinnamon.Api.Core.Services.DashboardService.Interactors;
using Cinnamon.Api.Core.Services.DashboardService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.DashboardService;

public class CreateDirectStudentsHandler : ICreateDirectStudentsHandler
{
    private readonly IDirectStudentData directStudentData;
    private readonly IMapper mapper;
    public CreateDirectStudentsHandler(IDirectStudentData directStudentData, IMapper mapper)
    {
        this.mapper            = mapper;
        this.directStudentData = directStudentData;
    }

    public AppResult<CreateDirectStudentsResult> Execute(CreateDirectStudentsArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<CreateDirectStudentsResult>.CreateFailed(ex, "An error occured in CreateDirectStudentsHandler");
        }
    }

    public async Task<AppResult<CreateDirectStudentsResult>> ExecuteAsync(CreateDirectStudentsArgs args)
    {
        try
        {
            var result = await directStudentData.CreateDirectStudents(new Framework.ApiCommand.ApiData.DirectStudent.Request.CreateDirectStudentsArgs
            {
                CreateDirectStudents = args.CreateDirectStudents.Select(s =>
                {
                    return new Framework.ApiCommand.ApiData.DirectStudent.Request.CreateDirectStudentsArgs.CreateDirectStudent
                    {
                        CreateDirectStudentInfo = new Framework.ApiCommand.ApiData.DirectStudent.Request.CreateDirectStudentsArgs.CreateDirectStudentInfo
                        {
                            BirthMonth = s.CreateDirectStudentInfo.BirthMonth,
                            BirthYear  = s.CreateDirectStudentInfo.BirthYear,
                            Gender     = s.CreateDirectStudentInfo.Gender,
                            Name       = s.CreateDirectStudentInfo.Name,
                            ProviderId = s.CreateDirectStudentInfo.ProviderId
                        },
                        CreateDirectStudentSession = new Framework.ApiCommand.ApiData.DirectStudent.Request.CreateDirectStudentsArgs.CreateDirectStudentSession
                        {
                            ActivityId       = s.CreateDirectStudentSession.ActivityId,
                            Name             = s.CreateDirectStudentSession.Name,
                            NumberOfSessions = s.CreateDirectStudentSession.NumberOfSessions,
                            SessionsAttended = s.CreateDirectStudentSession.SessionsAttended,
                            StudentNo        = s.CreateDirectStudentSession.StudentNo,
                            Remarks          = s.CreateDirectStudentSession.Remarks,
                            ScheduleId       = s.CreateDirectStudentSession.ScheduleId,
                            Status           = s.CreateDirectStudentSession.Status
                        },
                        CreateDirectStudentPayment = new Framework.ApiCommand.ApiData.DirectStudent.Request.CreateDirectStudentsArgs.CreateDirectStudentPayment
                        {
                            Amount = s.CreateDirectStudentPayment.Amount
                        } 
                    };
                }).ToList()
            });

            if (!result.Succeeded || result.Result == null)
            {
                return AppResult<CreateDirectStudentsResult>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }
            if (result.Succeeded && !result.Result.IsSuccess)
            {
                return AppResult<CreateDirectStudentsResult>.CreateFailed(
                    new ApplicationException(result.Result.ErrorInfo?.Message), "An error occured in CreateDirectStudentsHandler");
            }
            var directStudentRes = mapper.Map<IEnumerable<CreateDirectStudentsResult.CreateDirectStudent>>(result.Result.Result);
            return AppResult<CreateDirectStudentsResult>.CreateSucceeded(
                new CreateDirectStudentsResult { CreateDirectStudents = directStudentRes}, "Successfully created direct students");
        }
        catch (Exception ex)
        {
            return AppResult<CreateDirectStudentsResult>.CreateFailed(ex, "An error occured in CreateDirectStudentsHandler");
        }
    }
}
