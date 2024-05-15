using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Repository.Interfaces;

namespace Cinnamon.Api.Data.Repository.DbSets;

public class DirectStudentAttendanceEntity : GenericEntity<DirectStudentAttendance>, IDirectStudentAttendance
{
    private readonly ApplicationContext applicationContext;

    public DirectStudentAttendanceEntity(ApplicationContext applicationContext)
        :base(applicationContext)
    {
        this.applicationContext = applicationContext;
    }
}