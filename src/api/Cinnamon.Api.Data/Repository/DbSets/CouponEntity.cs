using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Repository.Interfaces;

namespace Cinnamon.Api.Data.Repository.DbSets;

public class CouponEntity : GenericEntity<Coupon>, ICoupon
{
    public CouponEntity(ApplicationContext applicationContext) : base(applicationContext)
    {
    }
}