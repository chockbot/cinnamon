using Cinnamon.Api.Data.Repository.Interfaces;
using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.Reviews;
using Cinnamon.Framework.Common;
using System.Linq.Expressions;
using Entities = Cinnamon.Api.Data.Repository.Entities;

namespace Cinnamon.Api.Data.Services.Repository.Reviews;

public class ReviewsRepository: IReviewsRepository
{
    private readonly IDataStore dataStore;

    public ReviewsRepository(IDataStore dataStore)
    {
        this.dataStore = dataStore;
    }

    public async Task<AppResult<ReviewsDTO>> CreateExperienceCategoryAsync(int CustomerId, int MakerId, int ActivityId, int ScheduleId, int StudentId, int Rating, string Review, DateTime ReviewDate)
    {
        try
        {
            var review = new Entities.Reviews
            {
                CustomerId = CustomerId,
                MakerId = MakerId,
                ActivityId = ActivityId,
                ScheduleId = ScheduleId,
                StudentId = StudentId,
                Rating = Rating,
                Review = Review,
                ReviewDate = ReviewDate
            };
            var createdReview = await dataStore.Reviews.Add(review);
            if (!createdReview.Succeeded || createdReview.Result == null)
            {
                return AppResult<ReviewsDTO>.CreateFailed(createdReview.Error.Exception, createdReview.Message);
            }
            return AppResult<ReviewsDTO>.CreateSucceeded(new ReviewsDTO
            {
                CustomerId = CustomerId,
                MakerId = MakerId,
                ActivityId = ActivityId,
                ScheduleId = ScheduleId,
                StudentId = StudentId,
                Rating = Rating,
                Review = Review,
                ReviewDate = ReviewDate
            }, "Successfully created experience category");
        }
        catch (Exception ex)
        {

            return AppResult<ReviewsDTO>.CreateFailed(ex, "An error occured when creating review");
        }
    }

    public async Task<AppResult<IEnumerable<ReviewsDTO>>> GetAllAsync(int? count, int? skip)
    {
        try
        {
            var result = await dataStore.Reviews.FindAsync(i => true, count, skip);
            if (!result.Succeeded || result.Result == null)
            {
                return AppResult<IEnumerable<ReviewsDTO>>.CreateFailed(result.Error.Exception, result.Message);
            }

            var review = result.Result.Select(c =>
            {
                return new ReviewsDTO
                {
                   Id = c.Id,
                   CustomerId = c.CustomerId,
                   MakerId = c.MakerId,
                   ActivityId = c.ActivityId,
                   ScheduleId = c.ScheduleId,
                   StudentId = c.StudentId,
                   Rating = c.Rating,
                   Review = c.Review,
                   ReviewDate = c.ReviewDate
                };
            });

            return AppResult<IEnumerable<ReviewsDTO>>.CreateSucceeded(review, "Successfully get reviews");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<ReviewsDTO>>.CreateFailed(ex, "An error occured in getting reviews");
        }
    }

    public async Task<AppResult<IEnumerable<ReviewsDTO>>> GetAllAsync()
    {
        try
        {
            var result = await dataStore.Reviews.GetAllAsync();
            if (!result.Succeeded || result.Result == null)
            {
                return AppResult<IEnumerable<ReviewsDTO>>.CreateFailed(result.Error.Exception, result.Message);
            }
            var reviews = result.Result.Select(a =>
            {
                return new ReviewsDTO
                {
                    Id = a.Id,
                    CustomerId = a.CustomerId,
                    MakerId = a.MakerId,
                    ActivityId = a.ActivityId,
                    ScheduleId = a.ScheduleId,
                    StudentId = a.StudentId,
                    Rating = a.Rating,
                    Review = a.Review,
                    ReviewDate = a.ReviewDate
                };
            });
            return AppResult<IEnumerable<ReviewsDTO>>.CreateSucceeded(reviews, "Successfully get reviews");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<ReviewsDTO>>.CreateFailed(ex, "An error occured in getting reviews");
        }
    }
    
    public async Task<AppResult<ReviewsDTO>> GetByIdAsync(int id)
    {
        try
        {
            var result = await dataStore.Reviews.GetByIdAsync(id);
            if (!result.Succeeded || result.Result == null)
            {
                return AppResult<ReviewsDTO>.CreateFailed(result.Error.Exception, result.Message);
            }

            var reviewDTO = new ReviewsDTO
            {
                Id = result.Result.Id,
                CustomerId= result.Result.CustomerId,
                MakerId= result.Result.MakerId,
                ActivityId= result.Result.ActivityId,
                ScheduleId= result.Result.ScheduleId,
                StudentId= result.Result.StudentId,
                Rating = result.Result.Rating,
                Review = result.Result.Review,
                ReviewDate = result.Result.ReviewDate
            };

            return AppResult<ReviewsDTO>.CreateSucceeded(reviewDTO, "Successfully getting review by id");
        }
        catch (Exception ex)
        {
            return AppResult<ReviewsDTO>.CreateFailed(ex, "An error occured when getting review by id");
        }
    }

    public async Task<AppResult<ReviewsDTO>> UpdateExperienceCategoryAsync(int Id, int? CustomerId, int? MakerId, int? ActivityId, int? ScheduleId, int? StudentId, int? Rating, string? Review, DateTime? ReviewDate)
    {
        try
        {
            //check if experience category exist
            var reviewRes = await dataStore.Reviews.GetByIdAsync(Id);
            if (!reviewRes.Succeeded || reviewRes.Result == null)
            {
                return AppResult<ReviewsDTO>.CreateFailed(new ApplicationException("Can't find review to update"), "Can't find review to update");
            }

            var reviewActivity = reviewRes.Result;
            reviewActivity.CustomerId = CustomerId ?? reviewActivity.CustomerId;
            reviewActivity.MakerId = MakerId ?? reviewActivity.MakerId;
            reviewActivity.ActivityId = ActivityId ?? reviewActivity.ActivityId;
            reviewActivity.ScheduleId = ScheduleId ?? reviewActivity.ScheduleId;
            reviewActivity.StudentId = StudentId ?? reviewActivity.StudentId;
            reviewActivity.Rating = Rating ?? reviewActivity.Rating;
            reviewActivity.Review = Review ?? reviewActivity.Review;
            reviewActivity.ReviewDate = ReviewDate ?? reviewActivity.ReviewDate;

            var updatedReview = await dataStore.Reviews.Update(reviewActivity);
            if (!updatedReview.Succeeded)
            {
                return AppResult<ReviewsDTO>.CreateFailed(updatedReview.Error.Exception, updatedReview.Message);
            }
            return AppResult<ReviewsDTO>.CreateSucceeded(new ReviewsDTO
            {
                CustomerId = reviewActivity.CustomerId,
                MakerId = reviewActivity.MakerId,
                ActivityId = reviewActivity.ActivityId,
                ScheduleId = reviewActivity.ScheduleId,
                StudentId = reviewActivity.StudentId,
                Rating = reviewActivity.Rating,
                Review = reviewActivity.Review,
                ReviewDate = reviewActivity.ReviewDate,
            }, "Successfully updated review");

        }
        catch (Exception ex)
        {
            return AppResult<ReviewsDTO>.CreateFailed(ex, "An error occured when updating activity review");
        }
    }

    public async Task<AppResult<IEnumerable<ReviewsDTO>>> GetAllReviewsById(int? makerId, int? count, int? skip)
    {
        try
        {
            Expression<Func<Entities.Reviews, bool>> filter = a => (a.MakerId == makerId);
            var result = await dataStore.Reviews.FindAsync(filter, count, skip);
            if (!result.Succeeded || result.Result == null)
            {
                return AppResult<IEnumerable<ReviewsDTO>>.CreateFailed(result.Error.Exception, result.Message);
            }
            var reviews = result.Result.Select(s => {
                var reviewsDto = new ReviewsDTO
                {
                    Id          = s.Id,
                    CustomerId  = s.CustomerId,
                    MakerId     = s.MakerId,
                    ActivityId  = s.ActivityId,
                    StudentId   = s.StudentId,
                    ScheduleId  = s.ScheduleId,
                    Rating      = s.Rating,
                    Review      = s.Review,
                    ReviewDate  = s.ReviewDate
                };

                return reviewsDto;
            });

            return AppResult<IEnumerable<ReviewsDTO>>.CreateSucceeded(reviews, "Successfully get reviews");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<ReviewsDTO>>.CreateFailed(ex, "An error occured when getting reviews");
        }
    }

}
