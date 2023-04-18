using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.DashboardService.Handlers;
using Cinnamon.Api.Core.Services.DashboardService.Interactors;
using Cinnamon.Api.Core.Services.DashboardService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.DashboardService;
public class GetAllBadgeHandler: IGetAllBadgesHandler
{
    private readonly IBadgesData badgesData;
    public GetAllBadgeHandler(IBadgesData badgesData)
    {
        this.badgesData = badgesData;   
    }

    public AppResult<GetAllBadgeResult> Execute(GetAllBadgeArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<GetAllBadgeResult>.CreateFailed(ex, "An error occured in GetAllBadgeHandler");
        }
    }

    public async Task<AppResult<GetAllBadgeResult>> ExecuteAsync(GetAllBadgeArgs args)
    {
        try
        {
            var result = await badgesData.GetAllBadges(new Framework.ApiCommand.ApiData.BadgeList.Request.GetAllBadgeArgs { });
            if (!result.Succeeded || result.Result == null)
            {
                return AppResult<GetAllBadgeResult>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }

            if (result.Succeeded && !result.Result.IsSuccess)
            {
                return AppResult<GetAllBadgeResult>.CreateFailed(
                    new ApplicationException(result.Result.ErrorInfo?.Message), "An error occured in GetAllBadgeHandler");
            }

            return AppResult<GetAllBadgeResult>.CreateSucceeded(new GetAllBadgeResult
            {
                Badges = result.Result.Result.Select(e => {
                    return new GetAllBadgeResult.Badge
                    {
                        Id = e.Id,
                        Name = e.Name,
                        Description = e.Description,
                        NumberOfStudent = e.NumberOfStudent,
                        NumberOfCompleted = e.NumberOfCompleted,
                        NumberOfReviews = e.NumberOfReviews,
                        ImgScr = e.ImgScr,
                    };
                })
            }, "Successfully get experience categories");
        }
        catch (Exception ex)
        {
            return AppResult<GetAllBadgeResult>.CreateFailed(ex, "An error occured in GetAllBadgeHandler");
        }
    }
}
