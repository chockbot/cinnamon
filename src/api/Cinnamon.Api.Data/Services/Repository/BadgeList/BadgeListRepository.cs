using Cinnamon.Api.Data.Repository.Interfaces;
using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.BadgeList;
using Cinnamon.Framework.Common;
using Entities = Cinnamon.Api.Data.Repository.Entities;

namespace Cinnamon.Api.Data.Services.Repository.BadgeList;

public class BadgeListRepository: IBadgeListRepository
{
    private readonly IDataStore dataStore;
    public BadgeListRepository(IDataStore dataStore)
    {
        this.dataStore = dataStore;
    }

    public async Task<AppResult<BadgeListDTO>> CreateBadgeAsync(string Name, string Description, int NumberOfStudent,int NumberOfCompleted, int NumberOfReviews, string ImgScr)
    {
        try
        {
            var badge = new Entities.BadgeList
            {
                Name = Name,
                Description = Description,
                NumberOfEnrolledStudent = NumberOfStudent,
                NumberOfCompletedStudent = NumberOfCompleted,
                NumberOfReviews = NumberOfReviews,
                ImgScr = ImgScr
            };
            var createdBadge = await dataStore.BadgeList.Add(badge);
            if (!createdBadge.Succeeded || createdBadge.Result == null)
            {
                return AppResult<BadgeListDTO>.CreateFailed(createdBadge.Error.Exception, createdBadge.Message);
            }
            return AppResult<BadgeListDTO>.CreateSucceeded(new BadgeListDTO
            {
                Name = Name,
                Description = Description,
                NumberOfStudent = NumberOfStudent,
                NumberOfCompleted = NumberOfCompleted,
                NumberOfReviews = NumberOfReviews,
                ImgScr = ImgScr
            }, "Successfully created badge");
        }
        catch (Exception ex)
        {
            return AppResult<BadgeListDTO>.CreateFailed(ex, "An error occured when creating badge");
        }
    }

    public async Task<AppResult<IEnumerable<BadgeListDTO>>> GetAllAsync(int? count, int? skip)
    {
        try
        {
            var result = await dataStore.BadgeList.FindAsync(i => true, count, skip);
            if (!result.Succeeded || result.Result == null)
            {
                return AppResult<IEnumerable<BadgeListDTO>>.CreateFailed(result.Error.Exception, result.Message);
            }

            var category = result.Result.Select(c =>
            {
                return new BadgeListDTO
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    NumberOfStudent = c.NumberOfEnrolledStudent,
                    NumberOfCompleted = c.NumberOfCompletedStudent,
                    NumberOfReviews = c.NumberOfReviews,
                    ImgScr = c.ImgScr
                };
            });

            return AppResult<IEnumerable<BadgeListDTO>>.CreateSucceeded(category, "Successfully get badges");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<BadgeListDTO>>.CreateFailed(ex, "An error occured in getting badges");
        }
    }

    public async Task<AppResult<IEnumerable<BadgeListDTO>>> GetAllAsync()
    {
        try
        {
            var result = await dataStore.BadgeList.GetAllAsync();
            if (!result.Succeeded || result.Result == null)
            {
                return AppResult<IEnumerable<BadgeListDTO>>.CreateFailed(result.Error.Exception, result.Message);
            }
            var experienceCategories = result.Result.Select(a =>
            {
                return new BadgeListDTO
                {
                    Id = a.Id,
                    Name= a.Name,
                    Description = a.Description,
                    NumberOfStudent = a.NumberOfEnrolledStudent,
                    NumberOfCompleted = a.NumberOfCompletedStudent,
                    NumberOfReviews= a.NumberOfReviews,
                    ImgScr = a.ImgScr
                };
            });
            return AppResult<IEnumerable<BadgeListDTO>>.CreateSucceeded(experienceCategories, "Successfully get badges");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<BadgeListDTO>>.CreateFailed(ex, "An error occured in getting badges");
        }
    }

    public async Task<AppResult<BadgeListDTO>> GetByIdAsync(int id)
    {
        try
        {
            var result = await dataStore.BadgeList.GetByIdAsync(id);
            if (!result.Succeeded || result.Result == null)
            {
                return AppResult<BadgeListDTO>.CreateFailed(result.Error.Exception, result.Message);
            }

            var badgeDTO = new BadgeListDTO
            {
                Id = result.Result.Id,
                Name = result.Result.Name,
                Description = result.Result.Description,
                NumberOfStudent= result.Result.NumberOfEnrolledStudent,
                NumberOfCompleted = result.Result.NumberOfCompletedStudent,
                NumberOfReviews = result.Result.NumberOfReviews,
                ImgScr = result.Result.ImgScr
            };

            return AppResult<BadgeListDTO>.CreateSucceeded(badgeDTO, "Successfully getting badge by id");
        }
        catch (Exception ex)
        {
            return AppResult<BadgeListDTO>.CreateFailed(ex, "An error occured when getting badge by id");
        }
    }

    public async Task<AppResult<BadgeListDTO>> UpdateBadgeAsync(int Id, string? Name, string? Description, int? NumberOfStudent, int? NumberOfCompleted, int? NumberOfReviews, string? ImgScr)
    {
        try
        {
            //check if badge exist
            var badgeRes = await dataStore.BadgeList.GetByIdAsync(Id);
            if (!badgeRes.Succeeded || badgeRes.Result == null)
            {
                return AppResult<BadgeListDTO>.CreateFailed(new ApplicationException("Can't find badge to update"), "Can't find badge to update");
            }

            var badge = badgeRes.Result;
            badge.Name = Name ?? badge.Name;
            badge.Description = Description ?? badge.Description;
            badge.NumberOfEnrolledStudent = NumberOfStudent ?? badge.NumberOfEnrolledStudent;
            badge.NumberOfCompletedStudent = NumberOfCompleted ?? badge.NumberOfCompletedStudent;
            badge.NumberOfReviews = NumberOfReviews ?? badge.NumberOfReviews;
            badge.ImgScr = ImgScr ?? badge.ImgScr;

            var updatedBadge = await dataStore.BadgeList.Update(badge);
            if (!updatedBadge.Succeeded)
            {
                return AppResult<BadgeListDTO>.CreateFailed(updatedBadge.Error.Exception, updatedBadge.Message);
            }
            return AppResult<BadgeListDTO>.CreateSucceeded(new BadgeListDTO
            {
                Name              = badge.Name,
                Description       = badge.Description,
                NumberOfStudent   = badge.NumberOfEnrolledStudent,
                NumberOfCompleted = badge.NumberOfCompletedStudent,
                NumberOfReviews   = badge.NumberOfReviews,
                ImgScr            = badge.ImgScr
            }, "Successfully updated experience category");

        }
        catch (Exception ex)
        {
            return AppResult<BadgeListDTO>.CreateFailed(ex, "An error occured when updating badge");
        }
    }
}
