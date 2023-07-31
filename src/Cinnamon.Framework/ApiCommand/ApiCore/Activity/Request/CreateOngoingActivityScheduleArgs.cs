using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinnamon.Framework.ApiCommand.ApiCore.Activity.Request
{
    public class CreateOngoingActivityScheduleArgs
    {
        public DateTime ScheduleDate { get; set; }
        public int ActivityScheduleTimeId { get; set; }
        public int PurchaseOrderId { get; set; }
        public bool IsCompleted { get; set; }
    }
}
