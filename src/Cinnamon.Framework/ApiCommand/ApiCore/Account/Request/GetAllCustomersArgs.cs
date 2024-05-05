using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinnamon.Framework.ApiCommand.ApiCore.Account.Request
{
    public class GetAllCustomersArgs
    {
        public bool? IsVerified { get; set; }
        public int? PageIndex { get; set; }
        public int? CountPerPage { get; set; }
        public string? HandlerLike { get; set; }
        public string? SearchValue { get; set; }
        public bool? IsOfficialPartner {get; set;}
        public bool? HasVerification {get; set;}
    }
}
