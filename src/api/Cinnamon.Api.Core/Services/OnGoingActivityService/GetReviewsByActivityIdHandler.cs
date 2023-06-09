using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.OnGoingActivityService.Handlers;
using Cinnamon.Api.Core.Services.OnGoingActivityService.Interactors;
using Cinnamon.Api.Core.Services.OnGoingActivityService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.OnGoingActivityService;

public class GetReviewsByActivityIdHandler: IGetReviewsByActivityIdHandler
{
    private readonly IReviewsData reviewsData;
    public GetReviewsByActivityIdHandler(IReviewsData reviewsData)
    {
        this.reviewsData = reviewsData;
    }

    public AppResult<GetReviewsByActivityIdResult> Execute(GetReviewsByActivityIdArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<GetReviewsByActivityIdResult>.CreateFailed(ex, "An error occured in GetReviewsByActivityIdHandler");
        }
    }

    public async Task<AppResult<GetReviewsByActivityIdResult>> ExecuteAsync(GetReviewsByActivityIdArgs args)
    {
        try
        {
            var reviewResReLoad = await reviewsData.GetReviewsByActivityId(new Framework.ApiCommand.ApiData.Reviews.Request.GetReviewsByActivityIdArgs
            {
                ActivityId = args.ActivityId
            });
            if (!reviewResReLoad.Succeeded || reviewResReLoad.Result == null || !reviewResReLoad.Result.IsSuccess)
            {
                return AppResult<GetReviewsByActivityIdResult>.CreateFailed(new ApplicationException(reviewResReLoad.Result?.ErrorInfo?.Message), reviewResReLoad.Message);
            }
            return AppResult<GetReviewsByActivityIdResult>.CreateSucceeded(new GetReviewsByActivityIdResult
            {
                Review = reviewResReLoad.Result.Result.Select(s =>
                {
                    return new GetReviewsByActivityIdResult.Reviews
                    {
                        Id = s.Id,
                        CustomerId = s.CustomerId,
                        MakerId = s.MakerId,
                        ActivityId = s.ActivityId,
                        ScheduleId = s.ScheduleId,
                        StudentId = s.StudentId,
                        Review = s.Review,
                        Rating = s.Rating,
                        ReviewDate = s.ReviewDate
                    };
                })
            }, "Successfully get student attendance");

        }
        catch (Exception ex)
        {
            return AppResult<GetReviewsByActivityIdResult>.CreateFailed(ex, "An error occured in GetReviewsByActivityIdHandler");
        }
    }
}
