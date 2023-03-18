using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Modules.DataAccess.Location;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Interactors;
using Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.ActivityService
{
    public class GetAllCitiesHandler : IGetAllCitiesHandler
    {
        private readonly ICityData cityData;
        public GetAllCitiesHandler(ICityData cityData)
        {
            this.cityData = cityData;
        }
        public AppResult<GetAllCitiesResult> Execute(GetAllCitiesArgs args)
        {
            try
            {
                return ExecuteAsync(args).Result;
            }
            catch (Exception ex)
            {
                return AppResult<GetAllCitiesResult>.CreateFailed(ex, "An error occured in GetAllCitiesHandler");
            }
        }

        public async Task<AppResult<GetAllCitiesResult>> ExecuteAsync(GetAllCitiesArgs interactor)
        {
            try
            {
                var result = await cityData.GetAllCitiesByRegionCode(new Framework.ApiCommand.ApiData.Location.Request.GetAllCitiesArgs()
                {
                    CountPerPage = interactor.CountPerPage,
                    PageIndex = 1,
                    RegionCode = interactor.RegionCode
                });

                if (!result.Succeeded || result.Result == null)
                {
                    return AppResult<GetAllCitiesResult>.CreateFailed(new ApplicationException(result.Message), result.Message);
                }
                if (result.Succeeded && !result.Result.IsSuccess)
                {
                    return AppResult<GetAllCitiesResult>.CreateFailed(
                        new ApplicationException(result.Result.ErrorInfo?.Message), "An error occured in GetAllCitiesHandler");
                }
                return AppResult<GetAllCitiesResult>.CreateSucceeded(new GetAllCitiesResult
                {
                    Cities = result.Result.Result.Select(r => new GetAllCitiesResult.City
                    {
                        Code = r.Code,
                        Name = r.Name,
                        RegionCode = r.RegionCode
                    })
                }, "Successfully get all cities");
            }
            catch (Exception ex)
            {
                return AppResult<GetAllCitiesResult>.CreateFailed(ex, "An error occured in GetAllCitiesHandler");
            }
        }
    }
}
