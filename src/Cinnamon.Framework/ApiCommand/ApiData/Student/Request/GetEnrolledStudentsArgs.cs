namespace Cinnamon.Framework.ApiCommand.ApiData.Student.Request;

public class GetEnrolledStudentsArgs
{
    public int? PageIndex { get; set; }
    public int? CountPerPage { get; set; }
    public int ProviderId { get; set; }
    public string? SearchValue { get; set; }
    public int? SearchBy { get; set; }
}
