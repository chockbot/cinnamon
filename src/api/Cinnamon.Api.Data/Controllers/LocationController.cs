using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiData;
using Cinnamon.Framework.ApiCommand.ApiData.Location.Request;
using Cinnamon.Framework.ApiCommand.ApiData.Location.Response;
using Cinnamon.Framework.ApiCommand.ApiData.Student.Response;
using Microsoft.AspNetCore.Mvc;

namespace Cinnamon.Api.Data.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly ILocationRepository _locationRepository;

        public LocationController(ILocationRepository locationRepository)
        {
            _locationRepository = locationRepository;
        }

        [Route("Regions")]
        [HttpGet]
        [ProducesResponseType(typeof(GetAllRegionResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllRegions([FromQuery] GetAllRegionArgs args)
        {
            try
            {
                var result = await _locationRepository.GetAllRegionsAsync();

                if (!result.Succeeded || result.Result == null)
                {
                    return new JsonResult(new GetAllRegionResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
                }

                var totalRecords = result.Result.Count();
                return new JsonResult(new GetAllRegionResult
                {
                    Result = result.Result,
                    IsSuccess = true,
                    Pagination = new Pagination
                    {
                        PageIndex = args.PageIndex,
                        PerPage = args.CountPerPage,
                        TotalRecords = totalRecords,
                        TotalPages = args.CountPerPage.HasValue && args.PageIndex.HasValue ?
                                (int)Math.Ceiling((double)totalRecords / args.CountPerPage.Value) : null
                    }
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new GetAllRegionResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
            }
        }

        [Route("Cities")]
        [HttpGet]
        [ProducesResponseType(typeof(GetAllCitiesResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllCitiesByRegionCode([FromQuery] GetAllCitiesArgs args)
        {
            try
            {
                var result = await _locationRepository.GetAllCitiesByRegionAsync(args.RegionCode);

                if (!result.Succeeded || result.Result == null)
                {
                    return new JsonResult(new GetAllCitiesResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
                }

                var totalRecords = result.Result.Count();
                return new JsonResult(new GetAllCitiesResult
                {
                    Result = result.Result,
                    IsSuccess = true,
                    Pagination = new Pagination
                    {
                        PageIndex = args.PageIndex,
                        PerPage = args.CountPerPage,
                        TotalRecords = totalRecords,
                        TotalPages = args.CountPerPage.HasValue && args.PageIndex.HasValue ?
                                (int)Math.Ceiling((double)totalRecords / args.CountPerPage.Value) : null
                    }
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new GetAllCitiesResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
            }
        }

        [Route("barangays")]
        [HttpGet]
        [ProducesResponseType(typeof(GetAllBarangayResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllBarangaysByCityCode([FromQuery] GetAllBarangayArgs args)
        {
            try
            {
                var result = await _locationRepository.GetAllBarangaysByCityAsync(args.CityCode);

                if (!result.Succeeded || result.Result == null)
                {
                    return new JsonResult(new GetAllBarangayResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
                }

                var totalRecords = result.Result.Count();
                return new JsonResult(new GetAllBarangayResult
                {
                    Result = result.Result,
                    IsSuccess = true,
                    Pagination = new Pagination
                    {
                        PageIndex = args.PageIndex,
                        PerPage = args.CountPerPage,
                        TotalRecords = totalRecords,
                        TotalPages = args.CountPerPage.HasValue && args.PageIndex.HasValue ?
                                (int)Math.Ceiling((double)totalRecords / args.CountPerPage.Value) : null
                    }
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new GetAllBarangayResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
            }
        }
    }
}
    