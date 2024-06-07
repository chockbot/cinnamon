using Cinnamon.Framework.Interactor;
namespace Cinnamon.Api.Core.Services.DirectStudentService.Interactors;
public class CreateDirectStudentAttendanceArgs : IInteractor
{
    public IEnumerable<CreateStudentAttendance> CreateDirectStudentsAttendance { get; set; }

    public class CreateStudentAttendance
    {
        public int StudentId { get; set; }
        public bool IsPresent { get; set; }
        public DateTime AttendanceDate { get; set; }
    }
}
