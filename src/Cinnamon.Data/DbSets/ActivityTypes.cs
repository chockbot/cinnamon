using Cinnamon.Core;
using Microsoft.EntityFrameworkCore;

namespace Cinnamon.Data
{
    public class ActivityTypes : BaseDbSet<ActivityTypeModel>, IActivityTypes
    {
        public ActivityTypes(DataStoreDbContext dbContext) : base(dbContext) { }
        protected override DbSet<ActivityTypeModel> Table => mDbContext.ActivityTypes;
    }
}
