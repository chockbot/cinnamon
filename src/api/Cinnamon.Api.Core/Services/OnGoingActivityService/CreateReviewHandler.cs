using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.OnGoingActivityService.Handlers;
using Cinnamon.Api.Core.Services.OnGoingActivityService.Interactors;
using Cinnamon.Api.Core.Services.OnGoingActivityService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.OnGoingActivityService;
public class CreateReviewHandler : ICreateReviewHandler
{
    private readonly IReviewsData reviewsData;
    public CreateReviewHandler(IReviewsData reviewsData)
    {
        this.reviewsData = reviewsData;
    }

    public AppResult<CreateReviewResult> Execute(CreateReviewArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<CreateReviewResult>.CreateFailed(ex, "An error occured in CreateReviewHandler");
        }
    }

    public async Task<AppResult<CreateReviewResult>> ExecuteAsync(CreateReviewArgs args)
    {
        try
        {
            var createReview = await reviewsData.CreateReview(new Framework.ApiCommand.ApiData.Reviews.Request.CreateReviewArgs
            {
                CustomerId  = args.CustomerId,
                MakerId     = args.MakerId,
                ActivityId  = args.ActivityId,
                ScheduleId  = args.ScheduleId,
                StudentId   = args.StudentId,
                Rating      = args.Rating,
                Review      = args.Review,
                ReviewDate  = args.ReviewDate
            });

            if (!createReview.Succeeded)
            {
                return AppResult<CreateReviewResult>.CreateFailed(createReview.Error.Exception, createReview.Message);
            }

            if (createReview.Result == null)
            {
                return AppResult<CreateReviewResult>.CreateFailed(
                    new ApplicationException("An error occured in CreateReviewHandler"), "An error occured in CreateReviewHandler");
            }

            var created = createReview.Result.Result;

            return AppResult<CreateReviewResult>.CreateSucceeded(new CreateReviewResult
            {
                CustomerId  = created.CustomerId,
                MakerId     = created.MakerId,
                ActivityId  = created.ActivityId,
                ScheduleId  = created.ScheduleId,
                StudentId   = created.StudentId,
                Rating      = created.Rating,
                Review      = created.Review,
                ReviewDate  = created.ReviewDate
            }, "Successfully registered");
        }
        catch (Exception ex)
        {
            return AppResult<CreateReviewResult>.CreateFailed(ex, "An error occured in CreateReviewHandler");
        }
    }
}
