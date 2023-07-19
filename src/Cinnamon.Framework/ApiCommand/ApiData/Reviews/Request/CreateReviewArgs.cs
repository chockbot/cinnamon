using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.Reviews.Request;

public class CreateReviewArgs
{
    [Required]
    public int CustomerId { get; set; }
    [Required]
    public int MakerId { get; set; }
    [Required]
    public int ActivityId { get; set; }
    [Required]
    public int ScheduleId { get; set; }
    [Required]
    public int StudentId { get; set; }
    [Required]
    public int Rating { get; set; }
    [Required]
    public string Review { get; set; }
    [Required]
    public DateTime ReviewDate { get; set; }
}