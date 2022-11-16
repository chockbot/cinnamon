using Cinnamon.Core;
using Microsoft.EntityFrameworkCore;

namespace Cinnamon.Data
{
    public class WaitLists : BaseDbSet<WaitListModel>, IWaitList
    {
        public WaitLists(DataStoreDbContext dbContext) : base(dbContext) { }
        protected override DbSet<WaitListModel> Table => mDbContext.WaitLists;

        public async Task<WaitListModel> GetWaitListByGuid(string guid)
            => await mDbContext.WaitLists.FirstOrDefaultAsync(i => i.Guid == guid);
    }
}
