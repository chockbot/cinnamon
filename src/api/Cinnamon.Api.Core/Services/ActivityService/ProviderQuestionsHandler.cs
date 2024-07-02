using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Interactors;
using Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.ActivityService;

public class ProviderQuestionsHandler : IProviderQuestionsHandler
{
    private readonly IProviderCustomQuestionData providerCustomQuestionData;
    private readonly IGetProfileHandler getProfileHandler;

    public ProviderQuestionsHandler(IProviderCustomQuestionData providerCustomQuestionData, IGetProfileHandler getProfileHandler)
    {
        this.providerCustomQuestionData = providerCustomQuestionData;
        this.getProfileHandler = getProfileHandler;
    }
    
    public AppResult<ProviderQuestionsResult> Execute(ProviderQuestionsArgs args)
    {
        return ExecuteAsync(args).Result;
    }

    public async Task<AppResult<ProviderQuestionsResult>> ExecuteAsync(ProviderQuestionsArgs args)
    {
        try
        {
            var currentUserRes = await getProfileHandler.ExecuteAsync(new AccountService.Interactors.GetProfileArgs {});
            if(!currentUserRes.Succeeded || currentUserRes.Result is null)
            {
                return AppResult<ProviderQuestionsResult>.CreateFailed(new ApplicationException(currentUserRes.Message), currentUserRes.Message);
            }
            var user = currentUserRes.Result;

            var providerCustomQuestionsRes = await providerCustomQuestionData.GetCustomQuestions(new Framework.ApiCommand.ApiData.ProviderCustomQuestion.Request.GetCustomQuestionsArgs {
                ActivityId = args.ActivityId,
                ProviderId = user.Id
            });
            if(!providerCustomQuestionsRes.Succeeded || providerCustomQuestionsRes.Result is null || !providerCustomQuestionsRes.Result.IsSuccess)
            {
                return AppResult<ProviderQuestionsResult>.CreateFailed(
                    new ApplicationException(providerCustomQuestionsRes.Result?.ErrorInfo?.Message), providerCustomQuestionsRes.Message);
            }

            var providerQuestions = providerCustomQuestionsRes.Result.Result.Select(q => new ProviderQuestionsResult.Question {
                FieldLabel = q.FieldLabel,
                FieldType = q.FieldType,
                Required = q.Required,
                ActivitId = q.ActivityId,
                Id = q.Id,
                ProviderId = q.ProviderId
            });

            return AppResult<ProviderQuestionsResult>.CreateSucceeded(new ProviderQuestionsResult {Questions = providerQuestions}, "Successfully get provider questions.");
        }
        catch (Exception ex)
        {
            return AppResult<ProviderQuestionsResult>.CreateFailed(ex, "An error occured in ProviderQuestionsHandler.");
        }
    }
}