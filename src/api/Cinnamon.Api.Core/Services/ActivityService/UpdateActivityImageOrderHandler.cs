using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Interactors;
using Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.ActivityService;

public class UpdateActivityImageOrderHandler : IUpdateActivityImageOrderHandler
{
    private readonly IGetOwnedActivityHandler getOwnedActivityHandler;
    private readonly IActivityImagesData activityImagesData;

    public UpdateActivityImageOrderHandler(IGetOwnedActivityHandler getOwnedActivityHandler, IActivityImagesData activityImagesData)
    {
        this.getOwnedActivityHandler = getOwnedActivityHandler;
        this.activityImagesData = activityImagesData;
    }

    public AppResult<UpdateActivityImageOrderResult> Execute(UpdateActivityImageOrderArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<UpdateActivityImageOrderResult>.CreateFailed(ex, "An error occured in UpdateActivityImageOrderHandler");
        }
    }

    public async Task<AppResult<UpdateActivityImageOrderResult>> ExecuteAsync(UpdateActivityImageOrderArgs args)
    {
        try
        {
            var ownedActivity = await getOwnedActivityHandler.ExecuteAsync(new GetOwnedActivityArgs {ActivityId = args.ActivityId, IncludeActivityImages = true});
            if(!ownedActivity.Succeeded || ownedActivity.Result == null)
            {
                return AppResult<UpdateActivityImageOrderResult>.CreateFailed(new ApplicationException(ownedActivity.Message), ownedActivity.Message);
            }

            var images = ownedActivity.Result.Images.OrderBy(i => i.Order).ToList();
            args.ImageOrders = args.ImageOrders.OrderBy(i => i.OldOrder).ToList();

            // check if have images
            if(images.Count == 0 || images.Count != args.ImageOrders.Count) 
            {
                return AppResult<UpdateActivityImageOrderResult>.CreateFailed(new ApplicationException("Unable to update by order."), "Unable to update by order.");
            }

            int counter = 1;
            foreach(var item in args.ImageOrders)
            {
                var image = images.FirstOrDefault(i => i.Order == item.OldOrder);
                if(image != null)
                {
                    image.Order = counter;
                    counter++;
                }
            }

            var updated = await activityImagesData.UpdateManyActivityImage(new Framework.ApiCommand.ApiData.ActivityImage.Request.UpdateManyActivityImageArgs {
                Images = images.Select(i => {
                    return new Framework.ApiCommand.ApiData.ActivityImage.Request.UpdateManyActivityImageArgs.UpdateImage {
                        ActivityId = args.ActivityId,
                        Id = i.Id,
                        ImageName = i.Name,
                        ImageSrc = i.ImageSrc,
                        Order = i.Order
                    };
                })
            });

            if(!updated.Succeeded || updated.Result == null || !updated.Result.IsSuccess)
            {
                return AppResult<UpdateActivityImageOrderResult>.CreateFailed(new ApplicationException(updated.Result?.ErrorInfo?.Message), updated.Message);
            }
            var updatedResult = updated.Result.Result.OrderBy(i => i.Order).ToList();

            return AppResult<UpdateActivityImageOrderResult>.CreateSucceeded(new UpdateActivityImageOrderResult {
                Image1Path = updatedResult.Count >= 1 ? updatedResult[0].ImageLocation : string.Empty,
                Image2Path = updatedResult.Count >= 2 ? updatedResult[1].ImageLocation : string.Empty,
                Image3Path = updatedResult.Count >=3 ? updatedResult[2].ImageLocation : string.Empty
            }, "Successfully update activity image order");
        }
        catch (Exception ex)
        {
            return AppResult<UpdateActivityImageOrderResult>.CreateFailed(ex, "An error occured in UpdateActivityImageOrderHandler");
        }
    }
}