using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Repository.Interfaces;

namespace Cinnamon.Api.Data.Repository.DbSets;

public class DirectStudentSessionEntity : GenericEntity<DirectStudentSession>, IDirectStudentSession
{
    private readonly ApplicationContext applicationContext;

    public DirectStudentSessionEntity(ApplicationContext applicationContext)
        : base(applicationContext)
    {
        this.applicationContext = applicationContext;
    }
}