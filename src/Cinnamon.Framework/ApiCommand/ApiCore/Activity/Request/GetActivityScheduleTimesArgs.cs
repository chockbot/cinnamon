using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinnamon.Framework.ApiCommand.ApiCore.Activity.Request
{
    public class GetActivityScheduleTimesArgs
    {
        public int ActivityScheduleId { get; set; }
        public int DayOfWeek { get; set; }
        public DateTime ScheduleDate { get; set; }
    }
}
