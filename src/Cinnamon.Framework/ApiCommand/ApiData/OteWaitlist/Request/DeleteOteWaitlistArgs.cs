using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.OteWaitlist.Request;

public class DeleteOteWaitlistArgs
{
    [Required]
    public int Id { get; set; }
}
