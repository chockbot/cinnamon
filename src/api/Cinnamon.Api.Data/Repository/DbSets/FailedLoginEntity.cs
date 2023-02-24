using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Repository.Interfaces;
using Cinnamon.Framework.Common;
using Cinnamon.Api.Data.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Cinnamon.Api.Data.Repository.DbSets;

public class FailedLoginEntity : GenericEntity<FailedLogin>, IFailedLogin
{
    private readonly ApplicationContext applicationContext;

    public FailedLoginEntity(ApplicationContext applicationContext)
        : base(applicationContext)
    {
        this.applicationContext = applicationContext;
    }

    public async Task<AppResult<IEnumerable<FailedLogin>>> GetFailedLogins(string email, DateTime from, DateTime to)
    {
        try
        {
            // set kindutc for postgres date
            from = from.SetKindUtc();
            to = to.SetKindUtc();

            var result = await applicationContext.FailedLogins.Where(l => l.Email == email && l.LoginDate >= from && l.LoginDate <= to).ToListAsync();
            if(result == null)
            {
                return AppResult<IEnumerable<FailedLogin>>.CreateFailed(new ApplicationException("Can't find failed logins"), "Can't find failed logins");
            }

            return AppResult<IEnumerable<FailedLogin>>.CreateSucceeded(result, "Successfully get failed logins");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<FailedLogin>>.CreateFailed(ex, "An error occured when getting failed logins");
        }
    }
}