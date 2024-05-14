using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Repository.Interfaces;

namespace Cinnamon.Api.Data.Repository.DbSets;

public class DirectStudentInfoEntity : GenericEntity<DirectStudentInfo>, IDirectStudentInfo
{
    private readonly ApplicationContext applicationContext;

    public DirectStudentInfoEntity(ApplicationContext applicationContext)
        :base(applicationContext)
    {
        this.applicationContext = applicationContext;
    }
}