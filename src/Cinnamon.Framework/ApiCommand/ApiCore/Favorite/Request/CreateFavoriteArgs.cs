using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinnamon.Framework.ApiCommand.ApiCore.Favorite.Request
{
    public class CreateFavoriteArgs
    {
        public int CustomerId { get; set; }
        public int ActivityId { get; set; }
    }
}
