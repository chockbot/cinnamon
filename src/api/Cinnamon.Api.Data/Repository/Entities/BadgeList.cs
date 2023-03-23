namespace Cinnamon.Api.Data.Repository.Entities;

public class BadgeList : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int NumberOfEnrolledStudent { get; set; }
    public int NumberOfCompletedStudent { get; set; }
    public int NumberOfReviews { get; set; }
    public string ImgScr { get; set; }
}
