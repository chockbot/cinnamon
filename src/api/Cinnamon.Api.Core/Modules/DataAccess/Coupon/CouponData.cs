using Cinnamon.Api.Core.Config;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Framework.ApiCommand.ApiData.Coupon.Request;
using Cinnamon.Framework.ApiCommand.ApiData.Coupon.Response;
using Cinnamon.Framework.Common;
using Flurl.Http;
using Flurl.Http.Configuration;

namespace Cinnamon.Api.Core.Modules.DataAccess.Coupon;

public class CouponData : ICouponData
{
    private readonly IFlurlClient flurlClient;
    
	public CouponData(ApplicationConfig config, IFlurlClientFactory flurlFac)
	{
        flurlClient = flurlFac.Get(config.ApiDataUrl);
    }

    public async Task<AppResult<CreateCouponResult>> CreateCoupon(CreateCouponArgs args)
    {
        try
        {
            var result = await flurlClient
                            .Request("Coupon/CreateCoupon")
                            .PostJsonAsync(args)
                            .ReceiveJson<CreateCouponResult>();

            return AppResult<CreateCouponResult>.CreateSucceeded(result, "Successfully posting create coupon api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<CreateCouponResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<CreateCouponResult>.CreateFailed(ex, "An error occured when posting create coupon api");
        }
    }

    public async Task<AppResult<GetAllCouponResult>> GetAllCoupon(GetAllCouponArgs args)
    {
        try
        {
            var result = await flurlClient
                            .Request("Coupon/GetAllCoupon")
                            .SetQueryParams(args)
                            .GetJsonAsync<GetAllCouponResult>();

            return AppResult<GetAllCouponResult>.CreateSucceeded(result, "Successfully getting get all coupon api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetAllCouponResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetAllCouponResult>.CreateFailed(ex, "An error occured when getting all coupon api");
        }
    }

    public async Task<AppResult<GetCouponResult>> GetCouponById(int id)
    {
        try
        {
            var result = await flurlClient
                            .Request($"Coupon/GetCouponById/{id}")
                            .GetJsonAsync<GetCouponResult>();

            return AppResult<GetCouponResult>.CreateSucceeded(result, "Successfully getting coupon by id api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetCouponResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetCouponResult>.CreateFailed(ex, "An error occured when getting coupon by id api");
        }
    }

    public async Task<AppResult<IsPromotionCodeExistResult>> IsPromotionCodeExist(IsPromotionCodeExistArgs args)
    {
        try
        {
            var result = await flurlClient
                            .Request("Coupon/IsPromotionCodeExist")
                            .SetQueryParams(args)
                            .GetJsonAsync<IsPromotionCodeExistResult>();

            return AppResult<IsPromotionCodeExistResult>.CreateSucceeded(result, "Successfully checking coupon");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<IsPromotionCodeExistResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<IsPromotionCodeExistResult>.CreateFailed(ex, "An error occured when checking coupon");
        }
    }

    public async Task<AppResult<UpdateCouponResult>> UpdateCoupon(UpdateCouponArgs args)
    {
        try
        {
            var result = await flurlClient
                            .Request("Coupon/UpdateCoupon")
                            .PostJsonAsync(args)
                            .ReceiveJson<UpdateCouponResult>();

            return AppResult<UpdateCouponResult>.CreateSucceeded(result, "Successfully posting update coupon api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<UpdateCouponResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<UpdateCouponResult>.CreateFailed(ex, "An error occured when posting update coupon api");
        }
    }

    public async Task<AppResult<GetCouponByCodeResult>> GetCouponByCode(GetCouponByCodeArgs args)
    {
        try
        {
            var result = await flurlClient
                            .Request("Coupon/GetCouponByCode")
                            .PostJsonAsync(args)
                            .ReceiveJson<GetCouponByCodeResult>();

            return AppResult<GetCouponByCodeResult>.CreateSucceeded(result, "Successfully get coupon by code coupon api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetCouponByCodeResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetCouponByCodeResult>.CreateFailed(ex, "An error occured when get coupon by code coupon api");
        }
    }
}