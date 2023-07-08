using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiData;
using Cinnamon.Framework.ApiCommand.ApiData.Coupon.Request;
using Cinnamon.Framework.ApiCommand.ApiData.Coupon.Response;
using Microsoft.AspNetCore.Mvc;

namespace Cinnamon.Api.Data.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CouponController : ControllerBase
{
    private readonly ICouponRepository couponRepository;

    public CouponController(ICouponRepository couponRepository)
    {
        this.couponRepository = couponRepository;
    }

    [Route("CreateCoupon")]
    [HttpPost]
    [ProducesResponseType(typeof(CreateCouponResult), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateCoupon([FromBody] CreateCouponArgs args)
    {
        try
        {
            var result = await couponRepository.CreateCouponAsync(args.ActivityId, args.IsAdmin, args.CustomerId, args.Name,
                args.Code, args.DiscountType, args.Amount, args.MaximumSpend, args.From, args.To, args.Status);
            
            if(!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new CreateCouponResult {ErrorInfo = new ErrorInfo { Message = result.Message }});
            }

            return new JsonResult(new CreateCouponResult { IsSuccess = true, Result = result.Result });
        }
        catch (Exception ex)
        {
            return new JsonResult(new CreateCouponResult {ErrorInfo = new ErrorInfo { Message = ex.Message }});
        }
    }

    [Route("UpdateCoupon")]
    [HttpPost]
    [ProducesResponseType(typeof(UpdateCouponResult), StatusCodes.Status201Created)]
    public async Task<IActionResult> UpdateCoupon([FromBody] UpdateCouponArgs args)
    {
        try
        {
            var result = await couponRepository.UpdateCouponAsync(args.Id, args.ActivityId, args.IsAdmin, args.CustomerId,
                args.Name, args.Code, args.DiscountType, args.Amount, args.MaximumSpend, args.From, args.To, args.Status);
            
            if(!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new UpdateCouponResult {ErrorInfo = new ErrorInfo { Message = result.Message }});
            }

            return new JsonResult(new UpdateCouponResult {IsSuccess = true, Result = result.Result});
        }
        catch (Exception ex)
        {
            return new JsonResult(new UpdateCouponResult {ErrorInfo = new ErrorInfo { Message = ex.Message }});
        }
    }

    [Route("GetCouponById/{id}")]
    [HttpGet]
    [ProducesResponseType(typeof(GetCouponResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCouponById(int id)
    {
        try
        {
            var result = await couponRepository.GetByIdAsync(id);
            
            if(!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetCouponResult {ErrorInfo = new ErrorInfo {Message = result.Message}});
            }

            return new JsonResult(new GetCouponResult {IsSuccess = true, Result = result.Result});
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetCouponResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("GetAllCoupon")]
    [HttpGet]
    [ProducesResponseType(typeof(GetAllCouponResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllCoupon([FromQuery] GetAllCouponArgs args)
    {
        try
        {
            bool isHaveFilter = (args.PageIndex.HasValue && args.CountPerPage.HasValue) || args.IncludeActivity.HasValue || args.CustomerId.HasValue;

            var result = isHaveFilter?
                await couponRepository.GetAllAsync(args.CountPerPage, (args.PageIndex - 1) * args.CountPerPage, args.IncludeActivity, args.CustomerId) :
                await couponRepository.GetAllAsync();
            
            if (!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetAllCouponResult { ErrorInfo = new ErrorInfo { Message = result.Message } });
            }

            // get all without pagination to get all rows
            var all = isHaveFilter ?
                await couponRepository.GetAllAsync(null, null, null, null) :
                await couponRepository.GetAllAsync();

            if (!all.Succeeded || all.Result == null)
            {
                return new JsonResult(new GetAllCouponResult { ErrorInfo = new ErrorInfo { Message = all.Message } });
            }

            var totalRecords = all.Result.Count();
            return new JsonResult(new GetAllCouponResult
            {
                Result = result.Result,
                IsSuccess = true,
                Pagination = new Pagination
                {
                    PageIndex = args.PageIndex,
                    PerPage = args.CountPerPage,
                    TotalRecords = totalRecords,
                    TotalPages = args.CountPerPage.HasValue && args.PageIndex.HasValue ?
                                (int)Math.Ceiling((double)totalRecords / args.CountPerPage.Value) : null
                }
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetAllCouponResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("IsPromotionCodeExist")]
    [HttpGet]
    [ProducesResponseType(typeof(IsPromotionCodeExistResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> IsPromotionCodeExist([FromQuery] IsPromotionCodeExistArgs args) 
    {
        try
        {
            var result = args.ActivityId != null ? 
                await couponRepository.IsCouponCodeAlreadyExist(args.Code, args.ActivityId.Value, args.CustomerId) :
                await couponRepository.IsCouponCodeAlreadyExist(args.Code, args.CustomerId);

            if(!result.Succeeded)
            {
                return new JsonResult(new IsPromotionCodeExistResult {ErrorInfo = new ErrorInfo {Message = result.Message}});
            }

            return new JsonResult(new IsPromotionCodeExistResult{IsSuccess = true, Result = result.Result});
        }
        catch (Exception ex)
        {
            return new JsonResult(new IsPromotionCodeExistResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }

    [Route("GetCouponByCode")]
    [HttpPost]
    [ProducesResponseType(typeof(GetCouponByCodeResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCouponByCode([FromBody] GetCouponByCodeArgs args) 
    {
        try
        {
            var result = await couponRepository.GetByCouponCodeAsync(args.Code);

            if(!result.Succeeded || result.Result == null)
            {
                return new JsonResult(new GetCouponByCodeResult {ErrorInfo = new ErrorInfo {Message = result.Message}});
            }

            return new JsonResult(new GetCouponByCodeResult{IsSuccess = true, Result = result.Result});
        }
        catch (Exception ex)
        {
            return new JsonResult(new GetCouponByCodeResult { ErrorInfo = new ErrorInfo { Message = ex.Message } });
        }
    }
}