using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.AdminService.Interactors
{
    public class GetAdminUserByEmailArgs : IInteractor
    {
        public string Email { get; set; }
    }
}
