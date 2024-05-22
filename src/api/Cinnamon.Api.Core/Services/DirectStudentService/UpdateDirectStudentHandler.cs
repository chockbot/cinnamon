using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Api.Core.Services.DirectStudentService.Handlers;
using Cinnamon.Api.Core.Services.DirectStudentService.Interactors;
using Cinnamon.Api.Core.Services.DirectStudentService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.DirectStudentService;

public class UpdateDirectStudentHandler : IUpdateDirectStudentHandler
{
    private readonly IDirectStudentData directStudentData;
    private readonly IGetProfileHandler getProfileHandler;

    public UpdateDirectStudentHandler(IGetProfileHandler getProfileHandler, IDirectStudentData directStudentData)
    {
        this.directStudentData = directStudentData;
        this.getProfileHandler = getProfileHandler;
    }
    
    public AppResult<UpdateDirectStudentResult> Execute(UpdateDirectStudentArgs args)
    {
        return ExecuteAsync(args).Result;
    }

    public async Task<AppResult<UpdateDirectStudentResult>> ExecuteAsync(UpdateDirectStudentArgs args)
    {
        try
        {
            var result = await directStudentData.UpdateDirectStudent(new Framework.ApiCommand.ApiData.DirectStudent.Request.UpdateDirectStudentArgs {
                UpdateDirectStudentData = new Framework.ApiCommand.ApiData.DirectStudent.Request.UpdateDirectStudentArgs.UpdateDirectStudent {
                    UpdateDirectStudentInfo = new Framework.ApiCommand.ApiData.DirectStudent.Request.UpdateDirectStudentArgs.UpdateDirectStudentInfo {
                        BirthMonth = args.BirthMonth,
                        BirthYear = args.BirthYear,
                        Gender = args.Gender,
                        Name = args.Name,
                        Id = args.StudentId
                    },
                    UpdateDirectStudentPayment = new Framework.ApiCommand.ApiData.DirectStudent.Request.UpdateDirectStudentArgs.UpdateDirectStudentPayment {
                        Amount = args.Amount
                    },
                    UpdateDirectStudentSession = new Framework.ApiCommand.ApiData.DirectStudent.Request.UpdateDirectStudentArgs.UpdateDirectStudentSession {
                        ActivityId = args.ActivityId,
                        ScheduleId = args.ScheduleId,
                        Name = args.Name
                    }
                }
            });

            if(!result.Succeeded || result.Result is null || !result.Result.IsSuccess)
            {
                return AppResult<UpdateDirectStudentResult>.CreateFailed(new ApplicationException(result.Result?.ErrorInfo?.Message), result.Message);
            }

            return AppResult<UpdateDirectStudentResult>.CreateSucceeded(new UpdateDirectStudentResult {
                ActivityId = args.ActivityId,
                Amount = args.Amount,
                BirthMonth = args.BirthMonth,
                BirthYear = args.BirthYear,
                Gender = args.Gender,
                Id = args.StudentId,
                Name = args.Name,
                ScheduleId = args.ScheduleId
            }, "Successfully update student id");
        }
        catch (Exception ex)
        {
            return AppResult<UpdateDirectStudentResult>.CreateFailed(ex, "An error occured in UpdateDirectStudentHandler.");
        }
    }
}