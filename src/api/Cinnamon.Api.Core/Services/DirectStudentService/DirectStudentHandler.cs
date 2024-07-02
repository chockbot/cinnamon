using AutoMapper;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Api.Core.Services.DirectStudentService.Handlers;
using Cinnamon.Api.Core.Services.DirectStudentService.Interactors;
using Cinnamon.Api.Core.Services.DirectStudentService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.DirectStudentService;

public class DirectStudentHandler : IDirectStudentHandler
{
    private readonly IDirectStudentData directStudentData;
    private readonly IGetProfileHandler getProfileHandler;
    private readonly IMapper mapper;

    public DirectStudentHandler(IDirectStudentData directStudentData, IGetProfileHandler getProfileHandler, IMapper mapper)
    {
        this.directStudentData = directStudentData;
        this.getProfileHandler = getProfileHandler;
        this.mapper = mapper;
    }
    
    public AppResult<DirectStudentResult> Execute(DirectStudentArgs args)
    {
        return ExecuteAsync(args).Result;
    }

    public async Task<AppResult<DirectStudentResult>> ExecuteAsync(DirectStudentArgs args)
    {
        try
        {
            var profileRes = await getProfileHandler.ExecuteAsync(new AccountService.Interactors.GetProfileArgs {});
            if(!profileRes.Succeeded || profileRes.Result is null)
            {
                return AppResult<DirectStudentResult>.CreateFailed(new ApplicationException(profileRes.Message), profileRes.Message);
            }

            var studentRes = await directStudentData.DirectStudentInfo(args.StudentId);
            if(!studentRes.Succeeded || studentRes.Result is null || !studentRes.Result.IsSuccess)
            {
                return AppResult<DirectStudentResult>.CreateFailed(new ApplicationException(studentRes.Result?.ErrorInfo?.Message), studentRes.Message);
            }

            if(profileRes.Result.Id != studentRes.Result.Result.DirectStudentInfo.ProviderId)
            {
                return AppResult<DirectStudentResult>.CreateFailed(
                    new ApplicationException("Action not allowed. Invalid request."), "Action not allowed. Invalid request.");
            }
            var student = studentRes.Result.Result;

            var result = new DirectStudentResult 
            {
                DirectStudentInfoResult = mapper.Map<DirectStudentResult.DirectStudentInfo>(student.DirectStudentInfo),
                DirectStudentPaymentResult = mapper.Map<DirectStudentResult.DirectStudentPayment>(student.DirectStudentPayment),
                DirectStudentSessionResult = mapper.Map<DirectStudentResult.DirectStudentSession>(student.DirectStudentSession)
            };

            return AppResult<DirectStudentResult>.CreateSucceeded(result, "Successfully get direct student");
        }
        catch (Exception ex)
        {
            return AppResult<DirectStudentResult>.CreateFailed(ex, "An error occured in DirectStudentHandler.");
        }
    }
}