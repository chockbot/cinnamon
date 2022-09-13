using Cinnamon.Core;
using Microsoft.EntityFrameworkCore;

namespace Cinnamon.Data
{
    public class WaitLists : BaseDbSet<WaitListModel>, IWaitList
    {
        public WaitLists(DataStoreDbContext dbContext) : base(dbContext) { }
        protected override DbSet<WaitListModel> Table => mDbContext.WaitLists;
    }
}
