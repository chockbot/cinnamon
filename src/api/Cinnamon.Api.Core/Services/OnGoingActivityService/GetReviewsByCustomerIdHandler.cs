using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.OnGoingActivityService.Handlers;
using Cinnamon.Api.Core.Services.OnGoingActivityService.Interactors;
using Cinnamon.Api.Core.Services.OnGoingActivityService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.OnGoingActivityService;
public class GetReviewsByCustomerIdHandler : IGetReviewsByCustomerIdHandler
{
    private readonly IReviewsData reviewsData;
    public GetReviewsByCustomerIdHandler(IReviewsData reviewsData)
    {
        this.reviewsData = reviewsData;
    }

    public AppResult<GetReviewsByCustomerIdResult> Execute(GetReviewsByCustomerIdArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<GetReviewsByCustomerIdResult>.CreateFailed(ex, "An error occured in GetReviewsByCustomerIdHandler");
        }
    }

    public async Task<AppResult<GetReviewsByCustomerIdResult>> ExecuteAsync(GetReviewsByCustomerIdArgs args)
    {
        try
        {
            var reviewResReLoad = await reviewsData.GetReviewsByCustomerId(new Framework.ApiCommand.ApiData.Reviews.Request.GetReviewsByCustomerIdArgs
            {
                CustomerId   = args.CustomerId,
                PageIndex    = args.PageIndex,
                CountPerPage = args.CountPerPage,
            });
            if (!reviewResReLoad.Succeeded || reviewResReLoad.Result == null || !reviewResReLoad.Result.IsSuccess)
            {
                return AppResult<GetReviewsByCustomerIdResult>.CreateFailed(new ApplicationException(reviewResReLoad.Result?.ErrorInfo?.Message), reviewResReLoad.Message);
            }
            return AppResult<GetReviewsByCustomerIdResult>.CreateSucceeded(new GetReviewsByCustomerIdResult
            {
                Review = reviewResReLoad.Result.Result.Select(s =>
                {
                    return new GetReviewsByCustomerIdResult.Reviews
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
                }),
                ErrorInfo = new Framework.ApiCommand.ApiCore.ErrorInfo
                {
                    Code        = reviewResReLoad?.Result?.ErrorInfo?.Code,
                    Description = reviewResReLoad?.Result?.ErrorInfo?.Description,
                    Message     = reviewResReLoad?.Result?.ErrorInfo?.Message
                },
                Pagination = new Framework.ApiCommand.ApiCore.Pagination
                {
                    PageIndex    = reviewResReLoad.Result.Pagination.PageIndex,
                    PerPage      = reviewResReLoad.Result.Pagination.PerPage,
                    TotalPages   = reviewResReLoad.Result.Pagination.TotalPages,
                    TotalRecords = reviewResReLoad.Result.Pagination.TotalRecords
                }
            }, "Successfully get student attendance");

        }
        catch (Exception ex)
        {
            return AppResult<GetReviewsByCustomerIdResult>.CreateFailed(ex, "An error occured in GetReviewsByCustomerIdHandler");
        }
    }
}
