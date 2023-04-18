using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.ActivityService.Interactors
{
    public class GetAllCitiesArgs : IInteractor
    {
        public string RegionCode { get; set; }
        public int? CountPerPage { get; set; }
    }
}
