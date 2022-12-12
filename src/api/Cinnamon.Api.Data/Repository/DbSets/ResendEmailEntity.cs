using Microsoft.EntityFrameworkCore;
using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Repository.Interfaces;
using Cinnamon.Framework.Common;
using Cinnamon.Api.Data.Extensions;

namespace Cinnamon.Api.Data.Repository.DbSets;

public class ResendEmailEntity : GenericEntity<ResendEmail>, IResendEmail
{
    private readonly ApplicationContext applicationContext;

    public ResendEmailEntity(ApplicationContext applicationContext)
        : base(applicationContext)
    {
        this.applicationContext = applicationContext;
    }

    public async Task<AppResult<IEnumerable<ResendEmail>>> GetByEmailDateRange(string email, DateTime from, DateTime to)
    {
        try
        {
            // set kindutc for postgres date
            from = from.SetKindUtc();
            to = to.SetKindUtc();

            var result = await applicationContext.ResendEmails.Where(r => r.Email == email && r.DateResend >= from && r.DateResend <= to).ToListAsync();
            if(result == null)
            {
                return AppResult<IEnumerable<ResendEmail>>.CreateFailed(new ApplicationException("Can't find resend email"), "Can't find resend email");
            }

            return AppResult<IEnumerable<ResendEmail>>.CreateSucceeded(result, "Successfully getting resend emails");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<ResendEmail>>.CreateFailed(ex, "An error occured when getting resend email by email and date range");
        }
    }
}