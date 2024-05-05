using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.AccountService.Interactors
{
    public class GetAllCustomersArgs : IInteractor
    {
        public bool? IsVerified { get; set; }
        public int? PageIndex { get; set; }
        public int? CountPerPage { get; set; }
        public string? HandlerLike { get; set; }
        public string SearchValue { get; set; }
        public bool? IsOfficialPartner {get; set;}
        public bool? HasVerification {get; set;}
    }
}
