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
    public class SearchTags : BaseDbSet<SearchTagsModel>, ISearchTags
    {
        public SearchTags(DataStoreDbContext dbContext) : base(dbContext) { }
        protected override DbSet<SearchTagsModel> Table => mDbContext.SearchTags;
    }
}
