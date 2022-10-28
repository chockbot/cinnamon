using Cinnamon.Core;
using Microsoft.EntityFrameworkCore;

namespace Cinnamon.Data
{
    public class UserList : BaseDbSet<UserListModel>, IUser
    {
        public UserList(DataStoreDbContext dbContext) : base(dbContext) { }
        protected override DbSet<UserListModel> Table => mDbContext.UserList;
    }
}
