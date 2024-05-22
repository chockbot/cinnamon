using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.DirectStudent.Request;

public class GetDirectStudentsPaymentArgs
{
    [Required]
    public int? ProviderId { get; set; }
    [Required]
    public string DateFrom { get; set; }
    // date format must yyyyMMddHHmmss
}
