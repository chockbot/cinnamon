using Cinnamon.Framework.Common;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.ProviderCustomQuestion;

namespace Cinnamon.Api.Data.Services.Repository.Interfaces;

public interface IProviderCustomQuestionRepository
{
    Task<AppResult<ProviderCustomQuestionDTO>> CreateCustomQuestion(ProviderCustomQuestionDTO dto);

    Task<AppResult<IEnumerable<ProviderCustomQuestionDTO>>> GetCustomQuestions(int? providerId, int? activityId);
}