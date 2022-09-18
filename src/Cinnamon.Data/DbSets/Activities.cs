using Cinnamon.Core;
using Microsoft.EntityFrameworkCore;

namespace Cinnamon.Data
{
    public class Activities : BaseDbSet<ActivityModel>, IActivities
    {
        public Activities(DataStoreDbContext dbContext) : base(dbContext) { }
        protected override DbSet<ActivityModel> Table => mDbContext.Activities;
    }
}
