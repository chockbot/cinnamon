using Cinnamon.Api.Core.Config;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Framework.ApiCommand.ApiData.ProviderCustomQuestion.Request;
using Cinnamon.Framework.ApiCommand.ApiData.ProviderCustomQuestion.Response;
using Cinnamon.Framework.Common;
using Flurl.Http;
using Flurl.Http.Configuration;

namespace Cinnamon.Api.Core.Modules.DataAccess.ProviderCustomQuestion;

public class ProviderCustomQuestionData : IProviderCustomQuestionData
{
    private readonly IFlurlClient flurlClient;

    public ProviderCustomQuestionData(ApplicationConfig config, IFlurlClientFactory flurlFac)
    {
        flurlClient = flurlFac.Get(config.ApiDataUrl);
    }

    public async Task<AppResult<CreateCustomQuestionResult>> CreateCustomQuestion(CreateCustomQuestionArgs args)
    {
        try
        {
            var result = await flurlClient
                .Request("ProviderCustomQuestions")
                .PostJsonAsync(args)
                .ReceiveJson<CreateCustomQuestionResult>();
            return AppResult<CreateCustomQuestionResult>.CreateSucceeded(result, "Successfully posting create custom question api");
        }
        catch (FlurlHttpException ex)
        {
            var error = await ex.GetResponseJsonAsync();
            return AppResult<CreateCustomQuestionResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<CreateCustomQuestionResult>.CreateFailed(ex, "An error occured when posting create custom question api");
        }
    }

    public async Task<AppResult<GetCustomQuestionsResult>> GetCustomQuestions(GetCustomQuestionsArgs args)
    {
        try
        {
            var result = await flurlClient
                .Request("ProviderCustomQuestions")
                .SetQueryParams(args)
                .GetJsonAsync<GetCustomQuestionsResult>();
            return AppResult<GetCustomQuestionsResult>.CreateSucceeded(result, "Successfully posting get custom questions api");
        }
        catch (FlurlHttpException ex)
        {
            var error = await ex.GetResponseJsonAsync();
            return AppResult<GetCustomQuestionsResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetCustomQuestionsResult>.CreateFailed(ex, "An error occured when posting get custom questions api");
        }
    }

    public async Task<AppResult<DeleteCustomQuestionsResult>> DeleteCustomQuestions(DeleteCustomQuestionsArgs args)
    {
        try
        {
            var result = await flurlClient
                .Request("ProviderCustomQuestions/Bulk/Delete")
                .PostJsonAsync(args)
                .ReceiveJson<DeleteCustomQuestionsResult>();
            return AppResult<DeleteCustomQuestionsResult>.CreateSucceeded(result, "Successfully posting delete custom question api");
        }
        catch (FlurlHttpException ex)
        {
            var error = await ex.GetResponseJsonAsync();
            return AppResult<DeleteCustomQuestionsResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<DeleteCustomQuestionsResult>.CreateFailed(ex, "An error occured when posting delete custom question api");
        }
    }

    public async Task<AppResult<UpdateCustomQuestionsResult>> UpdateCustomQuestions(UpdateCustomQuestionsArgs args)
    {
        try
        {
            var result = await flurlClient
                .Request("ProviderCustomQuestions/Bulk/Update")
                .PostJsonAsync(args)
                .ReceiveJson<UpdateCustomQuestionsResult>();
            return AppResult<UpdateCustomQuestionsResult>.CreateSucceeded(result, "Successfully posting update custom question api");
        }
        catch (FlurlHttpException ex)
        {
            var error = await ex.GetResponseJsonAsync();
            return AppResult<UpdateCustomQuestionsResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<UpdateCustomQuestionsResult>.CreateFailed(ex, "An error occured when posting update custom question api");
        }
    }
}