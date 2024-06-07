namespace Cinnamon.Api.Core.Services.DirectStudentService.Interactors.Results;
public class UpdateStudentAttendanceResult
{
    public IEnumerable<UpdatedStudentDetails> StudentAttendaces { get; set; }
    public class UpdatedStudentDetails
    {
        public int StudentId { get; set; }
        public bool IsPresent { get; set; }
        public DateTime Date { get; set; }
    }
}
