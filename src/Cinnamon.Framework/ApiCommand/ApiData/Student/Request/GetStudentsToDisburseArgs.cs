using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.Student.Request;

public class GetStudentsToDisburseArgs 
{
    public bool? IsInclusive {get; set;}
}