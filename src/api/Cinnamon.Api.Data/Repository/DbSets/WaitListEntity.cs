using Microsoft.EntityFrameworkCore;
using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Repository.Interfaces;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Data.Repository.DbSets;

public class WaitListEntity : GenericEntity<WaitList>, IWaitList
{
    private readonly ApplicationContext applicationContext;

    public WaitListEntity(ApplicationContext applicationContext)
        : base(applicationContext)
    {
        this.applicationContext = applicationContext;
    }

    public async Task<AppResult<WaitList>> GetWaitListByEmailAsync(string email)
    {
        try
        {
            var result = await applicationContext.WaitLists.FirstOrDefaultAsync(w => w.Email == email);
            if(result == null)
            {
                return AppResult<WaitList>.CreateFailed(new ApplicationException("Can't find waitlist by email"), "Can't find waitlist by email");
            }

            return AppResult<WaitList>.CreateSucceeded(result, "Successfully find waitlist by email");
        }
        catch (Exception ex)
        {
            return AppResult<WaitList>.CreateFailed(ex, "An error occured when getting waitlist by email");
        }
    }

    public async Task<AppResult<WaitList>> GetWaitListByGuidAsync(string guid)
    {
        try 
        {
            var result = await applicationContext.WaitLists.FirstOrDefaultAsync(w => w.Guid == guid);
            if(result == null)
            {
                return AppResult<WaitList>.CreateFailed(new ApplicationException("Can't find waitlist by guid"), "Can't find waitlist by guid");
            }

            return AppResult<WaitList>.CreateSucceeded(result, "Successfully find waitlist by guid");
        }
        catch (Exception ex)
        {
            return AppResult<WaitList>.CreateFailed(ex, "An error occured when getting waitlist by guid");
        }
    }
}