using Cinnamon.Api.Data.Repository.Interfaces;
using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.Barangay;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.City;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.Region;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Data.Services.Repository.Location
{
    public class LocationRepository: ILocationRepository
    {
        private readonly IDataStore dataStore;

        public LocationRepository(IDataStore dataStore)
        {
            this.dataStore = dataStore;
        }

        public async Task<AppResult<IEnumerable<BarangayDTO>>> GetAllBarangaysByCityAsync(string cityCode, bool isCity, bool isMunicipality, int? count)
        {
            try
            {
                var result = await dataStore.Barangay.FindAsync(c => (isCity ? c.CityCode == cityCode : true) &&
                                                                     (isMunicipality ? c.MunicipalityCode == cityCode : true), count);

                if (!result.Succeeded || result.Result == null)
                {
                    return AppResult<IEnumerable<BarangayDTO>>.CreateFailed(result.Error.Exception, result.Message);
                }

                var barangays = result.Result.Select(r => new BarangayDTO
                {
                    Code = r.Code,
                    Name = r.Name,
                    CityCode = isCity ? r.CityCode : r.MunicipalityCode
                });

                return AppResult<IEnumerable<BarangayDTO>>.CreateSucceeded(barangays, "Successfully get barangays");
            }
            catch (Exception ex)
            {
                return AppResult<IEnumerable<BarangayDTO>>.CreateFailed(ex, "An error occured when getting barangays");
            }
        }

        public async Task<AppResult<IEnumerable<CityDTO>>> GetAllCitiesByRegionAsync(string regionCode, int? count)
        {
            try
            {
                var result = await dataStore.City.FindAsync(c => c.RegionCode == regionCode, count);

                if (!result.Succeeded || result.Result == null)
                {
                    return AppResult<IEnumerable<CityDTO>>.CreateFailed(result.Error.Exception, result.Message);
                }

                var regions = result.Result.Select(r => new CityDTO
                {
                    Code           = r.Code,
                    Name           = r.Name,
                    RegionCode     = r.RegionCode,
                    IsCity         = r.IsCity,
                    IsMunicipality = r.IsMunicipality
                });

                return AppResult<IEnumerable<CityDTO>>.CreateSucceeded(regions, "Successfully get regions");
            }
            catch (Exception ex)
            {
                return AppResult<IEnumerable<CityDTO>>.CreateFailed(ex, "An error occured when getting regions");
            }
        }

        public async Task<AppResult<IEnumerable<RegionDTO>>> GetAllRegionsAsync()
        {
            try
            {
                var result = await dataStore.Region.GetAllAsync();

                if (!result.Succeeded || result.Result == null)
                {
                    return AppResult<IEnumerable<RegionDTO>>.CreateFailed(result.Error.Exception, result.Message);
                }

                var regions = result.Result.Select(r => new RegionDTO
                {
                    Code = r.Code,
                    Name = r.Name,
                    RegionName = r.RegionName,
                });

                return AppResult<IEnumerable<RegionDTO>>.CreateSucceeded(regions, "Successfully get regions");
            }
            catch (Exception ex)
            {
                return AppResult<IEnumerable<RegionDTO>>.CreateFailed(ex, "An error occured when getting regions");
            }
        }
    }
}
