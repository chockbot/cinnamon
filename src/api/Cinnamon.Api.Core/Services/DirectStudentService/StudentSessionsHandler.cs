using AutoMapper;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Api.Core.Services.DirectStudentService.Handlers;
using Cinnamon.Api.Core.Services.DirectStudentService.Interactors;
using Cinnamon.Api.Core.Services.DirectStudentService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.DirectStudentService;

public class StudentSessionsHandler : IStudentSessionsHandler
{
    private readonly IDirectStudentHandler directStudentHandler;
    private readonly IDirectStudentData directStudentData;
    private readonly IMapper mapper;

    public StudentSessionsHandler(IDirectStudentHandler directStudentHandler, IDirectStudentData directStudentData, IMapper mapper)
    {
        this.directStudentHandler = directStudentHandler;
        this.directStudentData = directStudentData;
        this.mapper = mapper;
    }

    public AppResult<StudentSessionsResult> Execute(StudentSessionsArgs args)
    {
        return ExecuteAsync(args).Result;
    }

    public async Task<AppResult<StudentSessionsResult>> ExecuteAsync(StudentSessionsArgs args)
    {
        try
        {
            var studentInfoRes = await directStudentHandler.ExecuteAsync(new DirectStudentArgs {
                StudentId = args.StudentId
            });

            if(!string.IsNullOrEmpty(args.SessionStatus) && (args.SessionStatus.ToLower() != "ongoing" && args.SessionStatus.ToLower() != "completed") )
            {
                return AppResult<StudentSessionsResult>.CreateFailed(new ApplicationException("Invalid sessions status."), "Invalid sessions status.");
            }

            var studentSessionsRes = await directStudentData.StudentSessions(new Framework.ApiCommand.ApiData.DirectStudent.Request.StudentSessionsArgs {
                SessionStatus = args.SessionStatus
            }, args.StudentId);
            if(!studentSessionsRes.Succeeded || studentSessionsRes.Result is null || !studentSessionsRes.Result.IsSuccess)
            {
                return AppResult<StudentSessionsResult>.CreateFailed(new ApplicationException(studentSessionsRes.Result?.ErrorInfo?.Message), studentSessionsRes.Message);
            }

            var result = mapper.Map<IEnumerable<StudentSessionsResult.DirectStudentSession>>(studentSessionsRes.Result.Result);

            return AppResult<StudentSessionsResult>.CreateSucceeded(new StudentSessionsResult {DirectStudentSessions = result}, "Successfully get direct student sessions.");
        }
        catch (Exception ex)
        {   
            return AppResult<StudentSessionsResult>.CreateFailed(ex, "An error occured in StudentSessionsHandler.");
        }
    }
}