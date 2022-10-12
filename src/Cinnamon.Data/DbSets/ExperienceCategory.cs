using Cinnamon.Core;
using Cinnamon.Core.DI.Interfaces.Tables;
using Cinnamon.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinnamon.Data.DbSets
{
    public class ExperienceCategory : BaseDbSet<ExperienceCategoryModel>, IExperienceCategory
    {
        public ExperienceCategory(DataStoreDbContext dbContext) : base(dbContext) { }
        protected override DbSet<ExperienceCategoryModel> Table => mDbContext.ExperienceCategories;
    }
}
