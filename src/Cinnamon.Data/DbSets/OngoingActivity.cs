using Cinnamon.Core;
using Cinnamon.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Cinnamon.Data;

public class OngoingActivity : BaseDbSet<OngoingActivityModel>, IOngoingActivity
{
    public OngoingActivity(DataStoreDbContext dbContext) : base(dbContext) { }
    protected override DbSet<OngoingActivityModel> Table => mDbContext.OngoingActivities;
}