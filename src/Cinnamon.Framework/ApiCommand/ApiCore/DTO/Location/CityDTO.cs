using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinnamon.Framework.ApiCommand.ApiCore.DTO.Location
{
    public class CityDTO
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string RegionCode { get; set; }
        public bool IsCity { get; set; }
        public bool IsMunicipality { get; set; }
    }
}
