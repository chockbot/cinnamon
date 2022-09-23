using Cinnamon.Core;
using Microsoft.EntityFrameworkCore;

namespace Cinnamon.Data.DbSets
{
    public class ActivityImages : BaseDbSet<ActivityImagesModels>, IActivityImages
    {
        public ActivityImages(DataStoreDbContext dbContext) : base(dbContext) { }
        protected override DbSet<ActivityImagesModels> Table => mDbContext.ActivityImages;
    }
}
