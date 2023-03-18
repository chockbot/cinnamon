using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinnamon.Framework.ApiCommand.ApiCore.Activity.Request
{
    public class GetAllCitiesArgs
    {
        public string RegionCode { get; set; }
        public int? CountPerPage { get; set; }
    }
}
