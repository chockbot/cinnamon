namespace Cinnamon.Framework.ApiCommand.ApiData.Student.Request;

public class GetCompletedStudentsByIdArgs
{
    public int? PageIndex { get; set; }
    public int? CountPerPage { get; set; }
    public int CustomerId { get; set; }
}
