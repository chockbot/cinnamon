using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinnamon.Framework.ApiCommand.ApiData.Favorite.Request
{
    public class RemoveFavoriteArgs
    {
        public int CustomerId { get; set; }
        public int ActivityId { get; set; }
    }
}
