using Cinnamon.Framework.Enums;
using Cinnamon.Framework.Interactor;
using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Api.Core.Services.DirectStudentService.Interactors;
public class UpdateStudentAttendanceArgs : IInteractor
{
    public DateTime Date { get; set; }
    public IEnumerable<StudentDetails> Students { get; set; }

    public class StudentDetails
    {
        public int StudentId { get; set; }
        public int ActivityId { get; set; }
        public int ScheduleId { get; set; }
        public bool IsPresent { get; set; }
        public StudentType StudentType { get; set; }
    }
}
