using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Repository.Interfaces;

namespace Cinnamon.Api.Data.Repository.DbSets;

public class DirectStudentPaymentEntity : GenericEntity<DirectStudentPayment>, IDirectStudentPayment
{
    private readonly ApplicationContext applicationContext;

    public DirectStudentPaymentEntity(ApplicationContext applicationContext)
        : base(applicationContext)
    {
        this.applicationContext = applicationContext;
    }
}