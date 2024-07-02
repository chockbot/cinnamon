using System.Linq.Expressions;
using AutoMapper;
using Cinnamon.Api.Data.Repository.Interfaces;
using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.ProviderCustomQuestion;
using Cinnamon.Framework.Common;
using Entities = Cinnamon.Api.Data.Repository.Entities;

namespace Cinnamon.Api.Data.Services.Repository.ProviderCustomQuestion;

public class ProviderCustomQuestionRepository : IProviderCustomQuestionRepository
{
    private readonly IDataStore dataStore;
    private readonly IMapper mapper;

    public ProviderCustomQuestionRepository(IDataStore dataStore, IMapper mapper)
    {
        this.dataStore = dataStore;
        this.mapper = mapper;
    }

    public async Task<AppResult<ProviderCustomQuestionDTO>> CreateCustomQuestion(ProviderCustomQuestionDTO dto)
    {
        try
        {
            var entity = mapper.Map<Entities.ProviderCustomQuestion>(dto);

            var result = await dataStore.ProviderCustomQuestion.Add(entity);
            if(!result.Succeeded || result.Result is null)
            {
                return AppResult<ProviderCustomQuestionDTO>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }

            dto = mapper.Map<ProviderCustomQuestionDTO>(result.Result);
            return AppResult<ProviderCustomQuestionDTO>.CreateSucceeded(dto, "Provider custom question successfully created.");
        }
        catch (Exception ex)
        {
            return AppResult<ProviderCustomQuestionDTO>.CreateFailed(ex, "An error occured when creating custom question.");
        }
    }

    public async Task<AppResult<IEnumerable<ProviderCustomQuestionDTO>>> GetCustomQuestions(int? providerId, int? activityId)
    {
        try
        {
            Expression<Func<Entities.ProviderCustomQuestion, bool>> filter = p => 
                (activityId.HasValue ? p.ActivityId == activityId.Value : true) &&
                (providerId.HasValue ? p.ProviderId == providerId.Value : true);
            
            var result = await dataStore.ProviderCustomQuestion.FindAsync(filter);
            if(!result.Succeeded || result.Result is null)
            {
                return AppResult<IEnumerable<ProviderCustomQuestionDTO>>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }

            var dto = mapper.Map<IEnumerable<ProviderCustomQuestionDTO>>(result.Result);
            return AppResult<IEnumerable<ProviderCustomQuestionDTO>>.CreateSucceeded(dto, "Successfully get custom questions.");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<ProviderCustomQuestionDTO>>.CreateFailed(ex, "An error occured when getting custom questions.");
        }
    }
}