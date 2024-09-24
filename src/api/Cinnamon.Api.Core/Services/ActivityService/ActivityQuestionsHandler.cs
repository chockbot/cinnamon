using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Interactors;
using Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.ActivityService;

public class ActivityQuestionsHandler : IActivityQuestionsHandler
{
    private readonly IProviderCustomQuestionData providerCustomQuestionData;
    private readonly IGetActivityHandler getActivityHandler;

    public ActivityQuestionsHandler(IProviderCustomQuestionData providerCustomQuestionData,
        IGetActivityHandler getActivityHandler)
    {
        this.getActivityHandler = getActivityHandler;
        this.providerCustomQuestionData = providerCustomQuestionData;
    }
    
    public AppResult<ActivityQuestionsResult> Execute(ActivityQuestionsArgs args)
    {
        return ExecuteAsync(args).Result;
    }

    public async Task<AppResult<ActivityQuestionsResult>> ExecuteAsync(ActivityQuestionsArgs args)
    {
        try
        {
            var getActivityRes = await getActivityHandler.ExecuteAsync(new GetActivityArgs {
                ActivityId = args.ActivityId,
                IncludeCustomer = true
            });
            if(!getActivityRes.Succeeded || getActivityRes.Result is null)
            {
                return AppResult<ActivityQuestionsResult>.CreateFailed(new ApplicationException(getActivityRes.Message), getActivityRes.Message);
            }
            var activity = getActivityRes.Result;

            var activityQuestions = await providerCustomQuestionData.GetCustomQuestions(new Framework.ApiCommand.ApiData.ProviderCustomQuestion.Request.GetCustomQuestionsArgs {
                ActivityId = activity.Id,
                ProviderId = activity.Owner?.Id ?? 0
            });
            if(!activityQuestions.Succeeded || activityQuestions.Result is null || !activityQuestions.Result.IsSuccess)
            {
                return AppResult<ActivityQuestionsResult>.CreateFailed(new ApplicationException(activityQuestions.Result?.ErrorInfo?.Message), activityQuestions.Message);
            }

            var questions = activityQuestions.Result.Result.Select(q => new ActivityQuestionsResult.Question {
                ActivitId  = q.ActivityId,
                FieldLabel = q.FieldLabel,
                FieldType  = q.FieldType,
                Id         = q.Id,
                Required   = q.Required,
                Options    = q.Options
            });

            return AppResult<ActivityQuestionsResult>.CreateSucceeded(new ActivityQuestionsResult {Questions = questions}, "Successfully get activity questions.");
        }
        catch (Exception ex)
        {
            return AppResult<ActivityQuestionsResult>.CreateFailed(ex, "An error occured in ActivityQuestionsHandler.");
        }
    }
}