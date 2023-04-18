using Cinnamon.Api.Data.Repository.Interfaces;
using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.ResetPassword;
using Cinnamon.Framework.Common;
using Entities = Cinnamon.Api.Data.Repository.Entities;

namespace Cinnamon.Api.Data.Services.Repository.ResetPassword;

public class ResetPasswordRepository : IResetPasswordRepository
{
    private readonly IDataStore dataStore;

    public ResetPasswordRepository(IDataStore dataStore)
    {
        this.dataStore = dataStore;
    }
    
    public async Task<AppResult<ResetPasswordDTO>> Create(string email, string guid, string token, bool isUsed, string generatedToken)
    {
        try
        {
            var resetPasswordEntity = new Entities.ResetPassword {
                Email = email,
                Guid = guid,
                IsUsed = isUsed,
                Token = token,
                GeneratedToken = generatedToken
            };

            var result = await dataStore.ResetPassword.Add(resetPasswordEntity);
            if(!result.Succeeded || result.Result == null)
            {
                return AppResult<ResetPasswordDTO>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }
            var created = result.Result;

            return AppResult<ResetPasswordDTO>.CreateSucceeded(new ResetPasswordDTO {
                Email = created.Email,
                Id = created.Id,
                IsUsed = created.IsUsed,
                GeneratedToken = created.GeneratedToken
            }, "Successfully create reset password");
        }
        catch (Exception ex)
        {
            return AppResult<ResetPasswordDTO>.CreateFailed(ex, "An error occured when creating reset password");
        }
    }

    public async Task<AppResult<IEnumerable<ResetPasswordDTO>>> GetAllAsync(int? count, int? skip)
    {
        try
        {
            var result = await dataStore.ResetPassword.FindAsync(e => true, count, skip);
            if(!result.Succeeded || result.Result == null)
            {
                return AppResult<IEnumerable<ResetPasswordDTO>>.CreateFailed(result.Error.Exception, result.Message);
            }

            var resetPasswords = result.Result.Select(r =>
            {
                return new ResetPasswordDTO
                {
                    Email = r.Email,
                    Id = r.Id,
                    IsUsed = r.IsUsed,
                    GeneratedToken = r.GeneratedToken
                };
            });

            return AppResult<IEnumerable<ResetPasswordDTO>>.CreateSucceeded(resetPasswords, "Successfully get all reset passwords");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<ResetPasswordDTO>>.CreateFailed(ex, "An error occured when getting resetted passwords");
        }
    }

    public async Task<AppResult<IEnumerable<ResetPasswordDTO>>> GetAllAsync()
    {
        try
        {
            var result = await dataStore.ResetPassword.GetAllAsync();
            if(!result.Succeeded || result.Result == null)
            {
                return AppResult<IEnumerable<ResetPasswordDTO>>.CreateFailed(result.Error.Exception, result.Message);
            }

            var resetPasswords = result.Result.Select(r =>
            {
                return new ResetPasswordDTO
                {
                    Email = r.Email,
                    Id = r.Id,
                    IsUsed = r.IsUsed,
                    GeneratedToken = r.GeneratedToken
                };
            });

            return AppResult<IEnumerable<ResetPasswordDTO>>.CreateSucceeded(resetPasswords, "Successfully get all reset passwords");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<ResetPasswordDTO>>.CreateFailed(ex, "An error occured when getting all resetted paswords");
        }
    }

    public async Task<AppResult<ResetPasswordDTO>> GetByGuidTokenAsync(string guid, string token)
    {
        try
        {
            var result = await dataStore.ResetPassword.FindFirstAsync(r => r.Guid == guid && r.Token == token);
            if(!result.Succeeded || result.Result == null)
            {
                return AppResult<ResetPasswordDTO>.CreateFailed(result.Error.Exception, result.Message);
            }
            var resetPassword = result.Result;

            return AppResult<ResetPasswordDTO>.CreateSucceeded(new ResetPasswordDTO {
                Email = resetPassword.Email,
                Id = resetPassword.Id,
                IsUsed = resetPassword.IsUsed,
                GeneratedToken = resetPassword.GeneratedToken
            }, "Successfuly get reset pasword");
        }
        catch (Exception ex)
        {
            return AppResult<ResetPasswordDTO>.CreateFailed(ex, "An error occured when getting reset pasword by guid and token");
        }
    }

    public async Task<AppResult<ResetPasswordDTO>> GetByIdAsync(int id)
    {
        try
        {
            var result = await dataStore.ResetPassword.GetByIdAsync(id);
            if(!result.Succeeded || result.Result == null)
            {
                return AppResult<ResetPasswordDTO>.CreateFailed(result.Error.Exception, result.Message);
            }
            var resetPassword = result.Result;

            return AppResult<ResetPasswordDTO>.CreateSucceeded(new ResetPasswordDTO {
                Email = resetPassword.Email,
                Id = resetPassword.Id,
                IsUsed = resetPassword.IsUsed,
                GeneratedToken = resetPassword.GeneratedToken
            }, "Successfuly get reset pasword");
        }
        catch (Exception ex)
        {
            return AppResult<ResetPasswordDTO>.CreateFailed(ex, "An error occured when getting reset password by id");
        }
    }

    public async Task<AppResult<ResetPasswordDTO>> Update(int id, bool isUsed)
    {
        try
        {
            var result = await dataStore.ResetPassword.GetByIdAsync(id);
            if(!result.Succeeded || result.Result == null)
            {
                return AppResult<ResetPasswordDTO>.CreateFailed(result.Error.Exception, result.Message);
            }
            result.Result.IsUsed = isUsed;

            var updatedRes = await dataStore.ResetPassword.Update(result.Result);
            if(!updatedRes.Succeeded || updatedRes.Result == null)
            {
                return AppResult<ResetPasswordDTO>.CreateFailed(updatedRes.Error.Exception, updatedRes.Message);
            }

            return AppResult<ResetPasswordDTO>.CreateSucceeded(new ResetPasswordDTO {
                Email = updatedRes.Result.Email,
                Id = updatedRes.Result.Id,
                IsUsed = updatedRes.Result.IsUsed,
                GeneratedToken = updatedRes.Result.GeneratedToken
            }, "Successfully update reset password");
        }
        catch (Exception ex)
        {
            return AppResult<ResetPasswordDTO>.CreateFailed(ex, "An error occured when updating reset password");
        }
    }
}