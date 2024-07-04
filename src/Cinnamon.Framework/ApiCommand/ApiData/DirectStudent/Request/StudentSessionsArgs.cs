using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.DirectStudent.Request;

public class StudentSessionsArgs
{
    public string? SessionStatus {get; set;}
    public int? StudentId {get; set;}
}
