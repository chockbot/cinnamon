using Cinnamon.Core;
using Microsoft.EntityFrameworkCore;

namespace Cinnamon.Data
{
    public class Profile : BaseDbSet<ProfileModel>, IProfile
    {
        public Profile(DataStoreDbContext dbContext) : base(dbContext) { }
        protected override DbSet<ProfileModel> Table => mDbContext.Profile;
        public async Task<ProfileModel> GetProfileByIdAsync(int id)
            => await mDbContext.Profile.FirstOrDefaultAsync(i => i.Id == id);
    }
}
