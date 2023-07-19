using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.Reviews.Request;

public class UpdateReviewArgs
{
    [Required]
    public int Id { get; set; }

    public int CustomerId { get; set; }

    public int MakerId { get; set; }

    public int ActivityId { get; set; }

    public int ScheduleId { get; set; }

    public int StudentId { get; set; }

    public int Rating { get; set; }

    public string Review { get; set; }

    public DateTime ReviewDate { get; set; }
}