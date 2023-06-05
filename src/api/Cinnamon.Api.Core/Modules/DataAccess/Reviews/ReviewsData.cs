using Cinnamon.Api.Core.Config;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Framework.ApiCommand.ApiData.Reviews.Request;
using Cinnamon.Framework.ApiCommand.ApiData.Reviews.Response;
using Cinnamon.Framework.Common;
using Flurl;
using Flurl.Http;
using Flurl.Http.Configuration;

namespace Cinnamon.Api.Core.Modules.DataAccess.Reviews;
public class ReviewsData: IReviewsData
{
    private readonly IFlurlClient flurlClient;
    public ReviewsData(ApplicationConfig config, IFlurlClientFactory flurlFac)
    {
        flurlClient = flurlFac.Get(config.ApiDataUrl);
    }

    public async Task<AppResult<CreatedReviewResult>> CreateReview(CreateReviewArgs args)
    {
        try
        {
            var result = await flurlClient
                            .Request("Review/CreateReview")
                            .PostJsonAsync(args)
                            .ReceiveJson<CreatedReviewResult>();

            return AppResult<CreatedReviewResult>.CreateSucceeded(result, "Successfully posting create review api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<CreatedReviewResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<CreatedReviewResult>.CreateFailed(ex, "An error occured when posting create review api");
        }
    }

    public async Task<AppResult<GetAllReviewsResult>> GetAllReviews(GetAllReviewsArgs args)
    {
        try
        {
            var result = await flurlClient
                            .Request("Review/GetAllReviews")
                            .SetQueryParams(args)
                            .GetJsonAsync<GetAllReviewsResult>();

            return AppResult<GetAllReviewsResult>.CreateSucceeded(result, "Successfully getting get all reviews api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetAllReviewsResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetAllReviewsResult>.CreateFailed(ex, "An error occured when getting all reviews api");
        }
    }

    public async Task<AppResult<GetReviewResult>> GetReviewById(int id)
    {
        try
        {
            var result = await flurlClient
                            .Request($"Review/GetReviewById/{id}")
                            .GetJsonAsync<GetReviewResult>();

            return AppResult<GetReviewResult>.CreateSucceeded(result, "Successfully getting review by id api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetReviewResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetReviewResult>.CreateFailed(ex, "An error occured when getting review by id api");
        }
    }

    public async Task<AppResult<UpdatedReviewResult>> UpdateReview(UpdateReviewArgs args)
    {
        try
        {
            var result = await flurlClient
                            .Request("Review/UpdateReview")
                            .PostJsonAsync(args)
                            .ReceiveJson<UpdatedReviewResult>();

            return AppResult<UpdatedReviewResult>.CreateSucceeded(result, "Successfully posting update review api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<UpdatedReviewResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<UpdatedReviewResult>.CreateFailed(ex, "An error occured when posting update review api");
        }
    }
}
