using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Modules.DataAccess.Location;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Interactors;
using Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.ActivityService
{
    public class GetAllBarangaysHandler : IGetAllBarangaysHandler
    {
        private readonly IBarangayData barangayData;
        public GetAllBarangaysHandler(IBarangayData barangayData)
        {
            this.barangayData = barangayData;
        }

        public AppResult<GetAllBarangaysResult> Execute(GetAllBarangaysArgs args)
        {
            try
            {
                return ExecuteAsync(args).Result;
            }
            catch (Exception ex)
            {
                return AppResult<GetAllBarangaysResult>.CreateFailed(ex, "An error occured in GetAllBarangaysHandler");
            }
        }

        public async Task<AppResult<GetAllBarangaysResult>> ExecuteAsync(GetAllBarangaysArgs args)
        {
            try
            {
                var result = await barangayData.GetAllBarangaysByCityCode(new Framework.ApiCommand.ApiData.Location.Request.GetAllBarangayArgs()
                {
                    CountPerPage = args.CountPerPage,
                    PageIndex = 1,
                    CityCode = args.CityCode,
                    IsCity = args.IsCity,
                    IsMunicipality = args.IsMunicipality
                });

                if (!result.Succeeded || result.Result == null)
                {
                    return AppResult<GetAllBarangaysResult>.CreateFailed(new ApplicationException(result.Message), result.Message);
                }
                if (result.Succeeded && !result.Result.IsSuccess)
                {
                    return AppResult<GetAllBarangaysResult>.CreateFailed(
                        new ApplicationException(result.Result.ErrorInfo?.Message), "An error occured in GetAllBarangaysHandler");
                }
                return AppResult<GetAllBarangaysResult>.CreateSucceeded(new GetAllBarangaysResult
                {
                    Barangays = result.Result.Result.Select(r => new GetAllBarangaysResult.Barangay
                    {
                        Code = r.Code,
                        Name = r.Name,
                        CityCode = r.CityCode
                    })
                }, "Successfully get all barangays");
            }
            catch (Exception ex)
            {
                return AppResult<GetAllBarangaysResult>.CreateFailed(ex, "An error occured in GetAllBarangaysHandler");
            }
        }
    }
}
