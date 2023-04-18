using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Repository.Interfaces;

namespace Cinnamon.Api.Data.Repository.DbSets;

public class StudentEntity : GenericEntity<Student>, IStudent
{
    public StudentEntity(ApplicationContext applicationContext)
        : base(applicationContext)
    {
    }
}