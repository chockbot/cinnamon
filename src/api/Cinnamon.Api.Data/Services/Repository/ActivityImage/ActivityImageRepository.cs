using Cinnamon.Api.Data.Repository.Interfaces;
using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Framework.Common;
using Entities = Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.ActivityImage;

namespace Cinnamon.Api.Data.Services.Repository.ActivityImage;

public class ActivityImageRepository : IActivityImageRepository
{
    private readonly IDataStore dataStore;

    public ActivityImageRepository(IDataStore dataStore)
    {
        this.dataStore = dataStore;
    }
    
    public async Task<AppResult<ActivityImageDTO>> Create(int activityId, string imageName, string imagePath, int order)
    {
        try
        {
            // check if activity existed
            var activityRes = await dataStore.Activity.GetByIdAsync(activityId);
            if(!activityRes.Succeeded || activityRes.Result == null)
            {
                return AppResult<ActivityImageDTO>.CreateFailed(new ApplicationException("Can't find activity id"), "Can't find activity id");
            }

            var activityImage = new Entities.ActivityImage 
            {
                ActivityId = activityId,
                ImageLocation = imagePath,
                ImageName = imageName,
                Order = order
            };

            var result = await dataStore.ActivityImage.Add(activityImage);
            if(!result.Succeeded || result.Result == null)
            {
                return AppResult<ActivityImageDTO>.CreateFailed(result.Error.Exception, result.Message);
            }

            return AppResult<ActivityImageDTO>.CreateSucceeded(new ActivityImageDTO {
                Id = result.Result.Id,
                ImageLocation = imagePath,
                ImageName = imageName
            },"Successfully created activity image");
        }
        catch (Exception ex)
        {
            return AppResult<ActivityImageDTO>.CreateFailed(ex, "An error occured when creating activity image");
        }
    }

    public async Task<AppResult<IEnumerable<ActivityImageDTO>>> GetAllAsync(int? count, int? skip)
    {
        try
        {
            var result = await dataStore.ActivityImage.FindAsync(f => true, count, skip);
            if(!result.Succeeded || result.Result == null)
            {
                return AppResult<IEnumerable<ActivityImageDTO>>.CreateFailed(result.Error.Exception, result.Message);
            }

            var members = result.Result.Select(f => {
                return new ActivityImageDTO {
                    Id = f.Id,
                    ImageLocation = f.ImageLocation,
                    ImageName = f.ImageName
                };
            });

            return AppResult<IEnumerable<ActivityImageDTO>>.CreateSucceeded(members, "Sucessfully getting activity images");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<ActivityImageDTO>>.CreateFailed(ex, "An error occured when getting activity images");
        }
    }

    public async Task<AppResult<IEnumerable<ActivityImageDTO>>> GetAllAsync()
    {
        try
        {
            var result = await dataStore.ActivityImage.GetAllAsync();
            if(!result.Succeeded || result.Result == null)
            {
                return AppResult<IEnumerable<ActivityImageDTO>>.CreateFailed(result.Error.Exception, result.Message);
            }

            var members = result.Result.Select(f => {
                return new ActivityImageDTO {
                    Id = f.Id,
                    ImageLocation = f.ImageLocation,
                    ImageName = f.ImageName
                };
            });

            return AppResult<IEnumerable<ActivityImageDTO>>.CreateSucceeded(members, "Sucessfully getting activity images");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<ActivityImageDTO>>.CreateFailed(ex, "An error occured when getting activity images");
        }
    }

    public async Task<AppResult<ActivityImageDTO>> GetByIdAsync(int id)
    {
        try
        {
            var result = await dataStore.ActivityImage.GetByIdAsync(id);
            if(!result.Succeeded || result.Result == null)
            {
                return AppResult<ActivityImageDTO>.CreateFailed(result.Error.Exception, result.Message);
            }

            return AppResult<ActivityImageDTO>.CreateSucceeded(new ActivityImageDTO {
                Id = result.Result.Id,
                ImageLocation = result.Result.ImageLocation,
                ImageName = result.Result.ImageName
            }, "Successfully getting activity image by id");
        }
        catch (Exception ex)
        {
            return AppResult<ActivityImageDTO>.CreateFailed(ex, "An error occured when getting activity image by id");
        }
    }

    public async Task<AppResult<ActivityImageDTO>> Update(int activityImageId, string? imageName, string? imagePath, int? order)
    {
        try
        {
            // check activity image if existed
            var result = await dataStore.ActivityImage.GetByIdAsync(activityImageId);
            if(!result.Succeeded || result.Result == null)
            {
                return AppResult<ActivityImageDTO>.CreateFailed(
                    new ApplicationException("Can't find activity image to update"), "Can't find activity image to update");
            }
            var activityImage = result.Result;

            activityImage.ImageLocation = imagePath ?? activityImage.ImageLocation;
            activityImage.ImageName = imageName ?? activityImage.ImageName;
            activityImage.Order = order ?? activityImage.Order;

            var updatedRes = await dataStore.ActivityImage.Update(activityImage);
            if(!updatedRes.Succeeded || updatedRes.Result == null)
            {
                return AppResult<ActivityImageDTO>.CreateFailed(
                    new ApplicationException("An error occured when updating activity image"), "An error occured when updating activity image");
            }

            return AppResult<ActivityImageDTO>.CreateSucceeded(new ActivityImageDTO {
                Id = updatedRes.Result.Id,
                ImageLocation = updatedRes.Result.ImageLocation,
                ImageName = updatedRes.Result.ImageName
            }, "Successfully updated activity image");
        }
        catch (Exception ex)
        {
            return AppResult<ActivityImageDTO>.CreateFailed(ex, "An error occured when updating activity image");
        }
    }

    public async Task<AppResult<IEnumerable<ActivityImageDTO>>> Create(IEnumerable<ActivityImageDTO> images)
    {
        try
        {
            var entities = images.Select(s => {
                return new Entities.ActivityImage {
                    ActivityId = s.ActivityId,
                    ImageLocation = s.ImageLocation,
                    ImageName = s.ImageName,
                    Order = s.Order
                };
            });

            var result = await dataStore.ActivityImage.AddRange(entities);
            if(!result.Succeeded || result.Result == null)
            {
                return AppResult<IEnumerable<ActivityImageDTO>>.CreateFailed(new ApplicationException("An error occured when creating multiple images"), "An error occured when creating multiple images");
            }

            var createdImages = result.Result.Select(s => {
                return new ActivityImageDTO {
                    ActivityId = s.ActivityId,
                    Id = s.Id,
                    ImageLocation = s.ImageLocation,
                    ImageName = s.ImageName,
                    Order = s.Order
                };
            });

            return AppResult<IEnumerable<ActivityImageDTO>>.CreateSucceeded(createdImages, "Successfully create many activity images");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<ActivityImageDTO>>.CreateFailed(ex, "An error occured when creating many activity images");
        }
    }

    public async Task<AppResult<IEnumerable<ActivityImageDTO>>> Update(IEnumerable<ActivityImageDTO> images)
    {
        try
        {
            var entities = images.Select(s => {
                return new Entities.ActivityImage {
                    ActivityId = s.ActivityId,
                    ImageLocation = s.ImageLocation,
                    ImageName = s.ImageName,
                    Id = s.Id,
                    Order = s.Order
                };
            });

            var result = await dataStore.ActivityImage.UpdateRange(entities);
            if(!result.Succeeded || result.Result == null)
            {
                return AppResult<IEnumerable<ActivityImageDTO>>.CreateFailed(new ApplicationException("An error occured when updating multiple images"), "An error occured when updating multiple images");
            }

            var updatedImages = result.Result.Select(s => {
                return new ActivityImageDTO {
                    ActivityId = s.ActivityId,
                    Id = s.Id,
                    ImageLocation = s.ImageLocation,
                    ImageName = s.ImageName,
                    Order  = s.Order
                };
            });

            return AppResult<IEnumerable<ActivityImageDTO>>.CreateSucceeded(updatedImages, "Successfully update many activity images");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<ActivityImageDTO>>.CreateFailed(ex, "An error occured when creating many activity images");
        }
    }
}