using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Modules.DataAccess.Student;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Interactors;
using Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;
using Cinnamon.Api.Core.Services.OnGoingActivityService.Handlers;
using Cinnamon.Api.Core.Services.OnGoingActivityService.Interactors;
using Cinnamon.Api.Core.Services.OnGoingActivityService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.OnGoingActivityService;
public class GetReviewsByMakerIdHandler: IGetReviewsByMakerIdHandler
{
    private readonly IReviewsData reviewsData;
    public GetReviewsByMakerIdHandler(IReviewsData reviewsData)
    {
        this.reviewsData = reviewsData;
    }

    public AppResult<GetReviewsByMakerIdResult> Execute(GetReviewsByMakerIdArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<GetReviewsByMakerIdResult>.CreateFailed(ex, "An error occured in GetReviewsByMakerIdHandler");
        }
    }

    public async Task<AppResult<GetReviewsByMakerIdResult>> ExecuteAsync(GetReviewsByMakerIdArgs args)
    {
        try
        {
            var reviewResReLoad = await reviewsData.GetReviewsByMakerId(new Framework.ApiCommand.ApiData.Reviews.Request.GetReviewsByMakerIdArgs
            {
                MakerId = args.MakerId
            });
            if (!reviewResReLoad.Succeeded || reviewResReLoad.Result == null || !reviewResReLoad.Result.IsSuccess)
            {
                return AppResult<GetReviewsByMakerIdResult>.CreateFailed(new ApplicationException(reviewResReLoad.Result?.ErrorInfo?.Message), reviewResReLoad.Message);
            }
            return AppResult<GetReviewsByMakerIdResult>.CreateSucceeded(new GetReviewsByMakerIdResult
            {
                Review = reviewResReLoad.Result.Result.Select(s =>
                {
                    return new GetReviewsByMakerIdResult.Reviews
                    {
                        Id          = s.Id,
                        CustomerId  = s.CustomerId,
                        MakerId     = s.MakerId,
                        ActivityId  = s.ActivityId,
                        ScheduleId  = s.ScheduleId,
                        StudentId   = s.StudentId,
                        Review      = s.Review,
                        Rating      = s.Rating,
                        ReviewDate  = s.ReviewDate   
                    };
                })
            }, "Successfully get student attendance");

        }
        catch (Exception ex)
        {
            return AppResult<GetReviewsByMakerIdResult>.CreateFailed(ex, "An error occured in GetReviewsByMakerIdHandler");
        }
    }
}
