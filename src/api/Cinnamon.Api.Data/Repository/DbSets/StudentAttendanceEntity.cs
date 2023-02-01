using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Repository.Interfaces;

namespace Cinnamon.Api.Data.Repository.DbSets;

public class StudentAttendanceEntity : GenericEntity<StudentAttendance>, IStudentAttendance
{
    public StudentAttendanceEntity(ApplicationContext applicationContext)
        : base(applicationContext)
    {
    }
}