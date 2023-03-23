using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.AccountService.Interactors
{
    public class GetAllCustomersArgs : IInteractor
    {
        public bool? IsVerified { get; set; }
        public int? PageIndex { get; set; }
        public int? CountPerPage { get; set; }
        public string? HandlerLike { get; set; }
    }
}
