using Cinnamon.Core;
using Microsoft.EntityFrameworkCore;

namespace Cinnamon.Data
{
    public class Activities : BaseDbSet<ActivityModel>, IActivities
    {
        public Activities(DataStoreDbContext dbContext) : base(dbContext) { }
        protected override DbSet<ActivityModel> Table => mDbContext.Activities;

        public async Task<ActivityModel> GetActivityByIdAsync(int id)
            => await mDbContext.Activities.FirstOrDefaultAsync(i => i.Id == id);

        public async Task<IList<ActivityModel>> GetActivityByCreatedId(int id)
            => await mDbContext.Activities.Where(i => i.CreatedBy == id).ToListAsync();
    }
}
