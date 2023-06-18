using Cinnamon.Framework.ApiCommand.ApiData.DTO.Coupon;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Data.Services.Repository.Interfaces;

public interface ICouponRepository 
{
    Task<AppResult<CouponDTO>> GetByIdAsync(int id);
    Task<AppResult<IEnumerable<CouponDTO>>> GetAllAsync(int? count, int? skip, bool? includeActivity);
    Task<AppResult<IEnumerable<CouponDTO>>> GetAllAsync();
    Task<AppResult<CouponDTO>> CreateCouponAsync(int activityId, bool isAdmin, int customerId, string name, string code, int discountType, 
        decimal amount, decimal maximumSpend, DateTime from, DateTime to, int status);
    Task<AppResult<CouponDTO>> UpdateCouponAsync(int id, int? activityId, bool? isAdmin, int? customerId, string? name, string? code, int? discountType,
        decimal? amount, decimal? maximumSpend, DateTime? from, DateTime? to, int? status);
    Task<AppResult<bool>> IsCouponCodeAlreadyExist(string code, int activityId, int customerId);
    Task<AppResult<bool>> IsCouponCodeAlreadyExist(string code, int customerId);
}