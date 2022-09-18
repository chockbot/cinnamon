using Cinnamon.Core;
using Microsoft.EntityFrameworkCore;

namespace Cinnamon.Data
{
    public class ExperienceTypes : BaseDbSet<ExperienceTypeModel>, IExperienceTypes
    {
        public ExperienceTypes(DataStoreDbContext dbContext) : base(dbContext) { }
        protected override DbSet<ExperienceTypeModel> Table => mDbContext.ExperienceTypes;
    }
}
