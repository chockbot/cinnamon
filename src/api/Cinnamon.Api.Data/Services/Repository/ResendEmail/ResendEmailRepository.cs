using Cinnamon.Api.Data.Repository.Interfaces;
using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Entities = Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Framework.Common;
using Cinnamon.Api.Data.Extensions;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.ResendEmail;

namespace Cinnamon.Api.Data.Services.Repository.ResendEmail;

public class ResendEmailRepository : IResendEmailRepository
{
    private readonly IDataStore dataStore;

    public ResendEmailRepository(IDataStore dataStore)
    {
        this.dataStore = dataStore;
    }

    public async Task<AppResult<ResendEmailDTO>> Create(string email, DateTime dateResend)
    {
        try
        {
            dateResend = dateResend.SetKindUtc();

            var resendEmail = new Entities.ResendEmail
            {
                Email = email,
                DateResend = dateResend,
            };

            var resendEmailRes = await dataStore.ResendEmail.Add(resendEmail);
            if(!resendEmailRes.Succeeded || resendEmailRes.Result == null)
            {
                return AppResult<ResendEmailDTO>.CreateFailed(resendEmailRes.Error.Exception, resendEmailRes.Message);
            }

            return AppResult<ResendEmailDTO>.CreateSucceeded(new ResendEmailDTO
            {
                DateResend = resendEmailRes.Result.DateResend,
                Email = resendEmailRes.Result.Email,
                Id = resendEmailRes.Result.Id
            }, "Successfully created resend email");
        }
        catch (Exception ex)
        {
            return AppResult<ResendEmailDTO>.CreateFailed(ex, "An error occured when creating resend email");
        }
    }

    public async Task<AppResult<IEnumerable<ResendEmailDTO>>> GetAllAsync(int? count, int? skip)
    {
        try
        {
            var result = await dataStore.ResendEmail.FindAsync(e => true, count, skip);
            if(!result.Succeeded || result.Result == null)
            {
                return AppResult<IEnumerable<ResendEmailDTO>>.CreateFailed(result.Error.Exception, result.Message);
            }

            var resendEmails = result.Result.Select(r =>
            {
                return new ResendEmailDTO
                {
                    Email = r.Email,
                    DateResend = r.DateResend,
                    Id = r.Id,
                };
            });

            return AppResult<IEnumerable<ResendEmailDTO>>.CreateSucceeded(resendEmails, "Successfully getting resend emails");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<ResendEmailDTO>>.CreateFailed(ex, "An error occured when getting resend emails");
        }
    }

    public async Task<AppResult<IEnumerable<ResendEmailDTO>>> GetAllAsync()
    {
        try
        {
            var result = await dataStore.ResendEmail.GetAllAsync();
            if(!result.Succeeded || result.Result == null)
            {
                return AppResult<IEnumerable<ResendEmailDTO>>.CreateFailed(result.Error.Exception, result.Message);
            }

            var resendEmails = result.Result.Select(r =>
            {
                return new ResendEmailDTO
                {
                    Email = r.Email,
                    DateResend = r.DateResend,
                    Id = r.Id,
                };
            });

            return AppResult<IEnumerable<ResendEmailDTO>>.CreateSucceeded(resendEmails, "Successfully getting resend emails");

        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<ResendEmailDTO>>.CreateFailed(ex, "An error occured when getting email resends");
        }
    }

    public async Task<AppResult<IEnumerable<ResendEmailDTO>>> GetByEmailDateRange(string email, DateTime from, DateTime to)
    {
        try
        {
            // validate date
            if(from > to)
            {
                return AppResult<IEnumerable<ResendEmailDTO>>.CreateFailed(new ApplicationException("Date from must less than to Date to"), "Date from must less than to Date to");
            }

            var result = await dataStore.ResendEmail.GetByEmailDateRange(email, from, to);
            if(!result.Succeeded || result.Result == null)
            {
                return AppResult<IEnumerable<ResendEmailDTO>>.CreateFailed(result.Error.Exception, result.Message);
            }

            var resendEmails = result.Result.Select(r =>
            {
                return new ResendEmailDTO
                {
                    Email = r.Email,
                    DateResend = r.DateResend,
                    Id = r.Id,
                };
            });

            return AppResult<IEnumerable<ResendEmailDTO>>.CreateSucceeded(resendEmails, "Successfully getting resend emails");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<ResendEmailDTO>>.CreateFailed(ex, "An error occured when getting email resend by email and date range");
        }
    }

    public async Task<AppResult<ResendEmailDTO>> GetByIdAsync(int id)
    {
        try
        {
            var result = await dataStore.ResendEmail.GetByIdAsync(id);
            if(!result.Succeeded || result.Result == null)
            {
                return AppResult<ResendEmailDTO>.CreateFailed(result.Error.Exception, result.Message);
            }

            return AppResult<ResendEmailDTO>.CreateSucceeded(new ResendEmailDTO
            {
                DateResend = result.Result.DateResend,
                Email = result.Result.Email,
                Id = result.Result.Id,
            }, "Successfully getting resend email");
        }
        catch (Exception ex)
        {
            return AppResult<ResendEmailDTO>.CreateFailed(ex, "An error occured when getting resend email by id");
        }
    }

    public async Task<AppResult<ResendEmailDTO>> Update(int resendEmailId, string? email, DateTime? dateResend)
    {
        try
        {
            // check email resend if existed
            var emailResendCheck = await dataStore.ResendEmail.GetByIdAsync(resendEmailId);
            if(!emailResendCheck.Succeeded || emailResendCheck.Result == null)
            {
                return AppResult<ResendEmailDTO>.CreateFailed(new ApplicationException("Can't find email resend to update"), "Can't find email resend to update");
            }
            var updatedResendEmail = emailResendCheck.Result;

            updatedResendEmail.Email = email ?? updatedResendEmail.Email;
            updatedResendEmail.DateResend = dateResend ?? updatedResendEmail.DateResend;

            var updatedRes = await dataStore.ResendEmail.Update(updatedResendEmail);
            if(!updatedRes.Succeeded || updatedRes.Result == null)
            {
                return AppResult<ResendEmailDTO>.CreateFailed(new ApplicationException("An error occured when updating resend email"), "An error occured when updating resend email");
            }

            return AppResult<ResendEmailDTO>.CreateSucceeded(new ResendEmailDTO
            {
                DateResend = updatedRes.Result.DateResend,
                Email = updatedRes.Result.Email,
                Id = updatedRes.Result.Id
            }, "Successfully update resend email");
        }
        catch (Exception ex)
        {
            return AppResult<ResendEmailDTO>.CreateFailed(ex, "An error occured when updating resend email by id");
        }
    }
}