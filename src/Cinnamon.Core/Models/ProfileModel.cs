using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinnamon.Core
{
    public class ProfileModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string ImageName { get; set; } = "";
        public long ImageSize { get; set; }
        public string ImageLocation { get; set; } = "";
    }
}
