using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Cinnamon.Framework.Enums
{
    public partial class Enums
    {
        public enum PriceType
        {
            [Display(Name = "Pay To Reserve")]
            PayToReserve = 1,
            [Display(Name = "Reserve Only")]
            ReserveOnly = 2,
        }
    }
}
