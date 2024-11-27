using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Repository.Interfaces;

namespace Cinnamon.Api.Data.Repository.DbSets;
public class GuestOTPEntity : GenericEntity<GuestOTP>, IGuestOTP
{
    public GuestOTPEntity(ApplicationContext applicationContext)
        : base(applicationContext)
    {
    }
}
