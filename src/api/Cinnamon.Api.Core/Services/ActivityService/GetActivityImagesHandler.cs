using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Interactors;
using Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.ActivityService;
public class GetActivityImagesHandler: IGetActivityImagesHandler
{
	private readonly IActivityImagesData activityImagesData;
	public GetActivityImagesHandler(IActivityImagesData activityImagesData)
	{
		this.activityImagesData = activityImagesData;	
	}

    public AppResult<GetActivityImagesResult> Execute(GetActivityImagesArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<GetActivityImagesResult>.CreateFailed(ex, "An error occured in GetActivityImagesHandler");
        }
    }

    public async Task<AppResult<GetActivityImagesResult>> ExecuteAsync(GetActivityImagesArgs args)
    {
        try
        {
            var result = await activityImagesData.GetAllActivityImages(new Framework.ApiCommand.ApiData.ActivityImage.Request.GetAllActivityImagesArgs { });
            if (!result.Succeeded || result.Result == null)
            {
                return AppResult<GetActivityImagesResult>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }

            if (result.Succeeded && !result.Result.IsSuccess)
            {
                return AppResult<GetActivityImagesResult>.CreateFailed(
                    new ApplicationException(result.Result.ErrorInfo?.Message), "An error occured in GetActivityImagesHandler");
            }

            return AppResult<GetActivityImagesResult>.CreateSucceeded(new GetActivityImagesResult
            {
                ActivityImages = result.Result.Result.OrderBy(i => i.Order).Select(e => {
                    return new GetActivityImagesResult.ActivityImage
                    {
                        ActivityId = e.ActivityId,
                        ImageName = e.ImageName,    
                        ImageLocation =e.ImageLocation,
                        Id = e.Id,
                        Order = e.Order
                    };
                })
            }, "Successfully get activity images");
        }
        catch (Exception ex)
        {
            return AppResult<GetActivityImagesResult>.CreateFailed(ex, "An error occured in GetActivityImagesHandler");
        }
    }
}
