namespace Cinnamon.Framework.ApiCommand.ApiCore;

public abstract class AbstractModel<TTarget>
{
    public TTarget Result { get; set; }
    public bool IsSuccess { get; set; }
    public ErrorInfo? ErrorInfo { get; set; }
    public Pagination? Pagination { get; set; }
}

public class Pagination
{
    public int? TotalRecords { get; set; }
    public int? PageIndex { get; set; }
    public int? PerPage { get; set; }
    public int? TotalPages { get; set; }
}

public class ErrorInfo
{
    public string Message { get; set; }
    public string Code { get; set; }
    public string Description { get; set; }
}