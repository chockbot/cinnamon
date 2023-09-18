using Cinnamon.Api.Core.Config;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Framework.ApiCommand.ApiData.ActivityImage.Request;
using Cinnamon.Framework.ApiCommand.ApiData.ActivityImage.Response;
using Cinnamon.Framework.Common;
using Flurl.Http;
using Flurl.Http.Configuration;

namespace Cinnamon.Api.Core.Modules.DataAccess.ActivityImages;

public class ActivityImagesData : IActivityImagesData
{
    private readonly IFlurlClient flurlClient;

	public ActivityImagesData(ApplicationConfig config, IFlurlClientFactory flurlFac)
	{
        flurlClient = flurlFac.Get(config.ApiDataUrl);
    }

    public async Task<AppResult<CreateActivityImageResult>> CreateActivityImage(CreateActivityImageArgs args)
    {
        try
        {
            var result = await flurlClient
                .Request("ActivityImage/CreateActivityImage")
                .PostJsonAsync(args)
                .ReceiveJson<CreateActivityImageResult>();

            return AppResult<CreateActivityImageResult>.CreateSucceeded(result, "Successfully posting create activity image api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<CreateActivityImageResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<CreateActivityImageResult>.CreateFailed(ex, "An error occured when posting create activity image api");
        }
    }

    public async Task<AppResult<GetActivityImageResult>> GetActivityImageById(int id)
    {
        try
        {
            var result = await flurlClient
                            .Request($"ActivityImage/GetActivityImageById/{id}")
                            .GetJsonAsync<GetActivityImageResult>();

            return AppResult<GetActivityImageResult>.CreateSucceeded(result, "Successfully getting activity image by id api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetActivityImageResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetActivityImageResult>.CreateFailed(ex, "An error occured when getting activity image by id api");
        }
    }

    public async Task<AppResult<GetAllActivityImagesResult>> GetAllActivityImages(GetAllActivityImagesArgs args)
    {
        try
        {
            var result = await flurlClient
                            .Request("ActivityImages/GetAllActivityImages")
                            .GetJsonAsync<GetAllActivityImagesResult>();

            return AppResult<GetAllActivityImagesResult>.CreateSucceeded(result, "Successfully getting get all activity image api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetAllActivityImagesResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetAllActivityImagesResult>.CreateFailed(ex, "An error occured when getting all activity image api");
        }
    }

    public async Task<AppResult<UpdateActivityImageResult>> UpdateActivityImage(UpdateActivityImageArgs args)
    {
        try
        {
            var result = await flurlClient
                            .Request("ActivityImage/UpdateActivityImage")
                            .PostJsonAsync(args)
                            .ReceiveJson<UpdateActivityImageResult>();

            return AppResult<UpdateActivityImageResult>.CreateSucceeded(result, "Successfully posting update activity image api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<UpdateActivityImageResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<UpdateActivityImageResult>.CreateFailed(ex, "An error occured when posting update activity image api");
        }
    }

    public async Task<AppResult<CreateManyActivityImageResult>> CreateManyActivityImage(CreateManyActivityImageArgs args)
    {
        try
        {
            var result = await flurlClient
                            .Request("ActivityImage/CreateManyActivityImage")
                            .PostJsonAsync(args)
                            .ReceiveJson<CreateManyActivityImageResult>();
            
            return AppResult<CreateManyActivityImageResult>.CreateSucceeded(result, "Successfully posting create many activity images");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<CreateManyActivityImageResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<CreateManyActivityImageResult>.CreateFailed(ex, "An error occured when posting create many activity images");
        }
    }

    public async Task<AppResult<UpdateManyActivityImageResult>> UpdateManyActivityImage(UpdateManyActivityImageArgs args)
    {
        try
        {
            var result = await flurlClient
                            .Request("ActivityImage/UpdateManyActivityImage")
                            .PostJsonAsync(args)
                            .ReceiveJson<UpdateManyActivityImageResult>();
            
            return AppResult<UpdateManyActivityImageResult>.CreateSucceeded(result, "Successfully posting update many activity images");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<UpdateManyActivityImageResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<UpdateManyActivityImageResult>.CreateFailed(ex, "An error occured when posting update many activity images");
        }
    }

    public async Task<AppResult<RemoveActivityImagesResult>> RemoveActivityImages(RemoveActivityImageArgs args)
    {
        try
        {
            var result = await flurlClient
                            .Request("ActivityImage/RemoveActivityImages")
                            .PostJsonAsync(args)
                            .ReceiveJson<RemoveActivityImagesResult>();
            
            return AppResult<RemoveActivityImagesResult>.CreateSucceeded(result, "Successfully posting remove activity images");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<RemoveActivityImagesResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<RemoveActivityImagesResult>.CreateFailed(ex, "An error occured when posting remove activity images");
        }
    }

    public async Task<AppResult<RemoveMultipleIdsResult>> RemoveMultipleIds(RemoveMultipleIdsArgs args)
    {
        try
        {
            var result = await flurlClient
                            .Request("ActivityImage/RemoveMultipleIds")
                            .PostJsonAsync(args)
                            .ReceiveJson<RemoveMultipleIdsResult>();
            
            return AppResult<RemoveMultipleIdsResult>.CreateSucceeded(result, "Successfully posting remove activity images");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<RemoveMultipleIdsResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<RemoveMultipleIdsResult>.CreateFailed(ex, "An error occured when posting remove activity images");
        }
    }
}
