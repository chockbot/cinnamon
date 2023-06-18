using Cinnamon.Api.Data.Repository.Interfaces;
using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.Coupon;
using Cinnamon.Framework.Common;
using Entities = Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Extensions;
using System.Linq.Expressions;

namespace Cinnamon.Api.Data.Services.Repository.Coupon;

public class CouponRespository : ICouponRepository
{
    private readonly IDataStore dataStore;

    public CouponRespository(IDataStore dataStore)
    {
        this.dataStore = dataStore;
    }

    public async Task<AppResult<CouponDTO>> CreateCouponAsync(int activityId, bool isAdmin, int customerId, string name, string code, 
        int discountType, decimal amount, decimal maximumSpend, DateTime from, DateTime to, int status)
    {
        try
        {
            var coupon = new Entities.Coupon {
                ActivityId = activityId == 0 ? null : activityId,
                Amount = amount,
                Code = code,
                CustomerId = customerId,
                DiscountType = discountType,
                From = from.SetKindUtc(),
                To = to.SetKindUtc(),
                IsAdmin = isAdmin,
                MaximumSpend = maximumSpend,
                Status = status,
                Name = name,
            };

            var createCoupon = await dataStore.Coupon.Add(coupon);
            if(!createCoupon.Succeeded || createCoupon.Result == null)
            {
                return AppResult<CouponDTO>.CreateFailed(new ApplicationException(createCoupon.Message), createCoupon.Message);
            }
            var created = createCoupon.Result;

            return AppResult<CouponDTO>.CreateSucceeded(new CouponDTO {
                ActivityId = created.ActivityId ?? 0,
                Amount = created.Amount,
                Code = created.Code,
                CustomerId = created.CustomerId,
                DiscountType = created.DiscountType,
                From = created.From,
                Id = created.Id,
                MaximumSpend = created.MaximumSpend,
                Name = created.Name,
                Status = created.Status,
                To = created.To
            }, "Successfully created coupon");
        }
        catch (Exception ex)
        {
            return AppResult<CouponDTO>.CreateFailed(ex, "An error occured when creating coupon");
        }
    }

    public async Task<AppResult<IEnumerable<CouponDTO>>> GetAllAsync(int? count, int? skip, bool? includeActivity)
    {
        try
        {
            var include = new List<Expression<Func<Entities.Coupon, object>>>();
            if(includeActivity.HasValue && includeActivity.Value) include.Add(c => c.Activity);

            var result = await dataStore.Coupon.FindAsync(c => true, count, skip, include);
            if(!result.Succeeded || result.Result == null)
            {
                return AppResult<IEnumerable<CouponDTO>>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }
            var coupons = result.Result;

            return AppResult<IEnumerable<CouponDTO>>.CreateSucceeded(coupons.Select(c =>  {
                return new CouponDTO {
                    ActivityApplied = includeActivity.HasValue && includeActivity.Value ? new CouponDTO.Activity {
                        Id = c.Activity.Id,
                        Title = c.Activity.Title
                    } : null,
                    ActivityId = c.ActivityId ?? 0,
                    Amount = c.Amount,
                    Code = c.Code,
                    CustomerId = c.CustomerId,
                    DiscountType = c.DiscountType,
                    From = c.From,
                    Id = c.Id,
                    MaximumSpend = c.MaximumSpend,
                    Name = c.Name,
                    Status = c.Status,
                    To = c.To,
                };
            }), "Successfully get coupons");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<CouponDTO>>.CreateFailed(ex, "An error occured when getting coupon codes");
        }
    }

    public async Task<AppResult<IEnumerable<CouponDTO>>> GetAllAsync()
    {
        try
        {
            var result = await dataStore.Coupon.GetAllAsync();
            if(!result.Succeeded || result.Result == null)
            {
                return AppResult<IEnumerable<CouponDTO>>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }
            var coupons = result.Result;

            return AppResult<IEnumerable<CouponDTO>>.CreateSucceeded(coupons.Select(c =>  {
                return new CouponDTO {
                    ActivityId = c.ActivityId ?? 0,
                    Amount = c.Amount,
                    Code = c.Code,
                    CustomerId = c.CustomerId,
                    DiscountType = c.DiscountType,
                    From = c.From,
                    Id = c.Id,
                    MaximumSpend = c.MaximumSpend,
                    Name = c.Name,
                    Status = c.Status,
                    To = c.To,
                };
            }), "Successfully get coupons");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<CouponDTO>>.CreateFailed(ex, "An error occured when getting coupon codes");
        }
    }

    public async Task<AppResult<CouponDTO>> GetByIdAsync(int id)
    {
        try
        {
            var result = await dataStore.Coupon.GetByIdAsync(id);
            if(!result.Succeeded || result.Result == null)
            {
                return AppResult<CouponDTO>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }
            var coupon = result.Result;

            return AppResult<CouponDTO>.CreateSucceeded(new CouponDTO {
                ActivityId = coupon.ActivityId ?? 0,
                Amount = coupon.Amount,
                Code = coupon.Code,
                CustomerId = coupon.CustomerId,
                DiscountType = coupon.DiscountType,
                From = coupon.From,
                Id = coupon.Id,
                MaximumSpend = coupon.MaximumSpend,
                Name = coupon.Name,
                Status = coupon.Status,
                To = coupon.To
            }, "Successfully get coupon code by id");
        }
        catch (Exception ex)
        {
            return AppResult<CouponDTO>.CreateFailed(ex, "An error occured when getting coupon by id");
        }
    }

    public async Task<AppResult<bool>> IsCouponCodeAlreadyExist(string code, int activityId, int customerId)
    {
        try
        {
            var result = await dataStore.Coupon.FindAsync(c => c.Code.ToLower() == code.ToLower() && c.ActivityId == activityId && c.CustomerId == c.CustomerId);
            if(!result.Succeeded || result.Result == null)
            {
                return AppResult<bool>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }

            return AppResult<bool>.CreateSucceeded(result.Result.Count() > 0, "Successfully check coupon code");
        }
        catch (Exception ex)
        {
            return AppResult<bool>.CreateFailed(ex, "An error occured when checking if coupon code already exist");
        }
    }

    public async Task<AppResult<bool>> IsCouponCodeAlreadyExist(string code, int customerId)
    {
        try
        {
            var result = await dataStore.Coupon.FindAsync(c => c.Code.ToLower() == code.ToLower() && c.CustomerId == c.CustomerId);
            if(!result.Succeeded || result.Result == null)
            {
                return AppResult<bool>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }

            return AppResult<bool>.CreateSucceeded(result.Result.Count() > 0, "Successfully check coupon code");
        }
        catch (Exception ex)
        {
            return AppResult<bool>.CreateFailed(ex, "An error occured when checking if coupon code already exist");
        }
    }

    public async Task<AppResult<CouponDTO>> UpdateCouponAsync(int id, int? activityId, bool? isAdmin, int? customerId, string? name, 
        string? code, int? discountType, decimal? amount, decimal? maximumSpend, DateTime? from, DateTime? to, int? status)
    {
        try
        {
            var result = await dataStore.Coupon.GetByIdAsync(id);
            if(!result.Succeeded || result.Result == null)
            {
                return AppResult<CouponDTO>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }
            var coupon = result.Result;

            if(activityId.HasValue)
            {
                coupon.ActivityId = activityId.Value == 0 ? null : activityId.Value;
            }
            coupon.Amount = amount ?? coupon.Amount;
            coupon.Code = code ?? coupon.Code;
            coupon.CustomerId = customerId ?? coupon.CustomerId;
            coupon.DiscountType = discountType ?? coupon.DiscountType;
            coupon.From = from.HasValue ? from.Value.SetKindUtc() : coupon.From.SetKindUtc();
            coupon.IsAdmin = isAdmin ?? coupon.IsAdmin;
            coupon.MaximumSpend = maximumSpend ?? coupon.MaximumSpend;
            coupon.Name = name ?? coupon.Name;
            coupon.Status = status ?? coupon.Status;
            coupon.To = to.HasValue ? to.Value.SetKindUtc() : coupon.To.SetKindUtc();

            var updateRes = await dataStore.Coupon.Update(coupon);
            if(!updateRes.Succeeded || updateRes.Result == null)
            {
                return AppResult<CouponDTO>.CreateFailed(new ApplicationException(updateRes.Message), updateRes.Message);
            }
            var updated = updateRes.Result;

            return AppResult<CouponDTO>.CreateSucceeded(new CouponDTO {
                ActivityId = updated.ActivityId ?? 0,
                Amount = updated.Amount,
                Code = updated.Code,
                CustomerId = updated.CustomerId,
                DiscountType = updated.DiscountType,
                From = updated.From,
                Id = updated.Id,
                MaximumSpend = updated.MaximumSpend,
                Name = updated.Name,
                Status = updated.Status,
                To = updated.To,
            }, "Successfully update coupon code");
        }
        catch (Exception ex)
        {
            return AppResult<CouponDTO>.CreateFailed(ex, "An error occured when updating coupon code");
        }
    }
}