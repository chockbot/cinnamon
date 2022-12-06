namespace Cinnamon.Framework.Common;

public class ErrorInfo
{
    public virtual string Description { get; set; }
    public virtual string Code { get; set; }
    public virtual Exception Exception { get; set; }
}