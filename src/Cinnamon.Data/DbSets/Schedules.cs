using Cinnamon.Core;
using Cinnamon.Core.DI.Interfaces.Tables;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinnamon.Data.DbSets
{
    public class Schedules : BaseDbSet<ScheduleModel>, ISchedules
    {
        public Schedules(DataStoreDbContext dbContext) : base(dbContext) { }
        protected override DbSet<ScheduleModel> Table => mDbContext.Schedules;

        public async Task<ScheduleModel> GetScheduleByIdAsync(int id)
            => await mDbContext.Schedules.FirstOrDefaultAsync(i => i.Id == id);
    }
}
