using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.Student.Request;

public class UpdateStudentDisbursementArgs 
{
    [Required]
    public IEnumerable<int> Ids {get; set;}
    [Required]
    public bool IsDisbursement {get; set;}
}