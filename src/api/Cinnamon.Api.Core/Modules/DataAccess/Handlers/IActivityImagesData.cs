using Cinnamon.Framework.ApiCommand.ApiData.ActivityImage.Request;
using Cinnamon.Framework.ApiCommand.ApiData.ActivityImage.Response;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Modules.DataAccess.Handlers;

public interface IActivityImagesData
{
    Task<AppResult<GetActivityImageResult>> GetActivityImageById(int id);
    Task<AppResult<GetAllActivityImagesResult>> GetAllActivityImages(GetAllActivityImagesArgs args);
    Task<AppResult<CreateActivityImageResult>> CreateActivityImage(CreateActivityImageArgs args);
    Task<AppResult<UpdateActivityImageResult>> UpdateActivityImage(UpdateActivityImageArgs args);
    Task<AppResult<CreateManyActivityImageResult>> CreateManyActivityImage(CreateManyActivityImageArgs args);
    Task<AppResult<UpdateManyActivityImageResult>> UpdateManyActivityImage(UpdateManyActivityImageArgs args);
    Task<AppResult<RemoveActivityImagesResult>> RemoveActivityImages(RemoveActivityImageArgs args);
}
