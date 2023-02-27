using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinnamon.Framework.ApiCommand.ApiData.Location.Request
{
    public class GetAllBarangayArgs
    {
        public int? PageIndex { get; set; }
        public int? CountPerPage { get; set; }
        public string CityCode { get; set; }
    }
}
