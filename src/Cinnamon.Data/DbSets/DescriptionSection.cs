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
    public class DescriptionSection : BaseDbSet<DescriptionSectionModel>, IDescriptions
    {
        public DescriptionSection(DataStoreDbContext dbContext) : base(dbContext) { }
        protected override DbSet<DescriptionSectionModel> Table => mDbContext.Descriptions;
    }
}
