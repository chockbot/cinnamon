using AutoMapper;
using Cinnamon.Api.Data.Repository.Interfaces;
using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.TokenGenerated;
using Cinnamon.Framework.Common;
using Entities = Cinnamon.Api.Data.Repository.Entities;

namespace Cinnamon.Api.Data.Services.Repository.TokenGenerated;

public class TokenGeneratedRepository : ITokenGeneratedRepository
{
    private readonly IDataStore dataStore;
    private readonly IMapper mapper;

    public TokenGeneratedRepository(IDataStore dataStore, IMapper mapper)
    {
        this.dataStore = dataStore;
        this.mapper = mapper;
    }

    public async Task<AppResult<TokenGeneratedDTO>> CreateTokenGenearted(string tokenType, string guid, string token, string payload)
    {
        try
        {
            var entity = new Entities.TokenGenerated {
                Guid = guid,
                Payload = payload,
                Token = token,
                TokenType = tokenType
            };
            
            var createRes = await dataStore.TokenGenerated.Add(entity);
            if(!createRes.Succeeded || createRes.Result is null)
            {
                return AppResult<TokenGeneratedDTO>.CreateFailed(new ApplicationException(createRes.Message), createRes.Message);
            }

            var resultData = mapper.Map<TokenGeneratedDTO>(createRes.Result);
            return AppResult<TokenGeneratedDTO>.CreateSucceeded(resultData, "Token generated successfully saved.");
        }
        catch (Exception ex)
        {
            return AppResult<TokenGeneratedDTO>.CreateFailed(ex, "An error occured when creating token generated.");
        }
    }

    public async Task<AppResult<TokenGeneratedDTO>> GetTokenGenerated(string guid, string token)
    {
        try
        {
            var result = await dataStore.TokenGenerated.FindFirstAsync(t => t.Token == token && t.Guid == guid);
            if(!result.Succeeded || result.Result is null)
            {
                return AppResult<TokenGeneratedDTO>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }

            var resultData = mapper.Map<TokenGeneratedDTO>(result.Result);
            return AppResult<TokenGeneratedDTO>.CreateSucceeded(resultData, "Token generated successfully get");
        }
        catch (Exception ex)
        {
            return AppResult<TokenGeneratedDTO>.CreateFailed(ex, "An error occured when getting token generated.");
        }
    }
}