using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.FailedLogin;
using Cinnamon.Framework.Common;
using Cinnamon.Api.Data.Extensions;
using Entity = Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Repository.Interfaces;

namespace Cinnamon.Api.Data.Services.Repository.FailedLogin;

public class FailedLoginRepository : IFailedLoginRepository
{
    private readonly IDataStore dataStore;

    public FailedLoginRepository(IDataStore dataStore)
    {
        this.dataStore = dataStore;
    }
    
    public async Task<AppResult<FailedLoginDTO>> CreateAsync(string email, string metadata, DateTime loginDate)
    {
        try
        {
            if(string.IsNullOrEmpty(email))
            {
                return AppResult<FailedLoginDTO>.CreateFailed(new ApplicationException("Required email field"), "Required email field");
            }

            var entity = new Entity.FailedLogin {
                Email = email,
                LoginDate = loginDate.SetKindUtc(),
                Metadata = metadata
            };

            var result = await dataStore.FailedLogin.Add(entity);
            if(!result.Succeeded || result.Result == null)
            {
                return AppResult<FailedLoginDTO>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }

            return AppResult<FailedLoginDTO>.CreateSucceeded(new FailedLoginDTO {
                Email = result.Result.Email,
                Id = result.Result.Id,
                LoginDate = result.Result.LoginDate
            }, "Successfully create failed logins");
        }
        catch (Exception ex)
        {
            return AppResult<FailedLoginDTO>.CreateFailed(ex, "An error occured when creating failed logins");
        }
    }

    public async Task<AppResult<IEnumerable<FailedLoginDTO>>> GetFailedLogins(string email, DateTime from, DateTime to)
    {
        try
        {
            var result = await dataStore.FailedLogin.GetFailedLogins(email, from, to);
            if(!result.Succeeded || result.Result == null)
            {
                return AppResult<IEnumerable<FailedLoginDTO>>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }

            return AppResult<IEnumerable<FailedLoginDTO>>.CreateSucceeded(result.Result.Select(l => {
                return new FailedLoginDTO {
                    Email = l.Email,
                    Id = l.Id,
                    LoginDate = l.LoginDate
                };
            }), "Successfully get failed logins");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<FailedLoginDTO>>.CreateFailed(ex, "An error occured when getting failed logins");
        }
    }
}