using Cinnamon.Core;
using Cinnamon.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Cinnamon.Data;

public class ResendEmail : BaseDbSet<ResendEmailModel>, IResendEmail
{
    public ResendEmail(DataStoreDbContext dbContext) : base(dbContext) { }
    protected override DbSet<ResendEmailModel> Table => mDbContext.ResendEmails;

    public async Task<ResendEmailModel> GetResendByEmailAsync(string email)
        => await mDbContext.ResendEmails.FirstOrDefaultAsync(i => i.Email == email);
}