using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.ActivityService.Interactors
{
    public class GetAllBarangaysArgs : IInteractor
    {
        public string CityCode { get; set; }
        public int? CountPerPage { get; set; }
        public bool IsCity { get; set; }
        public bool IsMunicipality { get; set; }
    }
}
