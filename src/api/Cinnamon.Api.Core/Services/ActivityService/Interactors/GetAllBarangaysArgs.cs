using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.ActivityService.Interactors
{
    public class GetAllBarangaysArgs : IInteractor
    {
        public string CityCode { get; set; }
        public int? CountPerPage { get; set; }
    }
}
