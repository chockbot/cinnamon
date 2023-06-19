using System.ComponentModel.DataAnnotations;
namespace Cinnamon.Framework.ApiCommand.ApiCore.Dashboard.Request;
public  class GetCompletedStudetnsArgs
{
    [Required]
    public IEnumerable<int> ActivityIds { get; set; }
}
