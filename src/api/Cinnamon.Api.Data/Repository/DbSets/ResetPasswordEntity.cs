using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Repository.Interfaces;

namespace Cinnamon.Api.Data.Repository.DbSets;

public class ResetPasswordEntity : GenericEntity<ResetPassword>, IResetPassword
{
    private readonly ApplicationContext applicationContext;

    public ResetPasswordEntity(ApplicationContext applicationContext)
        : base(applicationContext)
    {
        this.applicationContext = applicationContext;
    }
}