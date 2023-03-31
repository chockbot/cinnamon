using Cinnamon.Framework.ApiCommand.ApiData.DTO.Barangay;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.City;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.Region;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Data.Services.Repository.Interfaces
{
    public interface ILocationRepository
    {
        Task<AppResult<IEnumerable<RegionDTO>>> GetAllRegionsAsync();
        Task<AppResult<IEnumerable<CityDTO>>> GetAllCitiesByRegionAsync(string regionCode, int? count);
        Task<AppResult<IEnumerable<BarangayDTO>>> GetAllBarangaysByCityAsync(string cityCode, bool isCity, bool isMunicipality, int? count);
    }
}
