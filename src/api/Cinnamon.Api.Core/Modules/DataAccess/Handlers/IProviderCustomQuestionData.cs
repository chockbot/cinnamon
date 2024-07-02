using Cinnamon.Framework.ApiCommand.ApiData.ProviderCustomQuestion.Request;
using Cinnamon.Framework.ApiCommand.ApiData.ProviderCustomQuestion.Response;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Modules.DataAccess.Handlers;

public interface IProviderCustomQuestionData
{
    Task<AppResult<CreateCustomQuestionResult>> CreateCustomQuestion(CreateCustomQuestionArgs args);

    Task<AppResult<GetCustomQuestionsResult>> GetCustomQuestions(GetCustomQuestionsArgs args);
}