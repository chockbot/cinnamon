using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinnamon.Framework.ApiCommand.ApiCore.Account.Request
{
    public class UpdateRequestRefundArgs
    {
        public int RefundId { get; set; }
        public int Status { get; set; }
        public decimal? RefundAmountGiven { get; set; }
    }
}
