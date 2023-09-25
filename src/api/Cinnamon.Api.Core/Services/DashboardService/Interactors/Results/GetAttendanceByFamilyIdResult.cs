using Cinnamon.Framework.ApiCommand.ApiCore.DTO.Student;
namespace Cinnamon.Api.Core.Services.OnGoingActivityService.Interactors.Results;
public class GetAttendanceByFamilyIdResult
{
    public IEnumerable<StudentAttendace> studentAttendaces { get; set; }

    public class StudentAttendace
    {
        public int Id { get; set; }
        public string StudentName { get; set; }
        public int NumberOfSessions { get; set; }
        public int SessionsAttended { get; set; }
        public int ActivityId { get; set; }
        public int ScheduleId { get; set; }
        public int CustomerId { get; set; }
        public int FamilyId { get; set; }
        public bool IsPresent { get; set; }
        public DateTime AttendanceDate { get; set; }
    }
}
