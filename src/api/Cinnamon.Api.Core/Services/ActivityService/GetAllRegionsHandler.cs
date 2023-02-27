using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Interactors;
using Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.ActivityService
{
    public class GetAllRegionsHandler : IGetAllRegionsHandler
    {
        private readonly IRegionData regionData;
        public GetAllRegionsHandler(IRegionData regionData)
        {
            this.regionData = regionData;
        }
        public AppResult<GetAllRegionsResult> Execute(GetAllRegionsArgs args)
        {
            try
            {
                return ExecuteAsync(args).Result;
            }
            catch (Exception ex)
            {
                return AppResult<GetAllRegionsResult>.CreateFailed(ex, "An error occured in GetAllRegionsHandler");
            }
        }

        public async Task<AppResult<GetAllRegionsResult>> ExecuteAsync(GetAllRegionsArgs interactor)
        {
            try
            {
                var result = await regionData.GetAllRegions(new Framework.ApiCommand.ApiData.Location.Request.GetAllRegionArgs()
                {
                    CountPerPage = 100,
                    PageIndex = 1
                });

                if (!result.Succeeded || result.Result == null)
                {
                    return AppResult<GetAllRegionsResult>.CreateFailed(new ApplicationException(result.Message), result.Message);
                }
                if (result.Succeeded && !result.Result.IsSuccess)
                {
                    return AppResult<GetAllRegionsResult>.CreateFailed(
                        new ApplicationException(result.Result.ErrorInfo?.Message), "An error occured in GetAllRegionsHandler");
                }
                return AppResult<GetAllRegionsResult>.CreateSucceeded(new GetAllRegionsResult
                {
                    Regions = result.Result.Result.Select(r => new GetAllRegionsResult.Region
                    {
                        Code = r.Code,
                        Name = r.Name,
                        RegionName = r.RegionName
                    })
                }, "Successfully get all regions");
            }
            catch (Exception ex)
            {
                return AppResult<GetAllRegionsResult>.CreateFailed(ex, "An error occured in GetAllRegionsHandler");
            }
        }
    }
}
