using Cinnamon.Api.Data.Repository.Interfaces;
using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.ExternalLoginToken;
using Cinnamon.Framework.Common;
using Cinnamon.Api.Data.Extensions;
using Entity = Cinnamon.Api.Data.Repository.Entities;

namespace Cinnamon.Api.Data.Services.Repository.ExternalLoginToken;

public class ExternalLoginTokenRepository : IExternalLoginTokenRepository
{
    private readonly IDataStore dataStore;

    public ExternalLoginTokenRepository(IDataStore dataStore)
    {
        this.dataStore = dataStore;
    }

    public async Task<AppResult<ExternalLoginTokenDTO>> CreateTokenAsync(string token, string guid, string email, 
        DateTime dateGenerated, string firstName, string lastName)
    {
        try
        {
            if(string.IsNullOrEmpty(token))
            {
                return AppResult<ExternalLoginTokenDTO>.CreateFailed(
                    new ApplicationException("Token can't be empty"), "Token can't be empty");
            }

            var entity = new Entity.ExternalLoginToken {
                IsUsed = false,
                DateGenerated = dateGenerated.SetKindUtc(),
                Token = token,
                Email = email,
                Guid = guid,
                FirstName = firstName,
                LastName = lastName
            };

            var result = await dataStore.ExternalLoginToken.Add(entity);
            if(!result.Succeeded || result.Result == null)
            {
                return AppResult<ExternalLoginTokenDTO>.CreateFailed(
                    new ApplicationException("An error occured when creating token"), "An error occured when creating token");
            }

            return AppResult<ExternalLoginTokenDTO>.CreateSucceeded(new ExternalLoginTokenDTO {
                DateGenerated = result.Result.DateGenerated,
                Id = result.Result.Id,
                IsUsed = result.Result.IsUsed,
                Token = result.Result.Token,
                Email = result.Result.Email,
                Guid = result.Result.Guid,
                FirstName = result.Result.FirstName,
                LastName = result.Result.LastName
            }, "Successfully saved external login token");
        }
        catch (Exception ex)
        {
            return AppResult<ExternalLoginTokenDTO>.CreateFailed(ex, "An error occured in ExternalLoginTokenRepository");
        }
    }

    public async Task<AppResult<ExternalLoginTokenDTO>> GetByTokenAsync(string token, string guid)
    {
        try
        {
            var result = await dataStore.ExternalLoginToken.FindFirstAsync(t => t.Token == token && t.Guid == guid);
            if(!result.Succeeded || result.Result == null)
            {
                return AppResult<ExternalLoginTokenDTO>.CreateFailed(new ApplicationException("Can't find requested token"), "Can't find requested token");
            }

            return AppResult<ExternalLoginTokenDTO>.CreateSucceeded(new ExternalLoginTokenDTO {
                DateGenerated = result.Result.DateGenerated,
                Id = result.Result.Id,
                IsUsed = result.Result.IsUsed,
                Token = result.Result.Token,
                Email = result.Result.Email,
                FirstName = result.Result.FirstName,
                LastName = result.Result.LastName
            }, "Successfully get token");
        }
        catch (Exception ex)
        {
            return AppResult<ExternalLoginTokenDTO>.CreateFailed(ex, "An error occured in ExternalLoginTokenRepository");
        }
    }

    public async Task<AppResult<ExternalLoginTokenDTO>> UpdateTokenAsync(int id, bool isUsed)
    {
        try
        {
            // ccheck token if existed
            var checkToken = await dataStore.ExternalLoginToken.GetByIdAsync(id);
            if(!checkToken.Succeeded || checkToken.Result == null)
            {
                return AppResult<ExternalLoginTokenDTO>.CreateFailed(new ApplicationException("Can't find requested token"), "Can't find requested token");
            }
            var loginToken = checkToken.Result;

            loginToken.IsUsed = isUsed;

            var updated = await dataStore.ExternalLoginToken.Update(loginToken);
            if(!updated.Succeeded || updated.Result == null)
            {
                return AppResult<ExternalLoginTokenDTO>.CreateFailed(
                    new ApplicationException("An error occured when updating token"), "An error occured when updating token");
            }

            return AppResult<ExternalLoginTokenDTO>.CreateSucceeded(new ExternalLoginTokenDTO {
                DateGenerated = updated.Result.DateGenerated,
                Id = updated.Result.Id,
                IsUsed = updated.Result.IsUsed,
                Token = updated.Result.Token,
                Email = updated.Result.Email,
                FirstName = updated.Result.FirstName,
                LastName = updated.Result.LastName
            }, "Successfully update token"); 
        }
        catch (Exception ex)
        {
            return AppResult<ExternalLoginTokenDTO>.CreateFailed(ex, "An error occured in ExternalLoginTokenRepository");
        }
    }
}